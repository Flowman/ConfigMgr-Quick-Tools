using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class DriverPackageImportPage : SmsPageControl
    {
        private string importSource = string.Empty;
        private BackgroundWorker backgroundWorker;

        public DriverPackageImportPage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();
            Headline = "Select driver packages to get imported";
            Title = "Select Drivers";
            panelComplete.Dock = DockStyle.Fill;
            panelProcessing.Dock = DockStyle.Fill;
            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            ControlsInspector.AddControl(dataGridViewDriverPackages, new ControlDataStateEvaluator(ValidateSelectedDriverPackages), "Select driver packages to import");
        }

        public override void OnActivated()
        {
            base.OnActivated();
            string b = UserData["ImportSourceLocation"] as string;
            if (string.Equals(importSource, b, StringComparison.OrdinalIgnoreCase))
                return;
            importSource = b;

            if (backgroundWorker != null)
                backgroundWorker.CancelAsync();

            Initialized = false;
            Cursor = Cursors.WaitCursor;
            dataGridViewDriverPackages.Rows.Clear();
            panelComplete.Visible = false;
            panelProcessing.Visible = true;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(infoWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(infoWorker_RunWorkerCompleted);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(infoWorker_ProgressChanged);
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;
            progressBarObjects.Value = 0;
            backgroundWorker.RunWorkerAsync();
        }

        public override bool OnDeactivate()
        {
            dataGridViewDriverPackages.EndEdit();
            return base.OnDeactivate();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            bool flag = false;
            try
            {
                int num = 0;
                List<DriverPackage> driverPackages = (List<DriverPackage>)UserData["DriverPackageItems"];
                foreach (DriverPackage driverPackage in driverPackages)
                {
                    worker.ReportProgress(num * 100 / driverPackages.Count, string.Format("Importing Driver Package: {0}", driverPackage.Name));
                    // create the dirver package category and package instanc 
                    driverPackage.Create();

                    if (!driverPackage.HasError)
                    {
                        string objectPath = null;
                        try
                        {
                            objectPath = driverPackage.Package["__RELPath"].StringValue;
                            Utility.RequestLock(ConnectionManager, objectPath);
                            bool refreshDP = false;
                            // parse ini files and create a dictinonary with the results
                            foreach (string inf in driverPackage.Infs)
                            {
                                worker.ReportProgress(num * 100 / driverPackages.Count, string.Format("Importing Driver Package: {0}\n - processing inf '{1}'", new object[2]
                                    {
                                        driverPackage.Name, 
                                        Path.GetFileName(inf)
                                    }));

                                Driver iniDriver = new Driver(inf);

                                if (iniDriver.HasWarning)
                                {
                                    driverPackage.ImportWarning[inf] = iniDriver.Warning.Message;
                                }
                                else if (iniDriver.HasError)
                                {
                                    driverPackage.ImportError[inf] = iniDriver.Error.Message;
                                }
                                else
                                {
                                    driverPackage.Drivers[Path.GetFileName(inf)] = iniDriver;
                                }
                            }

                            // check if driver is in driver package and same version
                            foreach (IResultObject driverObject in driverPackage.GetDriversFromPackge())
                            {
                                worker.ReportProgress(num * 100 / driverPackages.Count, string.Format("Importing Driver Package: {0}\n - retriving driver '{1} ({2})' from package", new object[3]
                                    {
                                        driverPackage.Name,
                                        driverObject["LocalizedDisplayName"].StringValue,
                                        driverObject["DriverVersion"].StringValue
                                    }));

                                Driver driver;
                                if (driverPackage.Drivers.TryGetValue(driverObject["DriverINFFile"].StringValue, out driver))
                                {
                                    if (driver.Version == driverObject["DriverVersion"].StringValue)
                                    {
                                        driver.AddObject(driverObject);

                                        if (driverPackage.DriverContentExists(driver))
                                        {
                                            driverPackage.AddDriverCategory(driver);
                                            driver.Import = false;
                                        }
                                    }
                                }
                                else
                                {
                                    worker.ReportProgress(num * 100 / driverPackages.Count, string.Format("Importing Driver Package: {0}\n - removing driver: {1} ({2})", new object[3]
                                        {
                                            driverPackage.Name,
                                            driverObject["LocalizedDisplayName"].StringValue,
                                            driverObject["DriverVersion"].StringValue
                                        }));
                                    // remove from driver package and category if source folder have been removed
                                    if (!driverPackage.DriverContentExists(driverObject))
                                    {
                                        driverPackage.RemoveDriverCategory(driverObject);
                                        driverPackage.RemoveDriverFromDriverPack(driverObject);
                                    }

                                    refreshDP = true;
                                }

                            }

                            int num2 = 1;
                            bool refreshPackage = false;
                            foreach (KeyValuePair<string, Driver> driver in driverPackage.Drivers)
                            {
                                if (driver.Value.Import)
                                {
                                    worker.ReportProgress(num * 100 / driverPackages.Count, string.Format("Importing Driver Package: {0}\n - importing and adding driver to category ({1}/{2}): {3} ({4})", new object[5]
                                        {
                                            driverPackage.Name,
                                            num2,
                                            driverPackage.Drivers.Count,
                                            driver.Value.Model,
                                            driver.Value.Version
                                        }));
                                    if (driver.Value.CreateObjectFromInfFile(ConnectionManager))
                                    {
                                        // add category to driver object
                                        driverPackage.AddDriverCategory(driver.Value);
                                    }
                                    refreshPackage = true;
                                    refreshDP = true;
                                }
                                ++num2;
                            }

                            if (refreshPackage)
                            {
                                worker.ReportProgress(num * 100 / driverPackages.Count, string.Format("Importing Driver Package: {0}\n - adding drivers to package", driverPackage.Name));
                                // add all drivers in the drivers list to the driver package
                                driverPackage.AddDriversToDriverPack();
                            }

                            if (refreshDP)
                            {
                                worker.ReportProgress(num * 100 / driverPackages.Count, string.Format("Importing Driver Package: {0}\n - updating distribution point", driverPackage.Name));
                                driverPackage.Package.ExecuteMethod("RefreshPkgSource", null);
                            }

                            driverPackage.CreateHashFile();
                        }
                        finally
                        {
                            Utility.ReleaseLock(ConnectionManager, objectPath);
                        }
                        ++num;
                    }
                }

                PrepareCompletion();
                AddRefreshResultObject(null, PropertyDataUpdateAction.RefreshAll);
                base.PostApply(worker, e);

            }
            catch (Exception ex)
            {
                AddRefreshResultObject(null, PropertyDataUpdateAction.RefreshAll);
                PrepareError(ex.Message);
                throw;
            }
            finally
            {
                int num = flag ? 1 : 0;
            }
        }

        private void infoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            bool result = true;
            int num = 0;
            string importFolder = UserData["ImportSourceLocation"] as string;

            try
            {
                backgroundWorker.ReportProgress(0, "Validating source folder");
                if (!Directory.Exists(importFolder))
                {
                    throw new InvalidOperationException("Configuration Manager cannot process the specified driver folder. Verify that the folder exists in the specified location.");
                }

                List<Vendor> vendors = new List<Vendor>();

                string[] subdirectoryEntries = Directory.GetDirectories(importFolder, "*", SearchOption.TopDirectoryOnly);

                foreach (string vendorDirectory in subdirectoryEntries)
                {
                    int percent = num * 100 / subdirectoryEntries.Length;

                    backgroundWorker.ReportProgress(percent, string.Format("Processing Driver Packages for Vendor: {0}", new DirectoryInfo(vendorDirectory).Name));

                    Vendor vendor = new Vendor(backgroundWorker, percent, ConnectionManager, vendorDirectory, (string)UserData["ImportPackageLocation"]);

                    vendors.Add(vendor);

                    ++num;
                }

                UserData["DriverVendorsInfo"] = vendors;

                backgroundWorker.ReportProgress(100, "Done");
            }
            catch
            {
                result = false;
            }
            if (backgroundWorker.CancellationPending)
            {
                backgroundWorker.Dispose();
                e.Cancel = true;
            }
            else
                e.Result = result;
        }

        private void infoWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((sender as BackgroundWorker).CancellationPending)
                return;
            progressBarObjects.Value = e.ProgressPercentage;
            labelProcessingObject.Text = (e.UserState as string);
        }

        private void infoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                    {
                        int num = (int)sccmExceptionDialog.ShowDialog();
                    }
                }
                else if (e.Cancelled)
                {
                    ConnectionManagerBase.SmsTraceSource.TraceInformation("User canceled to retrieve objects");
                }
                else
                {
                    InitializeDataGridView();
                }
            }
            finally
            {
                if (sender as BackgroundWorker == backgroundWorker)
                {
                    backgroundWorker.Dispose();
                    backgroundWorker = null;
                    Cursor = Cursors.Default;
                    panelComplete.Visible = true;
                    panelProcessing.Visible = false;
                    Initialized = true;
                    ControlsInspector.InspectAll();
                }
            }
        }

        private ControlDataState ValidateSelectedDriverPackages()
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[0].Value, CultureInfo.InvariantCulture))
                    return ControlDataState.Valid;
            }
            return ControlDataState.Invalid;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            dataGridViewDriverPackages.BeginEdit(true);
            bool flag = sender == buttonSelectAll;
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
                dataGridViewRow.Cells[0].Value = flag ? true : false;
            dataGridViewDriverPackages.EndEdit();
        }

        private void dataGridViewDriverPackages_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!dataGridViewDriverPackages.IsCurrentCellDirty)
                return;
            dataGridViewDriverPackages.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridViewDriverPackages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ControlsInspector.InspectAll();
            Dirty = !ReadOnly;
        }

        private void dataGridViewDriverPackages_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Utility.UpdateDataGridViewColumnsSize(dataGridViewDriverPackages, columnPackage);
        }

        private void InitializeDataGridView()
        {
            dataGridViewDriverPackages.Rows.Clear();
            foreach (Vendor vendor in (List<Vendor>)UserData["DriverVendorsInfo"])
            {
                foreach (DriverPackage package in vendor.Packages)
                {
                    DataGridViewRow dataGridViewRow = new DataGridViewRow();
                    dataGridViewRow.CreateCells(dataGridViewDriverPackages);

                    dataGridViewRow.Cells[0].Value = package.Import ? true : false;
                    dataGridViewRow.Cells[1].Value = package.Name;
                    dataGridViewRow.Cells[2].Value = package.Import ? string.Format("Found {0} drivers", package.Infs.Length) : "No change";

                    dataGridViewRow.Tag = package;
                    dataGridViewDriverPackages.Rows.Add(dataGridViewRow);
                }
            }
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);
            PrepareSummary();
        }

        private void RemoveAllSummary()
        {
            foreach (string id in Enumerable.ToList(Enumerable.Select(GetSummaryItems(), i => i.Id)))
                RemoveItem(id);
        }

        private void PrepareSummary()
        {
            RemoveAllSummary();

            AddAction("GeneralDescription", "Verify the following driver package(s) for importing.");
            AddAction("EmptyLine", string.Empty);

            List<DriverPackage> list = new List<DriverPackage>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnImport.Name].Value) == true)
                {
                    DriverPackage package = dataGridViewRow.Tag as DriverPackage;
                    if (package != null)
                    {
                        list.Add(package);
                    }
                }
            }
            UserData["DriverPackageItems"] = list;
            // Create a new setion in summary
            AddAction("PackageInformation", string.Format("The following driver package(s) will be imported ({0}):", list.Count));
            // Create a new row in section
            foreach (DriverPackage package in (IEnumerable)list)
                AddActionDetailMessage("PackageInformation", package.Name);
        }

        private void PrepareCompletion()
        {
            bool hasError = false;
            bool hasWarning = false;
            List<string> packageSuccess = new List<string>();
            List<DriverPackage> packageError = new List<DriverPackage>();
            foreach (DriverPackage package in (List<DriverPackage>)UserData["DriverPackageItems"])
            {
                if (package.HasDriverWarning)
                {
                    hasWarning = true;
                }
                if (package.HasError || package.HasDriverError)
                {
                    hasError = true;
                }

                if (package.HasError || package.HasDriverError || package.HasDriverWarning)
                    packageError.Add(package);
                else
                    packageSuccess.Add(package.Name);
            }

            string title = "All driver package(s) are imported successfully.";
            if (hasWarning)
            {
                title = "Some driver package(s) imported with warning. See following details.";
                UpdateActionStatus("GeneralDescription", SmsSummaryAction.ActionStatus.CompleteWithWarnings);
            }
            if (hasError)
            {
                title = "Some driver package(s) cannot be imported successfully. See following details.";
                UpdateActionStatus("GeneralDescription", SmsSummaryAction.ActionStatus.CompleteWithErrors);
            }
            UpdateAction("GeneralDescription", title);

            RemoveItem("PackageInformation");
            if (packageSuccess.Count > 0)
            {
                AddAction("PackageInformation", string.Format("The following driver package(s) were imported ({0}):", packageSuccess.Count));
                UpdateActionStatus("PackageInformation", SmsSummaryAction.ActionStatus.CompleteWithSuccess);

                foreach (string message in packageSuccess)
                    AddActionDetailMessage("PackageInformation", message);

                AddAction("EmptyLine", string.Empty);
            }

            if (hasError || hasWarning)
            {
                foreach (DriverPackage package in packageError)
                {
                    AddAction(package.Name, package.Name);

                    if (package.HasDriverWarning)
                    {
                        UpdateActionStatus(package.Name, SmsSummaryAction.ActionStatus.CompleteWithWarnings);
                    }
                    if (package.HasError || package.HasDriverError)
                    {
                        UpdateActionStatus(package.Name, SmsSummaryAction.ActionStatus.CompleteWithErrors);
                    }

                    if (package.HasDriverWarning)
                    {
                        foreach (KeyValuePair<string, string> warning in package.ImportWarning)
                        {
                            string str = string.Format("• {0}\nMessage: {1}", new object[2]
                            {
                                warning.Key.Replace((string)UserData["ImportSourceLocation"], ""),
                                warning.Value
                            });
                            AddActionWarningMessage(package.Name, str);
                        }
                    }

                    if (package.Errors.Count > 0)
                    {
                        string str = string.Format(CultureInfo.CurrentCulture, "• {0}", new object[1]
                        {
                             package.Errors.FirstOrDefault().Message
                        });
                        AddActionErrorMessage(package.Name, str);
                    }

                    if (package.HasDriverError)
                    {
                        foreach (KeyValuePair<string, string> error in package.ImportError)
                        {
                            string str = string.Format("• {0}\n{1}", new object[2]
                            {
                                error.Key.Replace((string)UserData["ImportSourceLocation"], ""),
                                error.Value
                            });
                            AddActionErrorMessage(package.Name, str);
                        }
                    }
                    AddAction("EmptyLine", string.Empty);
                }
            }
        }

        private void PrepareError(string errorMessage)
        {
            RemoveAllSummary();
            AddAction("ErrorInfo", errorMessage);
            UpdateActionStatus("ErrorInfo", SmsSummaryAction.ActionStatus.CompleteWithErrors);
        }
    }
}

