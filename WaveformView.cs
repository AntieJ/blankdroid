﻿using Android.Content;
using Android.Util;
using Android.Views;
using Android.Graphics;
using BlankDroid.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using BlankDroid.Models;

namespace BlankDroid
{
    public class WaveformView : View
    {
        private static List<Int16> samples = new List<short>();

        AudioSampleService _audioSampleService;
        WaveformService _waveformService;
        int yAxis = 500;
        int chartHeight = 500;

        public WaveformView(Context context, IAttributeSet attrs) : base(context) {
            _audioSampleService = new AudioSampleService();
            _waveformService = new WaveformService();
        }

        protected override void OnDraw(Canvas canvas)
        {
            Task.Run(() =>
            {
                samples = _audioSampleService.GetSampleValues().Result;
            });
            //samples = _audioSampleService.GetSampleValuesBytes();
            DrawInfo(canvas, samples, x:500, y:900);
            DrawGraph(canvas, samples);
        }

        private void DrawInfo(Canvas canvas, List<short> samples, float x, float y)
        {
            canvas.DrawText(
                $"{samples.Count} samples. {samples.Count / ConfigService.AudioFrequency} seconds",
                x, y, PaintService.GetDefaultTextPaint());
        }

        private void DrawGraph(Canvas canvas, List<short> samples)
        {            
            var displayLines = _waveformService.GetLinesFromSamples(canvas.Width, yAxis, chartHeight, samples);
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