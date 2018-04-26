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
            get { return m_Comparator; }
        }

        public Operator Operator
        {
            get { return m_Operator; }
        }

        public Operand Operand
        {
            get { return m_Operand; }
        }

        public override bool Equals(object obj)
        {
            var attribute = obj as ComposerInputsAttribute;
            return attribute != null &&
                   Comparator == attribute.Comparator &&
                   Operator == attribute.Operator &&
                   Operand == attribute.Operand;
        }

        public override int GetHashCode()
        {
            return ((int)m_Operator) * 1000  + ((int)m_Comparator) * 100 + ((int)m_Operand);
        }
    }
}
