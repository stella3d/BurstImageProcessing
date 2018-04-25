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
    }
}
