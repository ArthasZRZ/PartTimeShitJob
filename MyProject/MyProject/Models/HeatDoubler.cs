using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfRibbonApplication1.Models
{
    public class HeatDoubler
    {
        public string Name;
        public double X, Y, Z;

        public HeatDoubler() { }
        public HeatDoubler(string Name, double X, double Y, double Z)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }
}
