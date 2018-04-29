using BurstImageProcessing.Bitwise;
using BurstImageProcessing.Threshold.Bitwise;
using Unity.Collections;
using Unity.Jobs;

namespace BurstImageProcessing
{
    public static class RGBJob
    {
        public static void OverThresholdExclusiveOrOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdExclusiveOrJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void OverThresholdExclusiveOrSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdSelfExclusiveOrJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void OverThresholdComplementOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdComplementJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void OverThresholdComplementSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdSelfComplementJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void OverThresholdLeftShiftOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdLeftShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void OverThresholdLeftShiftSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdSelfLeftShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void OverThresholdRightShiftOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdRightShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void OverThresholdRightShiftSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new OverThresholdSelfRightShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }


        // UNDER THRESHOLD JOBS 
        public static void UnderThresholdExclusiveOrOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            var job = new UnderThresholdExclusiveOrJob()
            {
                data = channel,
                threshold = threshold
            };

            handle = job.Schedule(channel.Length, 512, dependency);
        }

        public static void UnderThresholdExclusiveOrSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            var job = new UnderThresholdSelfExclusiveOrJob()
            {
                data = channel,
                threshold = threshold
            };

            handle = job.Schedule(channel.Length, 512, dependency);
        }

        public static void UnderThresholdComplementOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new UnderThresholdComplementJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void UnderThresholdComplementSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new UnderThresholdSelfComplementJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void UnderThresholdLeftShiftOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new UnderThresholdLeftShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void UnderThresholdLeftShiftSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new UnderThresholdSelfLeftShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void UnderThresholdRightShiftOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new UnderThresholdRightShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void UnderThresholdRightShiftSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new UnderThresholdSelfRightShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        // EQUAL THRESHOLD JOBS 
        public static void AtThresholdExclusiveOrOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            var job = new AtThresholdExclusiveOrJob()
            {
                data = channel,
                threshold = threshold
            };

            handle = job.Schedule(channel.Length, 512, dependency);
        }

        public static void AtThresholdExclusiveOrSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            var job = new AtThresholdSelfExclusiveOrJob()
            {
                data = channel,
                threshold = threshold
            };

            handle = job.Schedule(channel.Length, 512, dependency);
        }

        public static void AtThresholdComplementOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new AtThresholdComplementJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void AtThresholdComplementSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new AtThresholdSelfComplementJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void AtThresholdLeftShiftOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new AtThresholdLeftShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void AtThresholdLeftShiftSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new AtThresholdSelfLeftShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void AtThresholdRightShiftOther(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new AtThresholdRightShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }

        public static void AtThresholdRightShiftSelf(NativeSlice<byte> channel, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            handle = new AtThresholdSelfRightShiftJob()
            {
                data = channel,
                threshold = threshold
            }
            .Schedule(channel.Length, 512, dependency);
        }
    }
}
