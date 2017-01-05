using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tttaka.Kmeans
{
    public static class ControlFactory
    {
        //
        // Constants
        // - - - - - - - - - - - - - - - - - -

        public static readonly int EllipseWidth = 6;
        public static readonly int EllipseHeight = 6;
        public static readonly int RectangleWidth = 7;
        public static readonly int RectangleHeight = 7;

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - -

        public static Ellipse CreateEllipse(Window context, int group)
        {
            return new Ellipse()
            {
                Width = EllipseWidth,
                Height = EllipseHeight,
                Stroke = GetBrush(context, "Color_m_", group),
                StrokeThickness = 1,
            };
        }

        public static Rectangle CreateRectangle(Window context, int group)
        {
            return new Rectangle()
            {
                Width = RectangleWidth,
                Height = RectangleHeight,
                Fill = GetBrush(context, "Color_c_", group),
            };
        }

        public static Line CreateLine(Window context, double x1, double y1, double x2, double y2, int group)
        {
            return new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = GetBrush(context, "Color_l_", group),
                StrokeThickness = 1,
            };
        }

        public static Brush GetBrush(Window context, string key, int group)
        {
            return context.FindResource(key + ((group % 6) + 1).ToString()) as Brush; // こ↑こ↓
        }
    }
}
