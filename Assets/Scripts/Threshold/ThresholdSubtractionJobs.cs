using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Threshold
{
    [ComputeJobOptimization]
    public struct OverThresholdSubtractValueJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;
        public byte value;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] - value, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct EqualThresholdSubtractValueJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;
        public byte value;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] - value, data[i] == threshold);
        }
    }

    [ComputeJobOptimization]
    public struct UnderThresholdSubtractValueJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;
        public byte value;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] - value, data[i] < threshold);
        }
    }
}

