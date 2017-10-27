using Android.Graphics;

namespace BlankDroid.Services
{
    public static class PaintService
    {
        public static Paint GetBlue()
        {
            var paint = GetDefaultPaint(Paint.Style.Stroke, 4);
            paint.SetARGB(255, 0, 0, 255);
            return paint;
        }

        public static Paint GetRed()
        {
            var paint = GetDefaultPaint(Paint.Style.Stroke, 4);
            paint.SetARGB(255, 255, 0, 0);
            return paint;
        }

        public static Paint GetDefaultTextPaint()
        {
            var paint = GetDefaultPaint(Paint.Style.Fill, 3);
            paint.SetARGB(255, 0, 255, 0);
            paint.TextSize = 30;
            paint.TextAlign = Paint.Align.Center;
            return paint;
        }

        public static Paint GetDefaultPaint(Paint.Style style, int strokeWidth)
        {
            var paint = new Paint();
            paint.SetStyle(style);
            paint.StrokeWidth = strokeWidth;
            return paint;
        }
    }
}