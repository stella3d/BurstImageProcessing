using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace BurstImageProcessing.Threshold
{
    [BurstCompile]
    //[ComposerInputs(Operator.Subtract, Comparator.Greater, Operand.Other)]
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

    [BurstCompile]
    //[ComposerInputs(Operator.Subtract, Comparator.Equal, Operand.Other)]
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

    [BurstCompile]
    //[ComposerInputs(Operator.Subtract, Comparator.Less, Operand.Other)]
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

