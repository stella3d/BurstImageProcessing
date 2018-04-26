using BurstImageProcessing.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Jobs;
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

        protected Operator m_PreviousRedOperator;
        protected Operand m_PreviousRedOperand;
        protected Comparator m_PreviousRedComparator;
        protected Operator m_PreviousGreenOperator;
        protected Operand m_PreviousGreenOperand;
        protected Comparator m_PreviousGreenComparator;
        protected Operator m_PreviousBlueOperator;
        protected Operand m_PreviousBlueOperand;
        protected Comparator m_PreviousBlueComparator;

        Dictionary<ComposerInputsAttribute, Type> m_JobTypes = AttributeUtils.FindAllComposerInputs();

        Type m_CurrentRedType;
        Type m_CurrentGreenType;
        Type m_CurrentBlueType;

        IJobParallelFor m_RedJob;
        IJobParallelFor m_GreenJob;
        IJobParallelFor m_BlueJob;

        void OnEnable()
        {
        }

        void Update()
        {
            if (m_PreviousRedOperator != m_RedOperator || m_PreviousRedComparator != m_RedComparator 
                || m_PreviousRedOperand != m_RedOperand)
            {
                var red = new ComposerInputsAttribute(m_RedOperator, m_RedComparator, m_RedOperand);
                if (m_JobTypes.TryGetValue(red, out m_CurrentRedType))
                {
                    Debug.Log("current red type: " + m_CurrentRedType);

                    m_RedJob = (IJobParallelFor)Activator.CreateInstance(m_CurrentRedType);
                }
            }

            if (m_PreviousGreenOperator != m_GreenOperator || m_PreviousGreenComparator != m_GreenComparator
                || m_PreviousGreenOperand != m_GreenOperand)
            {
                var red = new ComposerInputsAttribute(m_GreenOperator, m_GreenComparator, m_GreenOperand);
                if (m_JobTypes.TryGetValue(red, out m_CurrentGreenType))
                {
                    Debug.Log("current green type: " + m_CurrentGreenType);
                    m_GreenJob = (IJobParallelFor)Activator.CreateInstance(m_CurrentGreenType);
                }
            }

            if (m_PreviousBlueOperator != m_BlueOperator || m_PreviousBlueComparator != m_BlueComparator
                || m_PreviousBlueOperand != m_BlueOperand)
            {
                var red = new ComposerInputsAttribute(m_BlueOperator, m_BlueComparator, m_BlueOperand);
                if (m_JobTypes.TryGetValue(red, out m_CurrentBlueType))
                {
                    Debug.Log("current blue type: " + m_CurrentBlueType);
                    m_BlueJob = (IJobParallelFor)Activator.CreateInstance(m_CurrentBlueType);
                }
            }


            m_PreviousRedOperator = m_RedOperator;
            m_PreviousRedComparator = m_RedComparator;
            m_PreviousRedOperand = m_RedOperand;
            m_PreviousGreenOperator = m_GreenOperator;
            m_PreviousGreenComparator = m_GreenComparator;
            m_PreviousGreenOperand = m_GreenOperand;
            m_PreviousBlueOperator = m_BlueOperator;
            m_PreviousBlueComparator = m_BlueComparator;
            m_PreviousBlueOperand = m_BlueOperand;
        }
    }
}