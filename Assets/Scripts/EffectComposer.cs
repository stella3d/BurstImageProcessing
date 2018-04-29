using BurstImageProcessing.Threshold.Bitwise;
using BurstImageProcessing.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace BurstImageProcessing
{
    [ExecuteInEditMode]
    public class EffectComposer : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 255)]
        protected byte m_AdditionValue;

        [SerializeField]
        [Tooltip("This color defines the 'threshold' value against which pixel channel equality is tested")]
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

        Dictionary<ComposerInputsAttribute, Type> m_JobTypes = AttributeUtils.FindAllComposerInputs();

        JobHandle m_RedJobHandle;
        JobHandle m_GreenJobHandle;
        JobHandle m_BlueJobHandle;
        JobHandle m_DummyDependencyHandle;

        Type m_CurrentRedType;
        Type m_CurrentGreenType;
        Type m_CurrentBlueType;

        const int k_PixelCount = 320 * 240;

        NativeArray<Color32> m_Color32;
        NativeSlice<byte> m_RedChannel;
        NativeSlice<byte> m_GreenChannel;
        NativeSlice<byte> m_BlueChannel;

        void OnEnable()
        {
            m_Color32 = new NativeArray<Color32>(k_PixelCount, Allocator.Persistent);
            for (int i = 0; i < k_PixelCount; i++)
            {
                m_Color32[i] = new Color32(60, 100, 120, 255);
            }
            var wholeSlice = new NativeSlice<Color32>(m_Color32);
            m_RedChannel = wholeSlice.SliceWithStride<byte>(0);
            m_GreenChannel = wholeSlice.SliceWithStride<byte>(1);
            m_BlueChannel = wholeSlice.SliceWithStride<byte>(2);

            m_DummyDependencyHandle = new JobHandle();
            m_DummyDependencyHandle.Complete();
        }

        private void OnDisable()
        {
            m_Color32.Dispose();
        }

        void Update()
        {
            RedUpdate();
            GreenUpdate();
            BlueUpdate();
        }

        void LateUpdate()
        {
            m_RedJobHandle.Complete();
            m_GreenJobHandle.Complete();
            m_BlueJobHandle.Complete();
        }

        void RedUpdate()
        {
            if (!m_RedJobHandle.IsCompleted)
                m_RedJobHandle.Complete();

            // all the helper functions take a dependency handle, but the red channel always goes first and doesn't need one
            ScheduleChannel(m_RedOperator, m_RedComparator, m_RedOperand, m_RedChannel, m_ColorThreshold.r, ref m_RedJobHandle, ref m_DummyDependencyHandle);
        }

        void GreenUpdate()
        {
            if (!m_GreenJobHandle.IsCompleted)
                m_GreenJobHandle.Complete();

            ScheduleChannel(m_GreenOperator, m_GreenComparator, m_GreenOperand, m_GreenChannel, m_ColorThreshold.g, ref m_GreenJobHandle, ref m_RedJobHandle);
        }

        void BlueUpdate()
        {
            if (!m_BlueJobHandle.IsCompleted)
                m_BlueJobHandle.Complete();

            ScheduleChannel(m_BlueOperator, m_BlueComparator, m_BlueOperand, m_BlueChannel, m_ColorThreshold.b, ref m_BlueJobHandle, ref m_GreenJobHandle);
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
                    RGBSchedule.OverThresholdComplementSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBSchedule.OverThresholdExclusiveOrSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBSchedule.OverThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBSchedule.OverThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void EqualThresholdSelf(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBSchedule.AtThresholdComplementSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBSchedule.AtThresholdExclusiveOrSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBSchedule.AtThresholdLeftShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBSchedule.AtThresholdRightShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void UnderThresholdSelf(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBSchedule.UnderThresholdComplementSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBSchedule.UnderThresholdExclusiveOrSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBSchedule.UnderThresholdLeftShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBSchedule.UnderThresholdRightShiftSelf(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void OverThresholdOther(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBSchedule.OverThresholdComplementOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBSchedule.OverThresholdExclusiveOrOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBSchedule.OverThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBSchedule.OverThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void EqualThresholdOther(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBSchedule.AtThresholdComplementOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBSchedule.AtThresholdExclusiveOrOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBSchedule.AtThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBSchedule.AtThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }

        void UnderThresholdOther(Operator op, NativeSlice<byte> data, byte threshold, ref JobHandle handle, ref JobHandle dependency)
        {
            switch (op)
            {
                case Operator.BitwiseComplement:
                    RGBSchedule.UnderThresholdComplementOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseExclusiveOr:
                    RGBSchedule.UnderThresholdExclusiveOrOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseLeftShift:
                    RGBSchedule.UnderThresholdLeftShiftOther(data, threshold, ref handle, ref dependency);
                    break;
                case Operator.BitwiseRightShift:
                    RGBSchedule.UnderThresholdRightShiftOther(data, threshold, ref handle, ref dependency);
                    break;
            }
        }
    }
}