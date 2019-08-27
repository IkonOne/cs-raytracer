using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public interface IHittable
    {
        bool Hit(in Ray ray, float tMin, float tMax, out RayHitInfo hit);
    }
}
