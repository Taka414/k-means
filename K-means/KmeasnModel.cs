using System;
using System.Collections.Generic;
using System.Windows;

namespace Tttaka.Kmeans
{
    public class KmeasnModel
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private Random r = new Random();

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        private int _Width = 250;
        public int Width
        {
            get { return this._Width; }
            set
            {
                this._Width = value;
            }
        }

        private int _Height = 250;
        public int Height
        {
            get { return this._Height; }
            set { this._Height = value; }
        }

        public IList<PointModel> ClusterList { get; private set; }

        public IList<PointModel> MeasurementPointList { get; private set; }

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public KmeasnModel()
        {
            this.ClusterList = new List<PointModel>();
            this.MeasurementPointList = new List<PointModel>();
        }

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        public void Clear()
        {
            this.ClusterList.Clear();
            this.MeasurementPointList.Clear();
        }

        public void InitCluster(int max)
        {
            // Calc fixed values in advance
            double xCenter = this.Width / 2;
            double yCenter = this.Height / 2;
            double xRadius = this.Width / 10;
            double yRadius = this.Height / 10;

            this.ClusterList.Clear();
            for (int i = 0; i < max; i++)
            {
                this.ClusterList.Add(new PointModel()
                {
                    ID = i,
                    Group = i,
                    X = this.r.Next((int)(xCenter - xRadius), (int)(xCenter + xRadius)),
                    Y = this.r.Next((int)(yCenter - yRadius), (int)(yCenter + yRadius)),
                });
            }
        }

        public void InitMeasurementPoints(int max)
        {
            this.MeasurementPointList.Clear();
            for (int i = 0; i < max; i++)
            {
                this.MeasurementPointList.Add(new PointModel()
                {
                    ID = i,
                    Group = this.r.Next(0, this.ClusterList.Count - 1),
                    X = this.r.Next(0, this.Width),
                    Y = this.r.Next(0, this.Height),
                });
            }
        }

        public void ChangeMeasurementPointBelongs()
        {
            foreach (var meas in this.MeasurementPointList)
            {
                PointModel nereCluster = null;
                double nearNolm = double.MaxValue;

                foreach (var cluster in this.ClusterList)
                {
                    double x = meas.X - cluster.X;
                    double y = meas.Y - cluster.Y;
                    double norm = Math.Sqrt(x * x + y * y);

                    if (nearNolm > norm)
                    {
                        nearNolm = norm;
                        nereCluster = cluster;
                    }
                }
                meas.Group = nereCluster.Group;
            }
        }

        public void MoveCluster()
        {
            foreach (var c in this.ClusterList)
            {
                Point p = this.GetCentroid(c.Group);
                if (double.IsNaN(p.X) || double.IsNaN(p.Y))
                {
                    return; // not move
                }
                c.X = p.X;
                c.Y = p.Y;
            }
        }

        public Point GetCentroid(int group)
        {
            double x = 0;
            double y = 0;
            int cnt = 0;

            var items = this.GetSamples(group);

            foreach (var item in items)
            {
                cnt++;
                x += item.X;
                y += item.Y;
            }
            return new Point(x / cnt, y / cnt);
        }

        public IEnumerable<PointModel> GetSamples(int group)
        {
            foreach (var item in this.MeasurementPointList)
            {
                if (item.Group == group)
                {
                    yield return item;
                }
            }
        }
    }
}
