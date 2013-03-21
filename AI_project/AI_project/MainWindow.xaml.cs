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
            map.buildMap(tbLoc.Text, tbConn.Text);

            cbHeuristic.Items.Clear();
            cbStartCity.Items.Clear();
            cbEndCity.Items.Clear();
            cbOmitCity.Items.Clear();

            cbHeuristic.IsEnabled = true;
            cbHeuristic.Items.Add("Straight Line Distance");
            cbHeuristic.Items.Add("Fewest Links");

            cbStartCity.IsEnabled = true;
            cbEndCity.IsEnabled = true;
            cbOmitCity.IsEnabled = true;

            List<City> cities = map.getCities();
            List<string> ct = new List<string>();

            //create new list of strings of cities names
            foreach (City city in cities)
            {
                ct.Add(city.getCityName());
            }

            //sort the list and fill comboboxes
            ct.Sort();

            cbOmitCity.Items.Add("none");
           
            foreach (string str in ct)
            {
                cbStartCity.Items.Add(str);
                cbEndCity.Items.Add(str);
                cbOmitCity.Items.Add(str);                
            }

            cbHeuristic.SelectedIndex = 0;
            cbStartCity.SelectedIndex = 0;
            cbEndCity.SelectedIndex = 0;
            cbOmitCity.SelectedIndex = 0;
            
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
            txtboxPath.Clear();

           int x = map.findPath(cbStartCity.SelectedValue.ToString(), cbEndCity.SelectedValue.ToString(),
                cbOmitCity.SelectedValue.ToString(), cbHeuristic.SelectedValue.ToString());

            List<string> path = map.showPath();

            foreach (string str in path)
            {
                txtboxPath.AppendText(str + "\n");
            }
        }       
    }
}
