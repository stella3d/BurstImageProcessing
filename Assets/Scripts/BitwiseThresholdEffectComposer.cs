using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace BurstImageProcessing
{
    public class BitwiseThresholdEffectComposer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This color defines the 'threshold' value against which a pixel's color channel equality is tested")]
        protected Color32 m_ColorThreshold = new Color32();

        [SerializeField]
        protected bool m_EnableRed = true;
        [SerializeField]
        protected Operator m_RedOperator;
        [SerializeField]
        protected Operand m_RedOperand;
        [SerializeField]
        protected Comparator m_RedComparator;

        [SerializeField]
        protected bool m_EnableGreen = true;
        [SerializeField]
        protected Operator m_GreenOperator;
        [SerializeField]
        protected Operand m_GreenOperand;
        [SerializeField]
        protected Comparator m_GreenComparator;

        [SerializeField]
        protected bool m_EnableBlue = true;
        [SerializeField]
        protected Operator m_BlueOperator;
        [SerializeField]
        protected Operand m_BlueOperand;
        [SerializeField]
        protected Comparator m_BlueComparator;

        [SerializeField]
        protected SharedPixelBuffer32 m_SharedPixelBuffer;

        JobHandle m_RedJobHandle;
        JobHandle m_GreenJobHandle;
        JobHandle m_BlueJobHandle;
        JobHandle m_DummyDependencyHandle;

        NativeArray<Color32> m_Pixels;
        NativeSlice<byte> m_RedChannel;
        NativeSlice<byte> m_GreenChannel;
        NativeSlice<byte> m_BlueChannel;

        Texture2D m_DynamicTexture;

        const int k_DefaultPixelCount = 1024 * 576;

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
            if(m_EnableRed)
                // all the helper functions take a dependency handle, but the red channel always goes first and doesn't need one
                ScheduleChannel(m_RedOperator, m_RedComparator, m_RedOperand, m_RedChannel, m_ColorThreshold.r, ref m_RedJobHandle, ref m_DummyDependencyHandle);

            if (m_EnableGreen)
                ScheduleChannel(m_GreenOperator, m_GreenComparator, m_GreenOperand, m_GreenChannel, m_ColorThreshold.g, ref m_GreenJobHandle, ref m_RedJobHandle);

            if (m_EnableBlue)
            {
                if(m_EnableGreen)
                    ScheduleChannel(m_BlueOperator, m_BlueComparator, m_BlueOperand, m_BlueChannel, m_ColorThreshold.b, ref m_BlueJobHandle, ref m_GreenJobHandle);
                else if (m_EnableRed)
                    ScheduleChannel(m_BlueOperator, m_BlueComparator, m_BlueOperand, m_BlueChannel, m_ColorThreshold.b, ref m_BlueJobHandle, ref m_RedJobHandle);
                else
                    ScheduleChannel(m_BlueOperator, m_BlueComparator, m_BlueOperand, m_BlueChannel, m_ColorThreshold.b, ref m_BlueJobHandle, ref m_DummyDependencyHandle);
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

        // for use when the size changes
        public void ReInitialize(Color32[] pixels)
        {
            if (!m_BlueJobHandle.IsCompleted)
                m_BlueJobHandle.Complete();

            if (m_SharedPixelBuffer == null)
                m_SharedPixelBuffer = GetComponent<SharedPixelBuffer32>();

            m_SharedPixelBuffer.Initialize(m_Pixels);
            m_SharedPixelBuffer.RegisterOnGetPixelBufferAction(FinishJobs);
        }

        void ScheduleChannel(Operator op, Comparator comparator, Operand operand,
            NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (comparator)
            {
                case Comparator.Greater:
                    switch (operand)
                    {
                        case Operand.Self:
                            OverThresholdSelf(op, data, threshold, ref handle, ref dependency);
                            break;
                        case Operand.Other:
                            OverThresholdOther(op, data, threshold, ref handle, ref dependency);
                            break;
                    }
                    break;
                case Comparator.Equal:
                    switch (operand)
                    {
                        case Operand.Self:
                            EqualThresholdSelf(op, data, threshold, ref handle, ref dependency);
                            break;
                        case Operand.Other:
                            EqualThresholdOther(op, data, threshold, ref handle, ref dependency);
                            break;
                    }
                    break;
                case Comparator.Less:
                    switch (operand)
                    {
                        case Operand.Self:
                            UnderThresholdSelf(op, data, threshold, ref handle, ref dependency);
                            break;
                        case Operand.Other:
                            UnderThresholdOther(op, data, threshold, ref handle, ref dependency);
                            break;
                    }
                    break;
            }
        }

        void OverThresholdSelf(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch(op)
            {
                case Operator.BitwiseComplement:
                    RGBJob.OverThresholdComplementSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBJob.OverThresholdExclusiveOrSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBJob.OverThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBJob.OverThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void EqualThresholdSelf(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBJob.AtThresholdComplementSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBJob.AtThresholdExclusiveOrSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBJob.AtThresholdLeftShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBJob.AtThresholdRightShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void UnderThresholdSelf(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBJob.UnderThresholdComplementSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBJob.UnderThresholdExclusiveOrSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBJob.UnderThresholdLeftShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBJob.UnderThresholdRightShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void OverThresholdOther(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBJob.OverThresholdComplementOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBJob.OverThresholdExclusiveOrOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBJob.OverThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBJob.OverThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void EqualThresholdOther(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBJob.AtThresholdComplementOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBJob.AtThresholdExclusiveOrOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBJob.AtThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBJob.AtThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void UnderThresholdOther(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBJob.UnderThresholdComplementOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBJob.UnderThresholdExclusiveOrOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBJob.UnderThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBJob.UnderThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }
    }
}