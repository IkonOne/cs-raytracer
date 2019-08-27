using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracer.Material
{
    public class Metal : IMaterial
    {
        public ColorFloat Albedo;
        public float Roughness;

        public Metal(float r, float g, float b, float roughness = 0.0f)
            : this(ColorFloat.FromARGB(r, g, b), roughness)
        { }

        public Metal(ColorFloat albedo, float roughness = 0.0f)
        {
            Albedo = albedo;
            Roughness = MathUtils.Clamp(roughness, 0, 1);
        }

        public bool Scatter(in Ray rayIn, in RayHitInfo hit, out ColorFloat attenuation, out Ray scattered)
        {
            var reflected = Vector3.Reflect(Vector3.Normalize(rayIn.Direction), hit.Normal);

            attenuation = Albedo;
            scattered = new Ray
            {
                Origin = hit.Point,
                Direction = reflected + (MathUtils.InUnitSphere() * Roughness)
            };

            return (Vector3.Dot(scattered.Direction, hit.Normal) > 0);
        }
    }
}
