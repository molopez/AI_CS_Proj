using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI_project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Map map = new Map();

        public MainWindow()
        {
            InitializeComponent();

            cbHeuristic.IsEnabled = false;
            cbStartCity.IsEnabled = false;
            cbEndCity.IsEnabled = false;
            cbOmitCity.IsEnabled = false;

            btnFindPath.IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                tbLoc.Text = filename;
            }
        }

        private void btnConn_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                tbConn.Text = filename;
            }
        }

        private void btnBuildMap_Click(object sender, RoutedEventArgs e)
        {
            //string loc = tbLoc.Text;
            //string conn = tbConn.Text;

            map.buildMap(tbLoc.Text, tbConn.Text);

            cbHeuristic.IsEnabled = true;
            cbHeuristic.Items.Add("Straight Line Distance");
            cbHeuristic.Items.Add("Fewest Links");

            cbStartCity.IsEnabled = true;
            cbEndCity.IsEnabled = true;
            cbOmitCity.IsEnabled = true;

            List<string> cities = map.getCities();

            foreach (string city in cities)
            {
                cbStartCity.Items.Add(city);
                cbEndCity.Items.Add(city);
                cbOmitCity.Items.Add(city);                
            }
        }

        private void cbHeuristic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbHeuristic.SelectedIndex != -1 && cbStartCity.SelectedIndex != -1
                && cbEndCity.SelectedIndex != -1 && cbOmitCity.SelectedIndex != -1)
            {
                btnFindPath.IsEnabled = true;
            }
        }

        private void cbStartCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbHeuristic.SelectedIndex != -1 && cbStartCity.SelectedIndex != -1
                && cbEndCity.SelectedIndex != -1 && cbOmitCity.SelectedIndex != -1)
            {
                btnFindPath.IsEnabled = true;
            }
        }

        private void cbEndCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbHeuristic.SelectedIndex != -1 && cbStartCity.SelectedIndex != -1
                && cbEndCity.SelectedIndex != -1 && cbOmitCity.SelectedIndex != -1)
            {
                btnFindPath.IsEnabled = true;
            }
        }

        private void cbOmitCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbHeuristic.SelectedIndex != -1 && cbStartCity.SelectedIndex != -1
                && cbEndCity.SelectedIndex != -1 && cbOmitCity.SelectedIndex != -1)
            {
                btnFindPath.IsEnabled = true;
            }
        }

        private void btnFindPath_Click(object sender, RoutedEventArgs e)
        {
            map.findPath(cbStartCity.SelectedValue.ToString(), cbEndCity.SelectedValue.ToString(),
                cbOmitCity.SelectedValue.ToString(), cbHeuristic.SelectedValue.ToString());
        }       
    }
}
