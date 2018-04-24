using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Threshold.Bitwise
{
    // over-threshold bitwise operation performed against the pixel's own data (self)
    [ComputeJobOptimization]
    public struct OverThresholdSelfComplementJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], ~data[i], data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct OverThresholdSelfLeftShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] << data[i], data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct OverThresholdSelfRightShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] >> data[i], data[i] > threshold);
        }
    }


    [ComputeJobOptimization]
    public struct OverThresholdSelfExclusiveOrJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] ^ data[i], data[i] > threshold);
        }
    }

    // perform bitwise operation against the threshold value instead of against the pixel's value
    [ComputeJobOptimization]
    public struct OverThresholdComplementJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], ~threshold, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct OverThresholdExclusiveOrJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] ^ threshold, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct OverThresholdLeftShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] << threshold, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct OverThresholdRightShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] >> threshold, data[i] > threshold);
        }
    }
}

