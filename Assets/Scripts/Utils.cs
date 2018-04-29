using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace BurstImageProcessing.Utils
{
    static class LoadUtils
    {
        static void InitFromPixels32(Color32[] pixels, ref NativeArray<Color32> array,
            ref NativeSlice<byte> red, ref NativeSlice<byte> green, ref NativeSlice<byte> blue)
        {
            array.CopyFrom(pixels);
            var wholeSlice = new NativeSlice<Color32>(array);
            red = wholeSlice.SliceWithStride<byte>(0);
            green = wholeSlice.SliceWithStride<byte>(1);
            blue = wholeSlice.SliceWithStride<byte>(2);
        }
    }
}
