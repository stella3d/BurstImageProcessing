using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using BurstImageProcessing.Threshold;

namespace BurstImageProcessing
{
    public class ThresholdOffsetEffectComposer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This color defines the 'threshold' value against which a pixel's color channel equality is tested")]
        protected Color32 m_ColorThreshold = new Color32();

        [SerializeField]
        protected bool m_EnableRed = true;

        [SerializeField]
        [Range(-200, 200)]
        protected int m_RedOffset;

        [SerializeField]
        protected bool m_EnableGreen = true;

        [SerializeField]
        [Range(-200, 200)]
        protected int m_GreenOffset;

        [SerializeField]
        protected bool m_EnableBlue = true;

        [SerializeField]
        [Range(-200, 200)]
        protected int m_BlueOffset;

        [SerializeField]
        protected SharedPixelBuffer32 m_SharedPixelBuffer;

        JobHandle m_RedJobHandle;
        JobHandle m_GreenJobHandle;
        JobHandle m_BlueJobHandle;
        JobHandle m_DummyDependencyHandle;

        NativeSlice<byte> m_RedChannel;
        NativeSlice<byte> m_GreenChannel;
        NativeSlice<byte> m_BlueChannel;

        Texture2D m_DynamicTexture;

        [SerializeField]
        int m_Width;
        [SerializeField]
        int m_Height;

        void OnEnable()
        {
            if (m_SharedPixelBuffer == null)
                m_SharedPixelBuffer = GetComponent<SharedPixelBuffer32>();

            m_SharedPixelBuffer.AssignColorChannels(out m_RedChannel, out m_GreenChannel, out m_BlueChannel);
            m_SharedPixelBuffer.RegisterOnGetPixelBufferAction(FinishJobs);

            m_DummyDependencyHandle = new JobHandle();
            m_DummyDependencyHandle.Complete();
        }

        private void OnDisable()
        {
            m_SharedPixelBuffer.UnregisterOnGetPixelBufferAction(FinishJobs);
        }

        void Update()
        {
            if (m_EnableRed)
                ScheduleChannel(ref m_RedChannel, m_ColorThreshold.r, m_RedOffset, ref m_RedJobHandle, ref m_DummyDependencyHandle);

            if (m_EnableGreen)
                ScheduleChannel(ref m_GreenChannel, m_ColorThreshold.g, m_GreenOffset, ref m_GreenJobHandle, ref m_RedJobHandle);

            if (m_EnableBlue)
            {
                if (m_EnableGreen)
                    ScheduleChannel(ref m_BlueChannel, m_ColorThreshold.b, m_BlueOffset, ref m_BlueJobHandle, ref m_GreenJobHandle);
                else if (m_EnableRed)
                    ScheduleChannel(ref m_BlueChannel, m_ColorThreshold.b, m_BlueOffset, ref m_BlueJobHandle, ref m_RedJobHandle);
                else
                    ScheduleChannel(ref m_BlueChannel, m_ColorThreshold.b, m_BlueOffset, ref m_BlueJobHandle, ref m_DummyDependencyHandle);
            }
        }

        void LateUpdate()
        {
            FinishJobs();
        }

        void FinishJobs()
        {
            m_RedJobHandle.Complete();
            m_GreenJobHandle.Complete();
            m_BlueJobHandle.Complete();
        }

        void ScheduleChannel(ref NativeSlice<byte> data, byte threshold, int offet, ref JobHandle handle, ref JobHandle dependency)
        {
            //Debug.Log("scheduling jobs for offset");
            var pixelCount = m_Width * m_Height;

            var job = new OverThresholdOffsetJob()
            {
                data = data,
                threshold = threshold,
                //height = m_Height,
                pixelCount = pixelCount,
                xOffsetTimesWidth = offet * m_Width
            };

            handle = job.Schedule(data.Length, 1024, dependency);
        }

    }
}