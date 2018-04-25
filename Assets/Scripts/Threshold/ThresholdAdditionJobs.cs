using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Threshold
{
    [ComputeJobOptimization]
    public struct OverThresholdAddValueJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;
        public byte value;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] + value, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct EqualThresholdAddValueJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;
        public byte value;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] + value, data[i] == threshold);
        }
    }

    [ComputeJobOptimization]
    public struct UnderThresholdAddValueJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;
        public byte value;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] + value, data[i] < threshold);
        }
    }
}

