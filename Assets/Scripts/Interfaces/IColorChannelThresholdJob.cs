using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace BurstImageProcessing
{
    [JobProducerType(typeof(IColorChannelThresholdJob))]
    public interface IColorChannelThresholdJob : IJobParallelFor
    {
        NativeSlice<byte> channelData { get; set; }
        byte thresholdValue { get; set; }

        //JobHandle Schedule();
    }
}
