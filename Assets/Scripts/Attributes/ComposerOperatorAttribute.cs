using System;

namespace BurstImageProcessing
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class ComposerOperatorAttribute : Attribute
    {
        Operator m_Operator;

        public ComposerOperatorAttribute(Operator op)
        {
            m_Operator = op;
        }
    }
}
