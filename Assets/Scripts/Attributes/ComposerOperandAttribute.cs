using System;

namespace BurstImageProcessing
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class ComposerOperandAttribute : Attribute
    {
        Operand m_Operand;

        public ComposerOperandAttribute(Operand operand)
        {
            m_Operand = operand;
        }
    }
}
