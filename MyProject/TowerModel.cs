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
using Kitware.VTK;

namespace WpfRibbonApplication1
{
    public class ListElemBase
    {
        public int Elem_Number;
        public int[] Elem_Modeltype;
        public int[] Elem_Nodes;
        public ListElemBase(int ElemNumber, int[] M, int[] N)
        {
            Elem_Modeltype = new int[5];
            Elem_Nodes = new int[8];

            Elem_Number = ElemNumber;
            for(int i = 0; i < 5; i++){
                Elem_Modeltype[i] = M[i];
            }
            for(int i = 0; i < 8; i++){
                Elem_Nodes[i] = N[i];
            }
        }
    }
    public class ListNodeBase
    {
        public int Node_id;
        public int Node_Number;
        public double[] Node_Coord;
        public ListNodeBase(int Nodeid, int NodeNumber, double X, double Y, double Z)
        {
            Node_Coord = new double[3];
            Node_Coord[0] = X;
            Node_Coord[1] = Y;
            Node_Coord[2] = Z;
            Node_Number = NodeNumber;
            Node_id = Nodeid;
        }
    }
    public class NodeListCompare : IComparer<ListNodeBase>
    {
        public NodeListCompare()
        {

        }
        public int Compare(ListNodeBase x, ListNodeBase y)
        {
            if (x.Node_Coord[1] == y.Node_Coord[1])
                return x.Node_Coord[0].CompareTo(y.Node_Coord[0]);
            else
                return x.Node_Coord[1].CompareTo(y.Node_Coord[1]);
        }
    }
    public class BoundaryLine
    {
        public int x, y;
        public BoundaryLine(int x, int y) { 
            this.x = x; 
            this.y = y; 
        }
    }
    public class TowerModel 
    {
        List<ListElemBase> ElemList;
        List<ListNodeBase> NodeList;
        List<BoundaryLine> Boundary2D;

        Dictionary<int, int> NodeElemTable;
        int ElemListSize, NodeListSize;
        int id;

        int Cloud2DWriteMark = 0;

        public TowerModel()
        {
            id = 0;
        }
        public TowerModel(TowerModel prevTowerModel)
        {
            ElemList = new List<ListElemBase>(prevTowerModel.ElemList.ToArray());
            NodeList = new List<ListNodeBase>(prevTowerModel.NodeList.ToArray());
            Boundary2D = new List<BoundaryLine>(prevTowerModel.Boundary2D.ToArray());
            NodeElemTable = new Dictionary<int, int>(prevTowerModel.NodeElemTable);
            ElemListSize = prevTowerModel.ElemListSize;
            NodeListSize = prevTowerModel.NodeListSize;

            id = 1;
        }
        public void ImportModel(string fnNode, string fnEle)
        {
            ElemList = new List<ListElemBase>();
            NodeList = new List<ListNodeBase>();
            NodeElemTable = new Dictionary<int, int>();

            FileStream fsNode = new FileStream(fnNode, FileMode.Open, FileAccess.Read);
            FileStream fsElem = new FileStream(fnEle, FileMode.Open, FileAccess.Read);
            StreamReader srNode = new StreamReader(fsNode);
            StreamReader srElem = new StreamReader(fsElem);
            srNode.BaseStream.Seek(0, SeekOrigin.Begin);
            srElem.BaseStream.Seek(0, SeekOrigin.Begin);

            NodeListSize = ElemListSize = 0;

            // iterate NLIST.lis
            string tmpNode = srNode.ReadLine();
            int NodeBegin = 0;
            while (tmpNode != null)
            {
                string[] tmpNodeSplit = Regex.Split(tmpNode.Trim(), " ", RegexOptions.IgnoreCase);
                if (tmpNodeSplit[0] == "NODE")
                {
                    NodeBegin = 1;
                }
                else if(NodeBegin == 1 && tmpNodeSplit[0] != "")
                {
                    int NodeNumber = int.Parse(tmpNodeSplit[0]);
                    
                    double[] Coord = new double[3];
                    int cnt = 0;
                    int start = 0;
                    foreach (string str in tmpNodeSplit)
                    {
                        if (start == 0)
                            start = 1;
                        else if (str != "")
                        {
                            Coord[cnt] = System.Convert.ToDouble(str);
                            cnt++;
                        }
                    }
                    NodeList.Add(new ListNodeBase(NodeListSize, NodeNumber, Coord[0], Coord[1], Coord[2]));
                    NodeListSize ++;
                }
                tmpNode = srNode.ReadLine();
            }
            NodeListCompare NLComparer = new NodeListCompare();
            NodeList.Sort(NLComparer.Compare);
            int cntn = 1;
            foreach (ListNodeBase node in NodeList)
            {
                NodeElemTable.Add(node.Node_Number, cntn);
                cntn++;
            }
            

            // iterate ELIST.lis
            string tmpElem = srElem.ReadLine();
            int ElemBegin = 0;
            while (tmpElem != null)
            {
                string[] tmpElemSplit = Regex.Split(tmpElem.Trim(), " ", RegexOptions.IgnoreCase);
                if (tmpElemSplit[0] == "ELEM")
                {
                    ElemBegin = 1;
                }
                else if (ElemBegin == 1 && tmpElemSplit[0] != "")
                {
                    int ElemNumber = int.Parse(tmpElemSplit[0]);
                    int[] M = new int[5];
                    int[] N = new int[8];
                    int cntM = 0, cntN = 0;
                    int start = 0;
                    foreach (string str in tmpElemSplit)
                    {
                        if (start == 0)
                            start++;
                        else if (str != "")
                        {
                            if (cntM == 5)
                            {
                                N[cntN] = int.Parse(str);
                                cntN++;
                            }
                            else
                            {
                                M[cntM] = int.Parse(str);
                                cntM++;
                            }
                        }
                    }
                    
                    ElemList.Add(new ListElemBase(ElemNumber, M, N));
                    ElemListSize ++;
                }
                tmpElem = srElem.ReadLine();
            }

            //MessageBox.Show(ElemListSize.ToString() + " " + NodeListSize.ToString());
            //DrawModel();
            MessageBox.Show("Done");
            //System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\testttt.txt", true);
            //BuildPointCloud();
        }

        public int cnt = 0;
        public void RunQhullCmd(string filein, string fileout)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(@"E:\qconvex.exe");
            string param = "w";
            psi.Arguments = param;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;

            //psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;

            System.Diagnostics.Process listFiles = new Process();
            listFiles.StartInfo = psi;
            //listFiles.OutputDataReceived += new DataReceivedEventHandler(GetDataOutputHandler);

            double pi = Math.PI;
            double range = pi;
            double slice = 10;
            double dis = range / slice;

            int dim = 3;
            
            
            try
            {
                listFiles.Start();
                //listFiles.BeginOutputReadLine();

                StreamWriter sw = listFiles.StandardInput;
                
                sw.WriteLine(dim.ToString());
                sw.WriteLine(NodeListSize.ToString());

                for (int j = 0; j < slice; j++)
                {
                    for (int i = 0; i < NodeListSize; i++)
                    {
                        string nstr;
                        nstr = Math.Round((NodeList[i].Node_Coord[0] * Math.Cos(j * dis)), 4).ToString() + " " +
                            Math.Round((NodeList[i].Node_Coord[0] * Math.Sin(j * dis)), 4).ToString() + " " +
                            Math.Round(NodeList[i].Node_Coord[1], 4).ToString();
                        sw.WriteLine(nstr);
                    }
                }
                sw.Close();

                MessageBox.Show(listFiles.StandardOutput.ReadToEnd());
                listFiles.WaitForExit();
                
                listFiles.Close();
            }
            catch (Exception e)
            {

            }
        }

        /*
        private void GetDataOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            MessageBox.Show(outLine.Data);
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                
                string[] line2Split = Regex.Split((outLine.Data).Trim(), " ", RegexOptions.IgnoreCase);
                if (line2Split[0] == "2")
                {
                    _2DNodeList.Add(new ListNodeBase(cnt, cnt,
                        System.Convert.ToDouble(line2Split[1]), System.Convert.ToDouble(line2Split[2]), 0.0));
                    cnt++;
                }
            }
        }
        */
        public void BuildPointCloud()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\tower.asc", true);

            double pi = Math.PI;
            double range = pi;
            double slice = 1;
            double dis = range / slice;

            int dim = 2;
            file.WriteLine(dim.ToString());
            file.WriteLine(NodeListSize.ToString());
            for (int j = 0; j < slice; j++)
            {
                for (int i = 0; i < NodeListSize; i++)
                {
                    string nstr;
                    nstr = Math.Round((NodeList[i].Node_Coord[0] * Math.Cos(j * dis)),4).ToString() + " " +
                        /* Math.Round((NodeList[i].Node_Coord[0] * Math.Sin(j * dis)),4).ToString() + " " + */
                        Math.Round(NodeList[i].Node_Coord[1],4).ToString();
                    file.WriteLine(nstr);
                }
            }
            Cloud2DWriteMark = 1;
            file.Close();
        }

        public void Build3DPointCloud()
        {
            System.Windows.Forms.OpenFileDialog openfiledialog1 = new System.Windows.Forms.OpenFileDialog();
            openfiledialog1.RestoreDirectory = true;
            if (openfiledialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openfiledialog1.FileName, true);
                Boundary2D = new List<BoundaryLine>();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] line2Split = Regex.Split(line.Trim(), " ", RegexOptions.IgnoreCase);
                    if (line2Split[0] == "2" && line2Split.Count() == 3)
                    {
                        //MessageBox.Show(line2Split[1]+" "+line2Split[2]);
                        Boundary2D.Add(new BoundaryLine(int.Parse(line2Split[1]), int.Parse(line2Split[2])));
                        cnt++;
                    }
                }
                Cloud2DWriteMark = 1;
                MessageBox.Show(Boundary2D.Count().ToString());
            }
        }

        #region discard
        public void DrawModel()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\testttt.txt", true);

            for (int j = 0; j < 2; j++)
            {
                foreach (ListNodeBase node in NodeList)
                {
                    string nstr;
                    nstr = "v " + Math.Round(node.Node_Coord[0]*Math.Cos(j*3.1425926*0.5), 4).ToString() + ' ' +
                        Math.Round(node.Node_Coord[0] * Math.Sin(j * 3.1425926 * 0.5), 4).ToString() + ' ' + 
                        Math.Round(node.Node_Coord[1], 4).ToString();
                    file.WriteLine(nstr);
                }
            }

            double d = 0.01, sum = -10.0;
            double maxd = 0.0;

            int ang = 50;
            double ang_width = 3.1415925 * 0.5 * (1.0 / 50.0);
            int count = 0;
            foreach (ListNodeBase node in NodeList)
            {
                if (!(node.Node_Coord[1] <= sum + d &&
                    node.Node_Coord[1] >= sum))
                {
                    sum += d; count++;
                    for (int j = 1; j < ang; j++)
                    {
                        string nstr;
                        nstr = "v " + Math.Round(maxd * Math.Cos(j * ang_width), 4).ToString() + ' ' +
                            Math.Round(maxd * Math.Sin(j * ang_width), 4).ToString() + ' ' +
                            Math.Round(sum, 4).ToString();
                        file.WriteLine(nstr);
                    }
                    
                }
                if (node.Node_Coord[0] > maxd)
                    maxd = node.Node_Coord[0];
            }

            for (int j = 0; j < 2; j++)
            {
                foreach (ListElemBase elem in ElemList)
                {
                    string fline = "f ";
                    for (int i = 0; i < 8; i += 2)
                    {
                        fline += ((int)NodeElemTable[elem.Elem_Nodes[i]] + j * NodeListSize).ToString() + ' ';
                    }
                    file.WriteLine(fline);
                }
            }
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < ang-2; j++)
                {
                    int xij = i * (ang-2) + j + 2 * NodeListSize;
                    int xi_j = (i + 1) * (ang-2) + j + 2 * NodeListSize;
                    int xij_ = i * (ang-2) + (j + 1) + 2 * NodeListSize;
                    int xi_j_ = (i + 1) * (ang-2) + (j + 1) + 2 * NodeListSize;

                    string fline = "f ";
                    
                    fline += (xij).ToString() + ' ' + xi_j.ToString() + ' ' +
                        xij_.ToString() + ' ' + xi_j_.ToString();
                    
                    file.WriteLine(fline);
                }
            }

            file.Close();
            
        }
        #endregion

        public void VTKDrawModel(ref vtkPoints points, ref vtkCellArray strips)
        {
            MessageBox.Show(id.ToString());
            double pi = Math.PI;
            double range = pi;
            double slice = 100;
            double dis = range/slice;

            int cnt = 1;
            List<int> cntArray = new List<int>();
            for (int j = 0; j < slice; j++)
            {
                if (j == 0 || j == slice - 1)
                {
                    for (int i = 0; i < NodeListSize; i++)
                    {
                        points.InsertPoint(cnt,
                            NodeList[i].Node_Coord[0] * Math.Cos(j * dis),
                            NodeList[i].Node_Coord[0] * Math.Sin(j * dis),
                            NodeList[i].Node_Coord[1]);
                        cnt++;
                    }
                }
                
                else if(Cloud2DWriteMark == 1)
                {
                    for (int i = 0; i < Boundary2D.Count(); i++)
                    {
                        int para; 
                        strips.InsertNextCell(2);
                        for (int k = 0; k < 2; k++)
                        {
                            if (k == 0) para = Boundary2D[i].x;
                            else para = Boundary2D[i].y;
                            
                            //int io = (int)NodeElemTable[para];
                            points.InsertPoint(cnt,
                                NodeList[para].Node_Coord[0] * Math.Cos(j * dis),
                                NodeList[para].Node_Coord[0] * Math.Sin(j * dis),
                                NodeList[para].Node_Coord[1]);
                            strips.InsertCellPoint(cnt);
                            cntArray.Add(cnt);
                            cnt++;
                        }
                    }
                }
                
            }
            
            for (int j = 0; j < 2; j++)
            {
                foreach (ListElemBase elem in ElemList)
                {
                    strips.InsertNextCell(8);
                    for (int i = 0; i < 8; i++)
                    {
                        strips.InsertCellPoint((int)NodeElemTable[elem.Elem_Nodes[i]]+j*(cnt-NodeListSize+1));
                    }
                }
            }
            
        }

    }
}
