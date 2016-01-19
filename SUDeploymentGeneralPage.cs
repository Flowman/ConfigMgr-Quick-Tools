using Microsoft.ConfigurationManagement.AdminConsole;
using Microsoft.ConfigurationManagement.AdminConsole.Common;
using Microsoft.ConfigurationManagement.ManagementProvider;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Zetta.ConfigMgr.QuickTools
{
    public partial class SUDeploymentGeneralPage : SmsPageControl
    {
        public SUDeploymentGeneralPage(SmsPageData pageData)
            : base(pageData)
        {
            InitializeComponent();
            Headline = "Specify collections for this deployments";
            Title = "Select Collections";
            pageData.ProgressBarStyle = ProgressBarStyle.Continuous;
            FormTitle = "Deploy Software Updates to Phase Collections";
        }

        public override void InitializePageControl()
        {
            base.InitializePageControl();

            ControlsInspector.AddControl(listViewListCollections, new ControlDataStateEvaluator(ValidateSelectedCollections), "Select collections for deployments");

            Initialized = true;
        }

        private ControlDataState ValidateSelectedCollections()
        {
            if (listViewListCollections.Items.Count != 0)
                return ControlDataState.Valid;

            return ControlDataState.Invalid;
        }

        private void AddCollectionsToList()
        {
            using (BrowseCollectionDialog collectionDialog = new BrowseCollectionDialog(ConnectionManager))
            {
                collectionDialog.MultiSelect = true;
                collectionDialog.CollectionType = CollectionType.Device;
                if (collectionDialog.ShowDialog(this) != DialogResult.OK)
                    return;
                foreach (IResultObject resultObject1 in collectionDialog.SelectedCollections)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Tag = resultObject1;
                    listViewItem.Text = resultObject1["Name"].StringValue;
                    listViewItem.SubItems.Add(resultObject1["CollectionID"].StringValue);

                    listViewListCollections.Items.Add(listViewItem);
                }
                listViewListCollections.Focus();
            }
        }

        private void buttonSelectCollections_Click(object sender, EventArgs e)
        {
            AddCollectionsToList();
            ControlsInspector.InspectAll();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listViewListCollections.Items.Clear();
            ControlsInspector.InspectAll();
        }

        public override void OnActivated()
        {
            ControlsInspector.InspectAll();
            base.OnActivated();
        }

        public override bool OnNavigating(NavigationType navigationType)
        {
            if (navigationType == NavigationType.Forward)
            {
                List<IResultObject> collectionList = new List<IResultObject>(listViewListCollections.Items.Count);
                for (int index = 0; index < listViewListCollections.Items.Count; ++index)
                    collectionList.Add(listViewListCollections.Items[index].Tag as IResultObject);

                UserData["Collections"] = collectionList;
            }
            return base.OnNavigating(navigationType);
        }
    }
}
