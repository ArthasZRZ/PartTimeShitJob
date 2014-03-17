using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using Com.StellmanGreene.CSVReader;
using NDatabase;

namespace WpfRibbonApplication1
{

    /// <summary>
    /// HeatDoubleImporter.xaml 的交互逻辑
    /// </summary>
    public partial class HeatDoubleImporter : Window
    {
        public HeatDoubleImporter()
        {
            InitializeComponent();
        }

        private void BtnImporter_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog opfile = new System.Windows.Forms.OpenFileDialog();
            opfile.Filter="CSV Files (*.csv)|*.csv";
            opfile.RestoreDirectory = true;
            if (opfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = opfile.FileName;
                DataTable dbtable = CSVReader.ReadCSVFile(filename, true);
                DataRow[] dbrow = dbtable.Select();
                foreach (DataRow dr in dbrow)
                {
                    Models.HeatDoubler hdrow = new Models.HeatDoubler(dr[0].ToString(), System.Convert.ToDouble(dr[1]),
                        System.Convert.ToDouble(dr[2]), System.Convert.ToDouble(dr[3]));
                    MainWindow.storeDB.StoreData_VirtualHeater(hdrow);
                }
                //ShowImportedData.ItemsSource = dbtable.AsDataView();
            }
        }
    }
}
