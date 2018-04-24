using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Bitwise
{
    // under-threshold bitwise operation performed against the pixel's own data (self)
    [ComputeJobOptimization]
    public struct UnderThresholdSelfComplementJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], ~data[i], data[i] < threshold);
        }
    }

    [ComputeJobOptimization]
    public struct UnderThresholdSelfLeftShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] << data[i], data[i] < threshold);
        }
    }

    [ComputeJobOptimization]
    public struct UnderThresholdSelfRightShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] >> data[i], data[i] < threshold);
        }
    }


    [ComputeJobOptimization]
    public struct UnderThresholdSelfExclusiveOrJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] ^ data[i], data[i] < threshold);
        }
    }


    // perform bitwise operation against the threshold value instead of against the pixel's valu
    [ComputeJobOptimization]
    public struct UnderThresholdComplementJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], ~threshold, data[i] < threshold);
        }
    }

    [ComputeJobOptimization]
    public struct UnderThresholdExclusiveOrJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] ^ threshold, data[i] < threshold);
        }
    }

    [ComputeJobOptimization]
    public struct UnderThresholdLeftShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] << threshold, data[i] < threshold);
        }
    }

    [ComputeJobOptimization]
    public struct UnderThresholdRightShiftJob : IJobParallelFor
    {
        public NativeSlice<byte> data;
        public byte threshold;

        public void Execute(int i)
        {
            data[i] = (byte)math.select(data[i], data[i] >> threshold, data[i] < threshold);
        }
    }
}
