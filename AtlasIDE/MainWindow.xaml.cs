using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AtlasIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ICollectionView view;
        public MainWindow()
        {
            InitializeComponent();
            Networking.Window = this;
            Networking.Start();
        }

        public void UpdateThings()
        {
            thingList.ItemsSource = null;
            thingList.ItemsSource = Networking.Things;
            //thingList.UpdateLayout();
        }
        public void UpdateServices()
        {
            List<string> services = new List<string>();
            foreach (Service service in Networking.Services)
                if (!services.Contains(service.ThingID))
                    services.Add(service.ThingID);
            thingFilterList.ItemsSource = services;

            view = CollectionViewSource.GetDefaultView(Networking.Services);
            serviceList.ItemsSource = null;
            serviceList.ItemsSource = view;
            //serviceList.UpdateLayout();
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
