using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace BurstImageProcessing.Threshold
{
    [BurstCompile]
    public struct OverThresholdOffsetJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeSlice<byte> data;

        public byte threshold;

        //public int offsetX;
        //public int width;
        //public int height;
        public int pixelCount;

        public int xOffsetTimesWidth;

        public void Execute(int i)
        {
            var over = data[i] > threshold;
            var index = math.select(i, math.clamp(i - xOffsetTimesWidth, 0, pixelCount), over);
            data[i] = data[index];
        }
    }

    [BurstCompile]
    public struct UnderThresholdOffsetJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeSlice<byte> data;

        public byte threshold;

        public int offsetX;
        public int width;
        //public int height;
        public int pixelCount;

        public void Execute(int i)
        {
            var over = data[i] < threshold;
            var index = math.select(i, math.clamp(i - offsetX * width, 0, pixelCount), over);
            data[i] = data[index];
        }
    }
}

