using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WpfRibbonApplication1
{
    public class ModelTypesBase
    {
        public string ModelName { set; get; }
        public int ModelNumber { set; get; }
    }
    public class ModelTypes : ObservableCollection<ModelTypesBase>
    {
        public ModelTypes()
        {
            this.Add(new ModelTypesBase { ModelNumber = 0, ModelName = "温度场模型" });
        }
    }
}
