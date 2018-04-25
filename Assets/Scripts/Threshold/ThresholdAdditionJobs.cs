using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Threshold
{
    [ComputeJobOptimization]
    [ComposerInputs(Operator.Add, Comparator.Greater, Operand.Other)]
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
    [ComposerInputs(Operator.Add, Comparator.Equal, Operand.Other)]
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
    [ComposerInputs(Operator.Add, Comparator.Less, Operand.Other)]
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

