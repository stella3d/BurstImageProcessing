using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace BurstImageProcessing
{
    public class SharedPixelBuffer32 : MonoBehaviour
    {
        NativeArray<Color32> m_Pixels;
        NativeSlice<byte> m_RedChannel;
        NativeSlice<byte> m_GreenChannel;
        NativeSlice<byte> m_BlueChannel;

        // a list of everything that has to happen before we can safely read from the NativeArray
        // usually just calling Complete() on job handles
        List<Action> m_CompleteBeforeRead = new List<Action>();

        public void Initialize(Color32[] pixels)
        {
            if (m_Pixels.IsCreated)
                m_Pixels.Dispose();

            m_Pixels = new NativeArray<Color32>(pixels.Length, Allocator.Persistent);
            var wholeSlice = new NativeSlice<Color32>(m_Pixels);
            m_RedChannel = wholeSlice.SliceWithStride<byte>(0);
            m_GreenChannel = wholeSlice.SliceWithStride<byte>(1);
            m_BlueChannel = wholeSlice.SliceWithStride<byte>(2);
        }

        public void OnDisable()
        {
            if (m_Pixels.IsCreated)
                m_Pixels.Dispose();
        }

        public void AssignColorChannels(out NativeSlice<byte> red, out NativeSlice<byte> green, out NativeSlice<byte> blue)
        {
            red = m_RedChannel;
            green = m_GreenChannel;
            blue = m_BlueChannel;
        }

        public void UpdateImageData(Color32[] pixels)
        {
            if (pixels.Length != m_Pixels.Length)
                Debug.LogError("input pixel array length must be equal to the current native pixel array length");

            m_Pixels.CopyFrom(pixels);
        }

        public void RegisterOnGetPixelBufferAction(Action action)
        {
            m_CompleteBeforeRead.Add(action);
        }

        public void UnregisterOnGetPixelBufferAction(Action action)
        {
            m_CompleteBeforeRead.Remove(action);
        }

        unsafe public int GetPixelBufferPtr(out IntPtr ptr)
        {
            foreach (var handle in m_CompleteBeforeRead)
                handle();

            ptr = (IntPtr)m_Pixels.GetUnsafeReadOnlyPtr();
            return m_Pixels.Length * sizeof(Color32);
        }
    }
}
