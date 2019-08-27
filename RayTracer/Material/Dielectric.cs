using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracer.Material
{
    public class Dielectric : IMaterial
    {
        public float RefractiveIndex { get; set; }
        public ColorFloat RefractedColor { get; set; }

        public Dielectric(ColorFloat refractedColor, float refractiveIndex)
        {
            RefractedColor = refractedColor;
            RefractiveIndex = refractiveIndex;
        }

        public bool Scatter(in Ray rayIn, in RayHitInfo hit, out ColorFloat attenuation, out Ray scattered)
        {
            Vector3 refracted;
            Vector3 outwardNormal;
            float ri;
            float reflectProb;
            float cosine;

            var reflected = Vector3.Reflect(rayIn.Direction, hit.Normal);
            attenuation = RefractedColor;

            if(Vector3.Dot(rayIn.Direction, hit.Normal) > 0)
            {
                outwardNormal = -hit.Normal;
                ri = RefractiveIndex;
                cosine = RefractiveIndex * Vector3.Dot(rayIn.Direction, hit.Normal) / rayIn.Direction.Length();
            }
            else
            {
                outwardNormal = hit.Normal;
                ri = 1.0f / RefractiveIndex;
                cosine = -Vector3.Dot(rayIn.Direction, hit.Normal) / rayIn.Direction.Length();
            }

            if (MathUtils.Refract(rayIn.Direction, outwardNormal, ri, out refracted))
            {
                reflectProb = Schlick(cosine, RefractiveIndex);
            }
            else
            {
                //scattered = new Ray
                //{
                //    Origin = hit.Point,
                //    Direction = reflected
                //};
                reflectProb = 1.0f;
            }

            if ((float)MathUtils.RNG.NextDouble() < reflectProb)
            {
                scattered = new Ray
                {
                    Origin = hit.Point,
                    Direction = reflected
                };
            }
            else
            {
                scattered = new Ray
                {
                    Origin = hit.Point,
                    Direction = refracted
                };
            }

            return true;
        }


        // What??? https://en.wikipedia.org/wiki/Schlick%27s_approximation
        private static float Schlick(float cosine, float refractionIndex)
        {
            var r0 = (1 - refractionIndex) / (1 + refractionIndex);
            r0 = r0 * r0;
            return r0 + (1 - r0) * (float)Math.Pow((1 - cosine), 5);
        }
    }
}
