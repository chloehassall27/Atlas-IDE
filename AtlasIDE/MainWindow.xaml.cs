using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AtlasIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public ICollectionView view;
        Relationship rel;
        Service serv;
        App app = new App();
        List<App> appList = new List<App>();
        Cond_Eval cond = new Cond_Eval();
        bool initRel = false;
        System.Windows.Controls.ListBox dragSource = null;

        public MainWindow()
        {
            InitializeComponent();
            Networking.Window = this;
            Networking.Start();

            RelationshipTweet tweet = new RelationshipTweet();
            rel = new Relationship(tweet);
            rel.Name = "Test Relationship";
            rel.Category = "Category";
            rel.Description = "Test Relation";
            rel.FSname = "First service";
            rel.Owner = "Entity";
            rel.SpaceID = "123456";
            rel.SSname = "Second service";
            rel.ThingID = "6969";
            rel.Type = "Software";

            RelationshipTweet tweet2 = new RelationshipTweet();
            Relationship rel2 = new Relationship(tweet2);
            rel2.Name = "Test 2";
            rel2.Category = "Category";
            rel2.Description = "Test Relation";
            rel2.FSname = "First service";
            rel2.Owner = "Entity";
            rel2.SpaceID = "123456";
            rel2.SSname = "Second service";
            rel2.ThingID = "6969";
            rel2.Type = "Software";

            ServiceTweet servtweet = new ServiceTweet();
            serv = new Service(servtweet);
            serv.Name = "Check_Button";
            serv.EntityID = "357179";
            serv.ThingID = "6969";
            serv.SpaceID = "123456";
            serv.Vendor = "Pied_Pipers";
            serv.API = "ATLAS";
            serv.Type = "Report";
            serv.AppCategory = "category";
            serv.Description = "description";
            serv.Keywords = "button";

            App testApp = new App();
            testApp.Name = "Test App";
            lbApp.Items.Add(testApp.Name);


            lbDrop.AllowDrop = true;

            lbRelationship.Items.Add(rel.Name);
            lbRelationship.Items.Add(rel2.Name);
            lbRelationship_Copy.Items.Add(rel.Name);
            lbRelationship_Copy.Items.Add(rel2.Name);
            lbService.Items.Add(serv.Name);
            appShow(false);
            ShowRelEdit(false);
        }


        // Justin Code

        private void btRelEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowRelEdit(true);
        }


        private void btRelSave_Click(object sender, RoutedEventArgs e)
        {
            ShowRelEdit(false);
            MessageBox.Show("Relationship saved!");
        }

        void ShowRelEdit(bool show) // Controller that hides or shows edit form
        {
            System.Windows.Controls.Label[] labels = { lbRelName, lbRelOwn, lbRelCat, lbRelDes, lbRelThing, lbRelSpace, lbRelType, lbRelFirstService, lbRelSecondService };
            System.Windows.Controls.TextBox[] textBoxes = { tbRelName, tbRelOwn, tbRelCat, tbRelDescription, tbRelThing, tbRelSpace, tbRelType, tbRelFirst, tbRelSec };
            string relSelectName = "";

            if (show) // Show Relationship edit form
            {
                if (lbRelationship.SelectedItem == null)
                {
                    MessageBox.Show("Error you must select a relationship to edit!");
                    return;
                }
                relSelectName = lbRelationship.SelectedItem.ToString();
                rel = findRelationship(relSelectName);

                btRelEdit.Visibility = Visibility.Hidden;
                btRelSave.Visibility = Visibility.Visible;
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].Visibility = Visibility.Visible;
                    textBoxes[i].Visibility = Visibility.Visible;
                }

                tbRelName.Text = rel.Name;
                tbRelOwn.Text = rel.Owner;
                tbRelCat.Text = rel.Category;
                tbRelDescription.Text = rel.Description;
                tbRelThing.Text = rel.ThingID;
                tbRelSpace.Text = rel.SpaceID;
                tbRelType.Text = rel.Type;
                tbRelFirst.Text = rel.FSname;
                tbRelSec.Text = rel.SSname;

            }
            else // Don't show
            {

                btRelEdit.Visibility = Visibility.Visible;
                btRelSave.Visibility = Visibility.Hidden;

                if (initRel)
                {

                    if (lbRelationship.SelectedItem == null)
                    {
                        MessageBox.Show("Error you must select a relationship to edit!");
                        return;
                    }
                    relSelectName = lbRelationship.SelectedItem.ToString();
                    rel = findRelationship(relSelectName);

                    rel.Name = tbRelName.Text;
                    rel.Owner = tbRelOwn.Text;
                    rel.Category = tbRelCat.Text;
                    rel.Description = tbRelDescription.Text;
                    rel.ThingID = tbRelThing.Text;
                    rel.SpaceID = tbRelSpace.Text;
                    rel.Type = tbRelType.Text;
                    rel.FSname = tbRelFirst.Text;
                    rel.SSname = tbRelSec.Text;

                }
             
                initRel = true;

                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].Visibility = Visibility.Hidden;
                    textBoxes[i].Clear();
                    textBoxes[i].Visibility = Visibility.Hidden;
                }

            }
        }

        Relationship findRelationship(string name)
        {

            foreach (Thing thing in Networking.Things)
                foreach (Relationship relationship in thing.Relationships)
                    if (relationship.Name.Equals(name))
                        return relationship;

            return null;
        }

        // Source for drag and drop between listboxes -> https://www.c-sharpcorner.com/uploadfile/dpatra/drag-and-drop-item-in-listbox-in-wpf/

        private void lbRelationship_PreMouseLeftButton(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Controls.ListBox parent = (System.Windows.Controls.ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }

        }


        private static object GetDataFromListBox(System.Windows.Controls.ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = System.Windows.Media.VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }



        private void lbDrop_Drop(object sender, DragEventArgs e)
        {
            System.Windows.Controls.ListBox parent = (System.Windows.Controls.ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            dragSource.Items.Remove(data);

            parent.Items.Add(data);

        }

        private void recipe_DragOver(object sender, DragEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => tabControl.SelectedIndex = 3));
        }

        private void Relationships_DragOver(object sender, DragEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => tabControl.SelectedIndex = 2));
        }

        //Fohrman recipe tab
        private void btClear(object sender, RoutedEventArgs e)
        {
            if (lbApp.SelectedItem == null)
            {
                MessageBox.Show("Error: No App Selected!");
                return;
            }
            string select_rel = lbApp.SelectedItem.ToString();

            int index = lbApp.Items.IndexOf(select_rel);
            lbApp.Items.RemoveAt(index);
            lbAppMan.Items.RemoveAt(index);
        }

        private void btNew(object sender, RoutedEventArgs e)
        {
            appShow(true);
        }

        private void btPublish(object sender, RoutedEventArgs e)
        {
            appPublish(app);
        }

        void appPublish(App app)
        {
            if (tbAppName.Text == null || tbAppName.Text.Equals(""))
            {
                MessageBox.Show("Error: Please input name!");
                return;
            }
            if (lbRecipe.Items.Count == 0)
            {
                MessageBox.Show("Error: No Instructions!");
                return;
            }

            string appName = tbAppName.Text;
            lbApp.Items.Add(appName);
            app.Name = appName;

            appList.Add(app);
            lbAppMan.Items.Add(appName);
            if (app.Commands.Count > 0) { app.Commands.Clear(); }
            app.Name = null;
            MessageBox.Show("App Published!");
            appShow(false);
        }

        void appShow(bool show)
        {
            System.Windows.Controls.Label[] labels = { Recipe_Rel, Recipe_Serv, Recipe_Editor, Recipe_Name, IF, THEN};
            System.Windows.Controls.ListBox[] listBoxes = { lbRelationship_Copy, lbService, lbRecipe, lbIF, lbTHEN };
            System.Windows.Controls.TextBox[] textBoxes = {tbAppName};
            System.Windows.Controls.Button[] buttons = { bt_Publish, bt_AddCond };

            if (show) // Show Recipe edit form
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].Visibility = Visibility.Visible;
                }
                for (int i = 0; i < listBoxes.Length; i++)
                {
                    listBoxes[i].Visibility = Visibility.Visible;
                }
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    textBoxes[i].Visibility = Visibility.Visible;
                }
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Visibility = Visibility.Visible;
                }
            }
            else // Don't show
            {
                //Create app and append to list

                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].Visibility = Visibility.Hidden;
                }
                for (int i = 0; i < listBoxes.Length; i++)
                {
                    listBoxes[i].Visibility = Visibility.Hidden;
                }
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    textBoxes[i].Visibility = Visibility.Hidden;
                }
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Visibility = Visibility.Hidden;
                }
            }
        }

        public void btAddCond(object sender, RoutedEventArgs e)
        {
            if (cond.IF == null)
            {
                MessageBox.Show("Error: Need IF Condition!");
                return;
            }
            if (cond.THEN == null)
            {
                MessageBox.Show("Error: Need THEN Statement!");
                return;
            }

            cond.IF = lbIF.Items.GetItemAt(0); //Need to get actual relationship/service
            cond.THEN = lbTHEN.Items.GetItemAt(0);

            lbRecipe.Items.Add(("IF " + lbIF.Items.GetItemAt(0) + " THEN " + lbTHEN.Items.GetItemAt(0)));
            app.Commands.Add(cond);

            cond.IF = null;
            cond.THEN = null;
            lbIF.Items.Clear();
            lbTHEN.Items.Clear();
        }

        public void Recipe_Rel_Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if(data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Copy);
            }
        }

        public void Recipe_Serv_Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Copy);
            }
        }

        public void Recipe_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            lbRecipe.Items.Add(data);
        }

        public void IF_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            if (lbIF.Items.Count > 1) { lbIF.Items.Clear(); }
            lbIF.Items.Add(data);
            cond.IF = data;
        }

        public void THEN_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            if (lbTHEN.Items.Count > 1) { lbTHEN.Items.Clear(); }
            lbTHEN.Items.Add(data);
            cond.THEN = data;
        }
        

        // Kyle Service/Thing Code

        public void UpdateThings()
        {
            thingList.ItemsSource = null;
            thingList.ItemsSource = Networking.Things;
            //thingList.UpdateLayout();
        }
        public void UpdateServices()
        {
            List<string> thingIDs = new List<string>();
            foreach (Service service in Networking.Services)
                if (!thingIDs.Contains(service.ThingID))
                    thingIDs.Add(service.ThingID);
            thingFilterList.ItemsSource = thingIDs;

            view = CollectionViewSource.GetDefaultView(Networking.Services);
            serviceList.ItemsSource = null;
            lbService.ItemsSource = null;
            serviceList.ItemsSource = view;
            lbService.ItemsSource = view;
            //serviceList.UpdateLayout();
        }


        //public void CallService(object sender, MouseButtonEventArgs e)
        //{
        //    Networking.Call((sender as StackPanel).DataContext as Service);
        //}
        
        public void UpdateRelationship()
        {
            List<string> relationships = new List<string>();
            foreach (Thing thing in Networking.Things)
                foreach (Relationship relationship in thing.Relationships)
                    relationships.Add(relationship.Name);


            lbRelationship.ItemsSource = null;
            lbRelationship_Copy.ItemsSource = null;
            lbRelationship.ItemsSource = relationships;
            lbRelationship_Copy.ItemsSource = relationships;
        }

        public void Filter()
        {
            if (thingFilterList.SelectedItems.Count > 0)
                view.Filter = (o) =>
                {
                    return thingFilterList.SelectedItems.Contains(((Service)o).ThingID);
                };
            else
                view.Filter = null;
        }

        private void FilterChangeSelection(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }


    }
}
