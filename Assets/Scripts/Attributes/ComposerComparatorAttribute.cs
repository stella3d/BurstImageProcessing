using System;

namespace BurstImageProcessing
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class ComposerComparatorAttribute : Attribute
    {
        Comparator m_Comparator;

        public ComposerComparatorAttribute(Comparator comparator)
        {
            m_Comparator = comparator;
        }
    }
}
