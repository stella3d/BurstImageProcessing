using System;
using UnityEngine;

namespace BurstImageProcessing
{
    interface IEffectComposer32
    {
        void ReInitialize(Color32[] pixels);

        void UpdateImageData(Color32[] pixels);

        int GetProcessedDataPtr(out IntPtr ptr);
    }
}
