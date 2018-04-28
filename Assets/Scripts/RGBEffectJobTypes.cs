using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurstImageProcessing
{
    public class RGBEffectJobTypes
    {
        public Type red { get; protected set; }
        public Type green { get; protected set; }
        public Type blue { get; protected set; }

        public RGBEffectJobTypes(Type r, Type g, Type b)
        {
            red = r;
            green = g;
            blue = b;
        }
    }
}
