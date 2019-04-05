using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.DialogFramework;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Net;
using IniParser;
using IniParser.Model;
using System.Management;
using System.Collections.Generic;
using Microsoft.ConfigurationManagement.AdminConsole.Common;

namespace ConfigMgr.QuickTools.DriverManager
{
    public partial class DriverPackageImportPage : SmsPageControl
    {
        #region Private
        private BackgroundWorker processWorker;
        private ModifyRegistry registry = new ModifyRegistry();
        #endregion

        public DriverPackageImportPage(SmsPageData pageData)
            : base(pageData)
        {
            Title = "Select Packages";
            Headline = "Select driver packages to get imported";

            InitializeComponent();

            panelComplete.Dock = DockStyle.Bottom;
            panelProcessing.Dock = DockStyle.Bottom;
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            dataGridViewDriverPackages.Rows.Clear();
            UtilitiesClass.UpdateDataGridViewColumnsSize(dataGridViewDriverPackages, columnPackage);

            ControlsInspector.AddControl(dataGridViewDriverPackages, new ControlDataStateEvaluator(ValidateSelectedDriverPackages), "Select driver packages to import");

            panelComplete.Visible = false;
            panelProcessing.Visible = true;

            processWorker = new BackgroundWorker();
            processWorker.DoWork += new DoWorkEventHandler(ProcessWorker_DoWork);
            processWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ProcessWorker_RunWorkerCompleted);
            processWorker.ProgressChanged += new ProgressChangedEventHandler(ProcessWorker_ProgressChanged);
            processWorker.WorkerSupportsCancellation = true;
            processWorker.WorkerReportsProgress = true;
            progressBarObjects.Value = 0;
            UseWaitCursor = true;
            processWorker.RunWorkerAsync();
            Initialized = true;
        }

        protected override void ApplyChanges()
        {
            base.ApplyChanges();
        }

        public override void PostApply(BackgroundWorker worker, DoWorkEventArgs e)
        {
            base.PostApply(worker, e);
        }

        public override void OnAddSummary(SummaryRequestHandler handler)
        {
            base.OnAddSummary(handler);
        }

        public override void OnActivated()
        {
            base.OnActivated();
        }








        private void ProcessWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            string sourceDirectory = registry.Read("DriverSourceFolder");
            string packageDirectory = registry.Read("DriverPackageFolder");

            backgroundWorker.ReportProgress(0, "Validating source folder");

            if (!Directory.Exists(packageDirectory) || !Directory.Exists(packageDirectory))
            {
                throw new InvalidOperationException("Cannot find source or destination folder. Verify that the folder exists in the specified locations.");
            }

            if (Convert.ToBoolean(registry.Read("LegacyFolderStructure")))
            {
                List<Vendor> vendors = new List<Vendor>();

                string[] subdirectoryEntries = Directory.GetDirectories(sourceDirectory, "*", SearchOption.TopDirectoryOnly);

                int num = 0;
                foreach (string vendorDirectory in subdirectoryEntries)
                {
                    string name = new DirectoryInfo(vendorDirectory).Name;
                    double start = 100 / subdirectoryEntries.Length * num;
                    backgroundWorker.ReportProgress(Convert.ToInt32(start), string.Format("Processing Driver Packages for Vendor: {0}", name));

                    Vendor vendor = new Vendor(backgroundWorker, ConnectionManager, vendorDirectory)
                    {
                        ProgressStart = start,
                        TotalVendors = subdirectoryEntries.Length
                    };

                    vendors.Add(vendor);

                    ++num;
                }
            }
            else
            {

            }
        }

        private void ProcessWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ((sender as BackgroundWorker).CancellationPending)
                return;
            progressBarObjects.Value = e.ProgressPercentage;
            labelProcessingObject.Text = (e.UserState as string);
        }

        private void ProcessWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    using (SccmExceptionDialog sccmExceptionDialog = new SccmExceptionDialog(e.Error))
                    {
                        sccmExceptionDialog.ShowDialog();
                    }
                }
                else
                    Initialized = true;
            }
            finally
            {
                if (sender as BackgroundWorker == processWorker)
                {
                    processWorker.Dispose();
                    processWorker = null;
                    UseWaitCursor = false;
                    panelComplete.Visible = true;
                    panelProcessing.Visible = false;
                }
            }
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
    }
}
