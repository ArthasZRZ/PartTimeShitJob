using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Microsoft.Windows.Controls.Ribbon;
using Microsoft.Win32;
using System.IO;

namespace WpfRibbonApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public static WorkSpaceClass WorkSpaceInstance = null;
        public static NoticeFromBuilding NoticeInstance = null;
        public static Models.StoreDB storeDB = null;
        public MainWindow()
        {
            InitializeComponent();
            WorkSpaceInstance = new WorkSpaceClass();

            string test_string = "check";
            NoticeInstance = new NoticeFromBuilding(test_string);
            storeDB = new Models.StoreDB();
            // Insert code required on object creation below this point.
        }

        // This is a test function
        public void OpenProjButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            OpenProjWindow.GetInstance().ShowDialog();               
        }

        private void BuildNewProj_Click(object sender, RoutedEventArgs e)
        {
            BuildNewProjWindow.GetInstance().ShowDialog();
        }

        private void ImportModel_Click(object sender, RoutedEventArgs e)
        {
            ImportModelWindow.GetInstance().ShowDialog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //_3DModel.GetInstance().ShowDialog();
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WorkSpaceInstance.TowerModelInstance.RunQhullCmd(@"E:\tower.asc", @"E:\result.off");
        }

        private void button1_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            WorkSpaceInstance.TowerModelInstance.Build3DPointCloud();
        }

        private void WorkingStatusButtonClick(object sender, RoutedEventArgs e)
        {
            WorkingDataImporterWindow newWindow = new WorkingDataImporterWindow();
            newWindow.Show();
        }

        private void VirtualHeat_Click_1(object sender, RoutedEventArgs e)
        {
            HeatDoubleImporter hdwindow = new HeatDoubleImporter();
            hdwindow.Show();
        }
    }

    public class NoticeFromBuilding
    {
        private string column1_private;
        public string column1
        {
            set{ column1_private = value; }
            get{ return column1_private; }
        }

        public NoticeFromBuilding(string column1_private)
        {
            this.column1_private = column1_private;
        }
    }
}

