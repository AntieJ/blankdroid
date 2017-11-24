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
        AudioSampleService _audioSampleService;
        WaveformService _waveformService;
        FileService _fileService;
        int yAxis = 500;
        int chartHeight = 500;
        static string _path;

        public WaveformView(Context context, IAttributeSet attrs) : base(context) {
            _audioSampleService = new AudioSampleService();
            _waveformService = new WaveformService();
            _fileService = new FileService();
            _path = AnalysisContext.FullAudioPath;
        }

        protected override void OnDraw(Canvas canvas)
        {
            canvas.Scale((float)1, (float)0.5);
            DrawGraph(canvas, AnalysisContext.samples);            
        }

        private void DrawInfo(Canvas canvas, List<short> samples, float x, float y)
        {
            canvas.DrawText(
                $"{samples.Count} samples. {samples.Count / ConfigService.AudioFrequency} seconds",
                x, y, PaintService.GetDefaultTextPaint());            
        }

        private void DrawGraph(Canvas canvas, List<short> samples)
        {
            SimpleLine[] displayLines;
            if (_fileService.ProcessedDisplayLinesFileExists(AnalysisContext.BaseDirectory, AnalysisContext.FileName))
            {
                displayLines = _fileService.GetProcessedDisplayLines(AnalysisContext.BaseDirectory, AnalysisContext.FileName);
            }
            else
            {
                displayLines = _waveformService.GetLinesFromSamples(canvas.Width, yAxis, chartHeight, samples);
                _fileService.SaveProcessedDisplayLines(displayLines, AnalysisContext.FileName);
            }           

            var baseLine = _waveformService.GetBaseLine(canvas.Width, yAxis);

            DrawLines(canvas, displayLines, PaintService.GetRed());
            DrawLine(canvas, baseLine, PaintService.GetBlue());
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