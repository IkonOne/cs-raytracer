using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracer.Material
{
    public class Lambertian : IMaterial
    {
        public ColorFloat Albedo;

        public Lambertian(float r, float g, float b)
            : this(ColorFloat.FromARGB(r, g, b))
        { }

        public Lambertian(ColorFloat albedo)
        {
            Albedo = albedo;
        }

        public bool Scatter(in Ray rayIn, in RayHitInfo hit, out ColorFloat attenuation, out Ray scattered)
        {
            var target = hit.Point + hit.Normal + MathUtils.InUnitSphere();
            attenuation = Albedo;

            scattered = new Ray
            {
                Origin = hit.Point,
                Direction = target - hit.Point
            };

            return true;
        }

        public static Lambertian Default()
        {
            return new Lambertian(ColorFloat.FromARGB(0.8f, 0.3f, 0.3f));
        }
    }
}
