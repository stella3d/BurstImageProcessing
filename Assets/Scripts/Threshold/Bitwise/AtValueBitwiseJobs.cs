using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Bitwise
{
    // at-value bitwise operation performed against the pixel's own data (self)
    [ComputeJobOptimization]
    [ComposerInputs(Operator.BitwiseComplement, Comparator.Greater, Operand.Self)]
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
    [ComposerInputs(Operator.BitwiseLeftShift, Comparator.Greater, Operand.Self)]
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
    [ComposerInputs(Operator.BitwiseRightShift, Comparator.Greater, Operand.Self)]
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
    [ComposerInputs(Operator.BitwiseExclusiveOr, Comparator.Greater, Operand.Self)]
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
    [ComposerInputs(Operator.BitwiseComplement, Comparator.Greater, Operand.Other)]
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
    [ComposerInputs(Operator.BitwiseExclusiveOr, Comparator.Greater, Operand.Other)]
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
    [ComposerInputs(Operator.BitwiseLeftShift, Comparator.Greater, Operand.Self)]
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
    [ComposerInputs(Operator.BitwiseRightShift, Comparator.Greater, Operand.Self)]
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

