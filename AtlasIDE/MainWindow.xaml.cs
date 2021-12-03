using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //Service serv;
        App app = new App();
        bool box = true;
        List<App> appList = new List<App>();
        Cond_Eval cond = new Cond_Eval();
        bool initRel = false;
        System.Windows.Controls.ListBox dragSource = null;
        ObservableCollection<AppOutput> Outputs = new ObservableCollection<AppOutput>();

        public MainWindow()
        {
            InitializeComponent();
            Networking.Window = this;
            Networking.Start();

            /*
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

            RelationshipTweet tweet2 = new RelationshipTweet(); //adding test relationships
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
            lbAppMan.Items.Add(testApp.Name);
            */


            //lbDrop.AllowDrop = true;
            view = CollectionViewSource.GetDefaultView(Networking.ServicesCollection);
            serviceList.ItemsSource = view;
            OutputResults.ItemsSource = Outputs;

            /*
            lbRelationship_Copy.Items.Add(rel.Name);
            lbRelationship_Copy.Items.Add(rel2.Name);
            lbService.Items.Add(serv.Name);
            */

            //lbDrop.AllowDrop = true;
            view = CollectionViewSource.GetDefaultView(Networking.ServicesCollection);
            serviceList.ItemsSource = view;
            
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
            System.Windows.Controls.Label[] labels = { lbRelName, lbRelOwn, lbRelDes, lbRelThing, lbRelSpace, lbRelFirstService, lbRelSecondService, lbRelFSArgs, lbRelSSArgs };
            System.Windows.Controls.TextBox[] textBoxes = { tbRelName, tbRelOwn, tbRelDescription, tbRelThing, tbRelSpace, tbRelFirst, tbRelSec, tbRelFirstArgs, tbRelSecondArgs };
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

                lbRelType.Visibility = Visibility.Visible;
                cbType.Visibility = Visibility.Visible;

                tbRelName.Text = rel.Name;
                tbRelOwn.Text = rel.Owner;
                tbRelDescription.Text = rel.Description;

                cbType.SelectedIndex = cbType.Items.IndexOf(cbType.FindName(rel.Type));

                tbRelThing.Text = rel.ThingID;
                tbRelSpace.Text = rel.SpaceID;
                tbRelFirst.Text = rel.FSname;
                tbRelSec.Text = rel.SSname;
                tbRelFirstArgs.Text = rel.FSargs;
                tbRelSecondArgs.Text = rel.SSargs;

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
                    rel.Description = tbRelDescription.Text;
                    rel.Type = cbType.SelectedItem.ToString();
                    rel.ThingID = tbRelThing.Text;
                    rel.SpaceID = tbRelSpace.Text;
                    rel.FSname = tbRelFirst.Text;
                    rel.SSname = tbRelSec.Text;

                    rel.FSargs = tbRelFirstArgs.Text;
                    rel.SSargs = tbRelSecondArgs.Text;
                }

                UpdateRelationship();
             
                initRel = true;

                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].Visibility = Visibility.Hidden;
                    textBoxes[i].Clear();
                    textBoxes[i].Visibility = Visibility.Hidden;
                }

                lbRelType.Visibility = Visibility.Hidden;
                cbType.Visibility = Visibility.Hidden;

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
        private void btClear(object sender, RoutedEventArgs e) //Clear recipe
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
            appList.RemoveAt(index);
        }

        private void btNew(object sender, RoutedEventArgs e) //Open recipe editor
        {
            appShow(true);
            app = new App();
        }

        private void btPublish(object sender, RoutedEventArgs e) //Post recipe to list and close recipe editor
        {
            appPublish(app);
        }

        void appPublish(App app) //Recipes are published to the global List<App> appList
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
            Console.WriteLine(app.Commands.Count);

            appList.Add(app);
            lbAppMan.Items.Add(appName);
            //if (app.Commands.Count > 0) { app.Commands.Clear(); }
            lbRecipe.Items.Clear();
            lbIF.Items.Clear();
            lbTHEN.Items.Clear();
            tbAppName.Clear();
            //app.Name = null;
            MessageBox.Show("App Published!");
            appShow(false);
        }

        void appShow(bool show) //Show/hide the recipe editor.
        {
            System.Windows.Controls.Label[] labels = { Recipe_Rel, Recipe_Serv, Recipe_Editor, Recipe_Name, IF, THEN, Arg};
            System.Windows.Controls.ListBox[] listBoxes = { lbRelationship_Copy, lbService, lbRecipe, lbIF, lbTHEN };
            System.Windows.Controls.TextBox[] textBoxes = {tbAppName, tbArg};
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

        public void btAddCond(object sender, RoutedEventArgs e) //Add conditional statement to recipe. Conditionals are saved as Cond_Eval objects in the app's command list
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

            string recipeCom = "IF ";
            if (cond.IF is ServiceInstruction)
            {
                recipeCom += cond.IF.func + "(" + cond.IF.arg + ")";
            }
            else
            {
                recipeCom += cond.IF.func;
            }

            recipeCom += " THEN ";

            if(cond.THEN is ServiceInstruction)
            {
                recipeCom += cond.THEN.func + "(" + cond.THEN.arg + ")";
            }
            else
            {
                recipeCom += cond.THEN.func;
            }
            lbRecipe.Items.Add(recipeCom);
            app.Commands.Add(cond);

            cond.IF = null;
            cond.THEN = null;
            lbIF.Items.Clear();
            lbTHEN.Items.Clear();
        }

        public void Recipe_Rel_Drag(object sender, System.Windows.Input.MouseButtonEventArgs e) //Drag from relationships box in Recipes
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            box = false; //tells the recipe listbox that they are receiving from a relationship

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Copy);
            }
        }

        public void Recipe_Serv_Drag(object sender, System.Windows.Input.MouseButtonEventArgs e) //Drag from services box in Recipes (identical to above, used in case of future variation)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            box = true; //tells the recipe listbox that they are receiving from a Service, not a recipe

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Copy);
            }
        }

        public void Recipe_Drop(object sender, DragEventArgs e) //Drop into recipe box
        {
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            if (box == false) // relationship
            {
                lbRecipe.Items.Add(data);
                app.Commands.Add(new RelationshipInstruction(data.ToString()));
            }
            else //service
            {
                if (tbArg.Text == null || tbArg.Text == "")
                {
                    app.Commands.Add(new ServiceInstruction(data.ToString(), ""));
                    data += "()";
                }
                else
                {
                    app.Commands.Add(new ServiceInstruction(data.ToString(), tbArg.Text.ToString()));
                    data += "(" + tbArg.Text.ToString() + ")";
                }
                lbRecipe.Items.Add(data);
            }
        }

        public void IF_Drop(object sender, DragEventArgs e) //Drop into IF box on conditional
        {
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            if (lbIF.Items.Count > 1) { lbIF.Items.Clear(); }
            if (box == false) // relationship
            {
                lbIF.Items.Add(data);
                cond.IF = new RelationshipInstruction(data.ToString());
            }
            else //service
            {
                if (tbArg.Text == null || tbArg.Text == "")
                {
                    cond.IF = new ServiceInstruction(data.ToString(), "");
                    data += "()";
                }
                else
                {
                    cond.IF = new ServiceInstruction(data.ToString(), tbArg.Text.ToString());
                    data += "(" + tbArg.Text.ToString() + ")";
                }
                lbIF.Items.Add(data);
            }
        }

        public void THEN_Drop(object sender, DragEventArgs e) //Drop into THEN box on conditional
        {
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(string));
            if (lbTHEN.Items.Count > 1) { lbTHEN.Items.Clear(); }
            if (box == false) // relationship
            {
                lbTHEN.Items.Add(data);
                cond.THEN = new RelationshipInstruction(data.ToString());
            }
            else //service
            {
                if (tbArg.Text == null || tbArg.Text == "")
                {
                    cond.THEN = new ServiceInstruction(data.ToString(), "");
                    data += "()";
                }
                else
                {
                    cond.THEN = new ServiceInstruction(data.ToString(), tbArg.Text.ToString());
                    data += "(" + tbArg.Text.ToString() + ")";
                }
                lbTHEN.Items.Add(data);
            }
        }

        public void btActivate(object sender, RoutedEventArgs e) //Activate App in Apps tab
        {
            if (lbAppMan.SelectedItem == null)
            {
                MessageBox.Show("Error: No App Selected!");
                return;
            }

            string selection = lbApp.SelectedItem.ToString();
            int index = lbApp.Items.IndexOf(selection);

            DateTime now = DateTime.Now;
            string select_status = lbAppMan.SelectedItem.ToString() + "\t\tActive\t" + now.Hour + ":" + now.Minute + ":" + now.Second;
            lbStatus.Items.Add(select_status);

            for (int i = 0; i < appList[index].Commands.Count; i++)
            {
                Evaluate(appList[index].Commands[i]);
            }
            lbStatus.Items.Remove(select_status);
            select_status = lbAppMan.SelectedItem.ToString() + "\t\tCompleted\t" + now.Hour + ":" + now.Minute + ":" + now.Second;
            lbStatus.Items.Add(select_status);

        }
        

        // Hassall Service/Thing Code

        public void Evaluate(App app)
        {

            Console.WriteLine(app.Commands.Count);
            foreach (Command command in app.Commands)
            {
                ServiceResponseTweet res = new ServiceResponseTweet();
                if (command.GetType() == typeof(Cond_Eval))
                {

                    Command cond1 = (command as Cond_Eval).IF;
                    //Console.Write("conditional");

                    if (command.GetType() == typeof(ServiceInstruction))
                    {
                        Service service = Networking.Services.Find(x => x.Name == cond1.func);
                        if (service != null) { 
                            res = Networking.Call(service, cond1.arg);
                            Outputs.Add(new AppOutput(app.Name, res.ServiceName, res.ServiceResult));
                        }
                    }
                    else foreach (Thing thing in Networking.Things)
                        {
                            Relationship rel = thing.Relationships.Find(x => x.Name == cond1.func);
                            if (rel != null)
                            {
                                res = Networking.EvalauteRelationship(rel);
                                Outputs.Add(new AppOutput(app.Name, res.ServiceName, res.ServiceResult));
                            }
                        }

                    if (res.ServiceResult == null || res.ServiceResult == "0")
                        return;

                    Command cond2 = (command as Cond_Eval).THEN;
                    //Console.Write("conditional");

                    if (command.GetType() == typeof(ServiceInstruction))
                    {
                        Service service = Networking.Services.Find(x => x.Name == cond2.func);
                        if (service != null)
                        {
                            res = Networking.Call(service, cond2.arg);
                            Outputs.Add(new AppOutput(app.Name, res.ServiceName, res.ServiceResult));
                        }
                    }
                    else foreach (Thing thing in Networking.Things)
                        {
                            Relationship rel = thing.Relationships.Find(x => x.Name == cond2.func);
                            if (rel != null)
                            { 
                                res = Networking.EvalauteRelationship(rel);
                                Outputs.Add(new AppOutput(app.Name, res.ServiceName, res.ServiceResult));
                            }
                        }
                }

                else
                {
                    if (command.GetType() == typeof(ServiceInstruction)) { 
                        Service service = Networking.Services.Find(x => x.Name == command.func);
                        if (service != null)
                        {
                            res = Networking.Call(service, command.arg);
                            Outputs.Add(new AppOutput(app.Name, res.ServiceName, res.ServiceResult));

                        }
                    }
                    else foreach (Thing thing in Networking.Things) {
                        Relationship rel = thing.Relationships.Find(x => x.Name == (command.func as string));
                            if (rel != null)
                            {
                                res = Networking.EvalauteRelationship(rel);
                                Outputs.Add(new AppOutput(app.Name, res.ServiceName, res.ServiceResult));
                            }
                    }

                }
            }
            OutputResults.Items.Refresh();
        }
        public void UpdateThings()
        {
            thingList.ItemsSource = null;
            thingList.ItemsSource = Networking.Things;
            //thingList.UpdateLayout();
        }
        public void UpdateServices()
        {
            List<string> thingIDs = new List<string>();
            foreach (Service service in Networking.ServicesCollection)
                if (!thingIDs.Contains(service.ThingID))
                    thingIDs.Add(service.ThingID);
            thingFilterList.ItemsSource = thingIDs;

            List<string> services = new List<string>();
            foreach (Service service in Networking.ServicesCollection)
                services.Add(service.Name);

            view = CollectionViewSource.GetDefaultView(Networking.ServicesCollection);
            serviceList.Items.Refresh();
            lbService.ItemsSource = services;
            //serviceList.UpdateLayout();
        }


        //public void CallService(object sender, MouseButtonEventArgs e)
        //{
        //    Networking.Call((sender as StackPanel).DataContext as Service);
        //}
        
        public void UpdateRelationship()
        {
            List<string> thingIDs = new List<string>();
            foreach (Thing thing in Networking.Things)
                foreach (Relationship relationship in thing.Relationships)
                    if (!thingIDs.Contains(relationship.ThingID))
                        thingIDs.Add(relationship.ThingID);
            thingFilterList.ItemsSource = thingIDs;

            List<string> relationships = new List<string>();
            foreach (Thing thing in Networking.Things)
                foreach (Relationship relationship in thing.Relationships)
                    relationships.Add(relationship.Name);

            view = CollectionViewSource.GetDefaultView(Networking.RelationshipCollection);

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
