using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BurstImageProcessing
{
    public class EffectComposer : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 255)]
        protected byte m_AdditionValue;


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
        [Tooltip("This color defines the 'threshold' value against which pixel channel equality is tested")]
        protected Color32 m_ColorThreshold = new Color32();

        void OnEnable()
        {
        }

        void Update()
        {

        }

        void OperandHandler()
        {
            switch (m_RedOperator)
            {
                case Operator.Add:
                    break;
                case Operator.BitwiseComplement:
                    break;
                case Operator.BitwiseExclusiveOr:
                    break;
                case Operator.BitwiseLeftShift:
                    break;
                case Operator.BitwiseRightShift:
                    break;
            }
        }
    }
}