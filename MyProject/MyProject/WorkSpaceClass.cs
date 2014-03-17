using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfRibbonApplication1
{
    public class WorkSpaceClass
    {
        public string NLIST_FILENAME { set; get; }
        public string ELIST_FILENAME { set; get; }
        public string ROOT_DIR { set; get; }
        public string DBNAME { set; get; }
        public TowerModel TowerModelInstance = null;
        public Models.HeatDoubler HeatDoublerInstance = null;
        public WorkSpaceClass()
        {
            NLIST_FILENAME = "";
            ELIST_FILENAME = "";
            TowerModelInstance = new TowerModel();
            HeatDoublerInstance = new Models.HeatDoubler();
            ROOT_DIR = "";
        }
    }
}
