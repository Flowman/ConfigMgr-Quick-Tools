using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ConfigMgr.QuickTools
{
    public partial class QuickDeploymentGeneralPage : SmsPageControl
    {
        public QuickDeploymentGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            Headline = "Specify collections for this deployments";
            Title = "Select Collections";
            FormTitle = "Quick Deploy Software Updates to Multiple Collections";

            InitializeComponent();

            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;

            Updater.CheckUpdates();
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            ControlsInspector.AddControl(listViewCollections, new ControlDataStateEvaluator(ValidateSelectedCollections), "Select collections for deployments");

            UtilitiesClass.UpdateListViewColumnsSize(listViewCollections, columnHeaderName);

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

        public override bool OnNavigating(NavigationType navigationType)
        {
            if (navigationType == NavigationType.Forward)
            {
                List<IResultObject> collectionList = new List<IResultObject>(listViewCollections.Items.Count);
                for (int index = 0; index < listViewCollections.Items.Count; ++index)
                    collectionList.Add(listViewCollections.Items[index].Tag as IResultObject);

                UserData["Collections"] = collectionList;
            }

            return base.OnNavigating(navigationType);
        }

        public override void OnActivated()
        {
            ControlsInspector.InspectAll();

            base.OnActivated();
        }

        public override bool OnDeactivate()
        {
            return base.OnDeactivate();
        }

        private ControlDataState ValidateSelectedCollections()
        {
            if (listViewCollections.Items.Count != 0)
                return ControlDataState.Valid;

            return ControlDataState.Invalid;
        }

        private void ButtonSelectCollections_Click(object sender, EventArgs e)
        {
            using (BrowseCollectionDialog collectionDialog = new BrowseCollectionDialog(ConnectionManager))
            {
                collectionDialog.MultiSelect = true;
                collectionDialog.CollectionType = CollectionType.Device;

                if (collectionDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                foreach (IResultObject collection in collectionDialog.SelectedCollections)
                {
                    listViewCollections.Items.Add(new ListViewItem()
                    {
                        Text = collection["Name"].StringValue,
                        SubItems = {
                            collection["CollectionID"].StringValue
                        },
                        Tag = collection
                    });
                }

                listViewCollections.Focus();
                UtilitiesClass.UpdateListViewColumnsSize(listViewCollections, columnHeaderName);
            }

            ControlsInspector.InspectAll();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            listViewCollections.Items.Clear();
            ControlsInspector.InspectAll();
        }
    }
}
