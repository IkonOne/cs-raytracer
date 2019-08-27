using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using RayTracer.Material;

namespace RayTracer
{
    public class Sphere : IHittable, IHasMaterial
    {
        public Vector3 Center;
        public float Radius;

        public IMaterial Material { get; set; }

        public Sphere()
            : this(Vector3.Zero, 1.0f, Lambertian.Default())
        { }

        public Sphere(Vector3 center, float radius, IMaterial material)
        {
            Center = center;
            Radius = radius;
            Material = material;
        }

        public bool Hit(in Ray ray, float tMin, float tMax, out RayHitInfo hit)
        {
            Vector3 oc = ray.Origin - Center;
            float c = oc.LengthSquared() - Radius * Radius;

            // FIXME: If the ray originates inside the sphere we should return true and create valid hit info
            if (c < 0)
            {
                hit = new RayHitInfo();
                return false;
            }

            float a = ray.Direction.LengthSquared();
            float b = Vector3.Dot(oc, ray.Direction);
            float discriminant = b * b - a * c;

            if (discriminant > 0)
            {
                float temp = (-b - (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin)
                {
                    var p = ray.PointAt(temp);
                    hit = new RayHitInfo
                    {
                        t = temp,
                        Point = p,
                        Normal = (p - Center) / Radius,
                        Material = Material
                    };
                    return true;
                }

                temp = (-b + (float)Math.Sqrt(discriminant)) / a;
                if (temp < tMax && temp > tMin)
                {
                    var p = ray.PointAt(temp);
                    hit = new RayHitInfo
                    {
                        t = temp,
                        Point = p,
                        Normal = (p - Center) / Radius,
                        Material = Material
                    };
                    return true;
                }
            }

            hit = new RayHitInfo();
            return false;
        }
    }
}
