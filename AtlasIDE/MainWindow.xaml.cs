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

            lbDrop.AllowDrop = true;

            lbRelationship.Items.Add(rel.Name);
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

            if (show) // Show Relationship edit form
            {
                if (lbRelationship.SelectedItem == null)
                {
                    MessageBox.Show("Error you must select a relationship to edit!");
                    return;
                }
                string select_rel = lbRelationship.SelectedItem.ToString();

                int index = lbRelationship.Items.IndexOf(select_rel); // Use this later when I know exactly how I'm getting relationships

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


        private void DragOver_Recipe(object sender, DragEventArgs e)
        {

            // TBD

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
            serviceList.ItemsSource = view;
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
            lbRelationship.ItemsSource = relationships;
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
