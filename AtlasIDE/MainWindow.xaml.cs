using System.Collections.Generic;
using System.Windows;

namespace AtlasIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Networking.Window = this;
            Networking.Start();
        }

        public void UpdateThings()
        {
            thingList.ItemsSource = Networking.Things;
        }
        public void UpdateServices()
        {
            serviceList.ItemsSource = Networking.Services;
        }
    }
}
