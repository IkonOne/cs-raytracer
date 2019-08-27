using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracer
{
    public interface IMaterial
    {
        bool Scatter(in Ray rayIn, in RayHitInfo hit, out ColorFloat attenuation, out Ray scattered);
    }
}
