using Microsoft.ConfigurationManagement.AdminConsole;
using System;
using System.IO.Compression;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.IO;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class LegacyPackageImportPage : SmsPageControl
    {
        #region Private
        private readonly ModifyRegistry registry = new ModifyRegistry();
        private int importProgresPercent;
        #endregion

        public LegacyPackageImportPage(SmsPageData pageData)
            : base(pageData)
        {
            Title = "Select Packages";
            Headline = "Select packages to get imported";

            InitializeComponent();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            ControlsInspector.AddControl(dataGridViewDriverPackages, new ControlDataStateEvaluator(ValidateSelectedDriverPackages), "Select packages to import");
            dataGridViewDriverPackages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            Initialized = false;

            InitializeDataGridView();
        }

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                Dictionary<string, string> error = new Dictionary<string, string>();
                List<string> successful = new List<string>();

                int num = 0;
                int stepCount = 2;

                List<LegacyPackage> legacyPackages = (List<LegacyPackage>)UserData["LegacyPackageItems"];
                int totalPackages = legacyPackages.Count;

                foreach (LegacyPackage package in legacyPackages)
                {
                    int startProgress = num * 100 / totalPackages;
                    worker.ReportProgress(startProgress, string.Format("Importing Package: {0}", package.Name));

                    if (Directory.Exists(package.Target))
                        Directory.Delete(package.Target, true);

                    if (package.Create())
                    {
                        importProgresPercent = 100 / stepCount / totalPackages * 1;

                        if (registry.ReadBool("LegacyPackageZipContent"))
                        {
                            worker.ReportProgress(startProgress + (importProgresPercent), string.Format("Zipping driver source for: {0}", package.Name));

                            string zipPath = Path.Combine(package.Target, string.Format("{0}.zip", package.Name.Replace(' ', '_')));

                            ZipFile.CreateFromDirectory(package.Source, zipPath);
                        }
                        else
                        {
                            worker.ReportProgress(startProgress + (importProgresPercent), string.Format("Copying driver source for: {0}", package.Name));

                            Utility.Copy(package.Source, package.Target, true);
                        }

                        // I still hate calculating progress bars
                        importProgresPercent = 100 / stepCount / totalPackages * 2;
                        worker.ReportProgress(startProgress + (importProgresPercent), string.Format("Importing Package: {0}\n - updating distribution point", package.Name));
                        package.Package.ExecuteMethod("RefreshPkgSource", null);

                        package.CreateHashFile();
                        package.UpdatePackageVersion();
                    }
                    ++num;
                }

                PrepareCompletion();
                base.PostApply(worker, e);
            }
            catch (Exception ex)
            {
                AddRefreshResultObject(null, PropertyDataUpdateAction.RefreshAll);
                PrepareError(ex.Message);
            }
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);

            List<LegacyPackage> list = new List<LegacyPackage>();
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[columnSelected.Name].Value) == true)
                {
                    if (dataGridViewRow.Tag is LegacyPackage package)
                    {
                        list.Add(package);
                    }
                }
            }
            UserData["LegacyPackageItems"] = list;

            AddAction("GeneralDescription", string.Format("The following package(s) will be imported ({0}):", list.Count));
            AddAction("PackageInformation", string.Empty);

            foreach (LegacyPackage package in list)
                AddActionDetailMessage("PackageInformation", package.Name);
        }

        private void PrepareCompletion()
        {
            RemoveAllSummary();

            bool hasError = false;
            List<string> packageSuccess = new List<string>();
            List<LegacyPackage> packageError = new List<LegacyPackage>();
            foreach (LegacyPackage package in (List<LegacyPackage>)UserData["LegacyPackageItems"])
            {
                if (package.HasException)
                {
                    hasError = true;
                }

                if (package.HasException)
                    packageError.Add(package);
                else
                    packageSuccess.Add(package.Name);
            }

            string title = "All package(s) imported successfully.";
            AddAction("GeneralDescription", title);
            if (hasError)
            {
                title = "Some package(s) cannot be imported successfully. See following details.";
                UpdateActionStatus("GeneralDescription", SmsSummaryAction.ActionStatus.CompleteWithErrors);
            }
            UpdateAction("GeneralDescription", title);

            if (packageSuccess.Count > 0)
            {
                AddAction("PackageInformation", string.Format("The following package(s) were imported ({0}):", packageSuccess.Count));
                UpdateActionStatus("PackageInformation", SmsSummaryAction.ActionStatus.CompleteWithSuccess);

                foreach (string message in packageSuccess)
                    AddActionDetailMessage("PackageInformation", message);

                AddAction("EmptyLine", string.Empty);
            }

            if (hasError)
            {
                foreach (LegacyPackage package in packageError)
                {
                    AddAction(package.Name, package.Name);

                    if (package.Exception.Count > 0)
                    {
                        string str = string.Format(CultureInfo.CurrentCulture, "• {0}", new object[1]
                        {
                             package.Exception.FirstOrDefault().Message
                        });
                        AddActionErrorMessage(package.Name, str);
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

        private void RemoveAllSummary()
        {
            foreach (string id in Enumerable.ToList(Enumerable.Select(GetSummaryItems(), i => i.Id)))
                RemoveItem(id);
        }

        public override void OnActivated()
        {
            base.OnActivated();
        }

        public override bool OnDeactivate()
        {
            dataGridViewDriverPackages.EndEdit();
            return base.OnDeactivate();
        }

        private void InitializeDataGridView()
        {
            List<LegacyPackage> driverPackages = (List<LegacyPackage>)UserData["LegacyPackages"];

            dataGridViewDriverPackages.Rows.Clear();
            foreach (LegacyPackage package in driverPackages)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(dataGridViewDriverPackages);

                dataGridViewRow.Cells[0].Value = package.Import ? true : false;
                dataGridViewRow.Cells[1].Value = package.Name;
                dataGridViewRow.Cells[2].Value = package.FileVersion;
                dataGridViewRow.Cells[3].Value = package.Import ? "Source changed" : "No change";

                dataGridViewRow.Tag = package;
                dataGridViewDriverPackages.Rows.Add(dataGridViewRow);
            }

            Utility.UpdateDataGridViewColumnsSize(dataGridViewDriverPackages, columnPackage);

            Initialized = true;
        }

        private ControlDataState ValidateSelectedDriverPackages()
        {
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
            {
                if (Convert.ToBoolean(dataGridViewRow.Cells[0].Value, CultureInfo.InvariantCulture))
                {
                    return ControlDataState.Valid;
                }
            }
            return ControlDataState.Invalid;
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            dataGridViewDriverPackages.Focus();
            dataGridViewDriverPackages.BeginEdit(true);
            bool flag = sender == buttonSelectAll;
            foreach (DataGridViewRow dataGridViewRow in dataGridViewDriverPackages.Rows)
                dataGridViewRow.Cells[0].Value = flag ? true : false;
            dataGridViewDriverPackages.EndEdit();
        }

        private void DataGridViewDrivers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!dataGridViewDriverPackages.IsCurrentCellDirty)
                return;
            dataGridViewDriverPackages.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridViewDrivers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ControlsInspector.InspectAll();
            Dirty = !ReadOnly;
        }

        private void DataGridViewDrivers_KeyUp(object sender, KeyEventArgs e)
        {
            Utility.SelectDataGridViewWithSpace(e, (DataGridView)sender, columnSelected);
        }
    }
}
