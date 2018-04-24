using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Bitwise
{
    // at-value bitwise operation performed against the pixel's own data (self)
    [ComputeJobOptimization]
    public struct AtValueSelfComplementJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], ~data[i], data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct AtValueSelfLeftShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] << data[i], data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct AtValueSelfRightShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] >> data[i], data[i] > threshold);
        }
    }


    [ComputeJobOptimization]
    public struct AtValueSelfExclusiveOrJob : IJobParallelFor
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
    public struct AtValueComplementJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], ~threshold, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct AtValueExclusiveOrJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] ^ threshold, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct AtValueLeftShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] << threshold, data[i] > threshold);
        }
    }

    [ComputeJobOptimization]
    public struct AtValueRightShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] >> threshold, data[i] > threshold);
        }
    }
}

