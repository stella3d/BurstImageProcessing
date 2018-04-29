using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurstImageProcessing.Generation
{
    public class RGBJobTemplate
    {
        IColorChannelThresholdJob m_RedJob;
        IColorChannelThresholdJob m_GreenJob;
        IColorChannelThresholdJob m_BlueJob;
    }

    public static class TextTemplates
    {
        public static string RGBJobText =
            "public class {0}" +
            "{" +
            "    {1} m_RedJob;" +
            "    {2} m_GreenJob;" +
            "    {3} m_BlueJob;" +
            "}";
    }
}
