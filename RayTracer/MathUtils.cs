using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracer
{
    public static class MathUtils
    {
        private static Random _rng = new Random();
        public static Random RNG { get => _rng; }

        public static Vector3 OnUnitSphere()
        {
            return Vector3.Normalize(InUnitSphere());
        }

        public static Vector3 InUnitSphere()
        {
            return new Vector3(
                2.0f * (float)_rng.NextDouble() - 1.0f,
                2.0f * (float)_rng.NextDouble() - 1.0f,
                2.0f * (float)_rng.NextDouble() - 1.0f
            );
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        // https://www.khronos.org/registry/OpenGL-Refpages/gl4/html/refract.xhtml
        public static bool Refract(Vector3 incident, Vector3 surfaceNorm, float refractiveIndex, out Vector3 refracted)
        {
            var unitIncidient = Vector3.Normalize(incident);
            var dt = Vector3.Dot(unitIncidient, surfaceNorm);
            var discriminant = 1.0f - refractiveIndex * refractiveIndex * (1.0f - dt * dt);

            if (discriminant > 0.0f)
            {
                //refracted = refractiveIndex * incident - (refractiveIndex * dt + (float)Math.Sqrt(discriminant)) * surfaceNorm;
                refracted = refractiveIndex * (unitIncidient - surfaceNorm * dt) - surfaceNorm * (float)Math.Sqrt(discriminant);
                return true;
            }

            refracted = Vector3.Zero;
            return false;
        }

        public static Matrix4x4 LookAt(Vector3 lookFrom, Vector3 lookAt, Vector3 up)
        {
            var zAxis = Vector3.Normalize(lookFrom - lookAt);
            var xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis));
            var yAxis = Vector3.Normalize(Vector3.Cross(zAxis, xAxis));

            return new Matrix4x4(
                xAxis.X, xAxis.Y, xAxis.Z, 0,
                yAxis.X, yAxis.Y, yAxis.Z, 0,
                zAxis.X, zAxis.Y, zAxis.Z, 0,
                lookFrom.X, lookFrom.Y, lookFrom.Z, 1
            );
        }

        public static Matrix4x4 Perspective(float fovRadians, float aspect, float near, float far)
        {
            var f = (float)Math.Tan(Math.PI * 0.5 - 0.5 * fovRadians);
            var rangeInv = 1.0f / (near - far);

            return new Matrix4x4(
                f / aspect, 0, 0, 0,
                0, f, 0, 0,
                0, 0, (near + far) * rangeInv, -1,
                0, 0, near * far * rangeInv * 2, 0
            );
        }
    }
}
