using System.Collections.Generic;
using System.Linq;
using BlankDroid.Models;

namespace BlankDroid.Services
{
    public class WaveformService
    {
        private int stepSize = 100;

        public SimpleLine[] GetLinesFromSamples(int screenWidth, int yAxis, int chartHeight, List<short> samples)
        {
            var smallBufferArray = GetFilteredArray(samples, stepSize);
            var sampleLines = new SimpleLine[smallBufferArray.Length];

            for (var i = 0; i < (smallBufferArray.Length); i++)
            {
                float xAnchor = ((float)i / (float)smallBufferArray.Length) * screenWidth;
                float lineHeightPercent = (float)smallBufferArray[i] / (float)smallBufferArray.Max();
                float lineHeightScaled = (float)lineHeightPercent * (float)chartHeight;
                var lineHeight = (float)yAxis - (float)lineHeightScaled;

                sampleLines[i] = new SimpleLine
                {
                    Start = new System.Drawing.Point((int)xAnchor, yAxis),
                    End = new System.Drawing.Point((int)xAnchor, (int)lineHeight)
                };
            }

            return sampleLines;
        }

        public SimpleLine GetBaseLine(int screenWidth, int yAxis)
        {
            return new SimpleLine
            {
                Start = new System.Drawing.Point(0, yAxis),
                End = new System.Drawing.Point(screenWidth, yAxis)
            };
        }

        private short[] GetFilteredArray(List<short> samples, int stepSize)
        {
            /*Creates a smaller array taking every X sample
            Do this because we don't need that much resolution when displaying
            Consider lowering the sample frequency of recording
            This could be done on file read*/

            return samples.Where((x, i) => i % stepSize == 0).ToArray();
        }
    }
}