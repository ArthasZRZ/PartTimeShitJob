﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Kitware.VTK;

namespace WpfRibbonApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void renderWindowControl1_Load(object sender, EventArgs e)
        {
            // Create a simple sphere. A pipeline is created.
            vtkPoints points = vtkPoints.New();
            vtkCellArray strips = vtkCellArray.New();
            MainWindow.WorkSpaceInstance.TowerModelInstance.VTKDrawModel(ref points, ref strips);
            vtkPolyData profile = vtkPolyData.New();
            profile.SetPoints(points);
            profile.SetStrips(strips);

            vtkExtractEdges extractedge = vtkExtractEdges.New();
            extractedge.SetInput(profile);

            /*
            vtkShrinkPolyData shrink = vtkShrinkPolyData.New();
            shrink.SetInputConnection(sphere.GetOutputPort());
            shrink.SetShrinkFactor(0.9);
            */
            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(extractedge.GetOutputPort());

            // The actor links the data pipeline to the rendering subsystem
            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            actor.GetProperty().SetColor(1, 0, 0);

            // Create components of the rendering subsystem
            //
            vtkRenderer ren1 = renderWindowControl1.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkRenderWindow renWin = renderWindowControl1.RenderWindow;

            // Add the actors to the renderer, set the window size
            //
            ren1.AddViewProp(actor);
            renWin.SetSize(250, 250);
            renWin.Render();
            vtkCamera camera = ren1.GetActiveCamera();
            camera.Zoom(1.5);
        }
    }
}
