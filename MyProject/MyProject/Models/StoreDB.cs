using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WpfRibbonApplication1;
using NDatabase;

namespace WpfRibbonApplication1.Models
{   
    public class StoreDB
    {
        //private string ConnectionString = Properties.Settings.Default.StoreDataBase;

        public StoreDB() { }
        public void StoreData_VirtualHeater(HeatDoubler HD)
        {
            MessageBox.Show(MainWindow.WorkSpaceInstance.DBNAME);
            using (var odb = OdbFactory.Open(MainWindow.WorkSpaceInstance.DBNAME))
            {
                odb.Store(HD);
            }
        }
    }
}
