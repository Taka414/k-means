using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Tttaka.Kmeans
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private KmeasnModel model = new KmeasnModel();

        // For gui controls (step button clicked)
        private bool isFastStep = true;
        private int i;
        IDictionary<int, Action> buttanActionTable = new Dictionary<int, Action>();

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public MainWindow()
        {
            this.InitializeComponent();

            this.initTable();
        }

        //
        // EventHandlers
        // - - - - - - - - - - - - - - - - - - - -

        protected virtual void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Button_Reset(sender, e);
        }

        protected virtual void Button_Step(object sender, RoutedEventArgs e)
        {
            if (this.isFastStep)
            {
                this.DisplayLines();
                this.isFastStep = false;
                return;
            }

            this.buttanActionTable[i % this.buttanActionTable.Count]();
            this.i++;
        }

        protected virtual void Button_Reset(object sender, RoutedEventArgs e)
        {
            this.Reset();
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void Reset()
        {
            this.Canvas.Children.Clear();

            this.model.Width = (int)this.Canvas.ActualWidth;
            this.model.Height = (int)this.Canvas.ActualHeight;
            this.model.InitCluster((int)this.ClusterNum.Value);
            this.model.InitMeasurementPoints((int)this.SamplesNum.Value);

            this.DisplayModel();

            this.isFastStep = true;
            this.i = 0;
        }

        public void DisplayModel()
        {
            this.DisplayModel(this.model.MeasurementPointList, ControlFactory.CreateEllipse);
            this.DisplayModel(this.model.ClusterList, ControlFactory.CreateRectangle);
        }

        public void DisplayModel(IList<PointModel> list, Func<Window, int, Shape> creator)
        {
            foreach (var item in list)
            {
                var c = creator(this, item.Group);
                Canvas.SetLeft(c, item.X);
                Canvas.SetTop(c, item.Y);
                this.Canvas.Children.Add(c);
            }
        }

        public void DisplayLines()
        {
            foreach (var c in this.model.ClusterList)
            {
                foreach (var m in this.model.MeasurementPointList)
                {
                    if (c.Group == m.Group)
                    {
                        Line line = 
                            ControlFactory.CreateLine(this,
                                (c.X + c.X + ControlFactory.EllipseWidth) / 2, 
                                (c.Y + c.Y + ControlFactory.EllipseHeight) /2,
                                (m.X + m.X + ControlFactory.EllipseWidth) / 2,
                                (m.Y + m.Y + ControlFactory.EllipseHeight) / 2,
                                c.Group);
                        this.Canvas.Children.Add(line);
                    }
                }
            }
        }

        //
        // Private Methods
        // - - - - - - - - - - - - - - - - - - - -

        private void initTable()
        {
            this.buttanActionTable.Add(0, () =>
            {
                this.model.ChangeMeasurementPointBelongs();
                this.Canvas.Children.Clear();
                this.DisplayModel();
                this.DisplayLines();
            });

            this.buttanActionTable.Add(1, () =>
            {
                this.model.MoveCluster();
                this.Canvas.Children.Clear();
                this.DisplayModel();
                this.DisplayLines();
            });
        }
    }
}