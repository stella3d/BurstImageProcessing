using System;

namespace BurstImageProcessing
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class ComposerInputsAttribute : Attribute
    {
        Comparator m_Comparator;
        Operator m_Operator;
        Operand m_Operand;

        public ComposerInputsAttribute(Operator op, Comparator comparator, Operand operand)
        {
            m_Operator = op;
            m_Comparator = comparator;
            m_Operand = operand;
        }

        public Comparator Comparator
        {
            get
            {
                return m_Comparator;
            }

            set
            {
                m_Comparator = value;
            }
        }

        public Operator Operator
        {
            get
            {
                return m_Operator;
            }

            set
            {
                m_Operator = value;
            }
        }

        public Operand Operand
        {
            get
            {
                return m_Operand;
            }

            set
            {
                m_Operand = value;
            }
        }

        public override bool Equals(object obj)
        {
            var attribute = obj as ComposerInputsAttribute;
            return attribute != null &&
                   base.Equals(obj) &&
                   Comparator == attribute.Comparator &&
                   Operator == attribute.Operator &&
                   Operand == attribute.Operand;
        }
    }
}
