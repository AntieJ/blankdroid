using Android.Content;
using Android.Util;
using Android.Views;
using Android.Graphics;
using BlankDroid.Services;
using System.Collections.Generic;
using BlankDroid.Models;

namespace BlankDroid.Views
{
    public class WaveformView : View
    {
        WaveformService _waveformService;
        FileService _fileService;        
        static string _path;

        public WaveformView(Context context, IAttributeSet attrs) : base(context) {
            _waveformService = new WaveformService();
            _fileService = new FileService();
            _path = AnalysisContext.FullAudioPath;
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.Scale((float)1, (float)0.5);
            var displayLines = GetDisplayLines(canvas, AnalysisContext.samples);
            DrawGraph(canvas, displayLines);          
        }

        private void DrawInfo(Canvas canvas, List<short> samples, float x, float y)
        {
            canvas.DrawText(
                $"{samples.Count} samples. {samples.Count / ConfigService.AudioFrequency} seconds",
                x, y, PaintService.GetDefaultTextPaint());            
        }

        private void DrawGraph(Canvas canvas, SimpleLine[] lines)
        {           
            var baseLine = _waveformService.GetBaseLine(canvas.Width, ConfigService.YAxis);
            DrawLines(canvas, lines, PaintService.GetRed());
            DrawLine(canvas, baseLine, PaintService.GetBlue());
        }        

        private SimpleLine[] GetDisplayLines(Canvas canvas, List<short> samples)
        {
            SimpleLine[] displayLines;
            if (_fileService.ProcessedDisplayLinesFileExists(AnalysisContext.BaseDirectory, AnalysisContext.FileName))
            {
                displayLines = _fileService.GetProcessedDisplayLines(AnalysisContext.BaseDirectory, AnalysisContext.FileName);
            }
            else
            {
                displayLines = _waveformService.GetLinesFromSamples(canvas.Width, ConfigService.YAxis, ConfigService.ChartHeight, samples);
                _fileService.SaveProcessedDisplayLines(displayLines, AnalysisContext.FileName);
            }

            return displayLines;
        }

        private void DrawLines(Canvas canvas, SimpleLine[] linesToDraw, Paint paint)
        {
            if (linesToDraw == null)
                return;
            
            for (var i = 0; i < (linesToDraw.Length); i++)
            {
                DrawLine(canvas, linesToDraw[i], paint);
            }
        }

        private void DrawLine(Canvas canvas, SimpleLine lineToDraw, Paint paint)
        {
            if (lineToDraw == null)
                return;

            canvas.DrawLine(lineToDraw.Start.X, lineToDraw.Start.Y, lineToDraw.End.X, lineToDraw.End.Y, paint);
        }
    }
}