using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracer
{
    public struct RayHitInfo
    {
        public Vector3 Point;
        public float t;
        public IMaterial Material;

        /// <summary>
        /// Surface normal at the hit point.  Do not assume normalized.
        /// </summary>
        public Vector3 Normal;

    }
}
