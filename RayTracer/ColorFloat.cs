using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace RayTracer
{
    public struct ColorFloat
    {
        public float A;
        public float R;
        public float G;
        public float B;

        public void Normalize()
        {
            var max = Math.Max(A, Math.Max(R, Math.Max(G, B)));
            A /= max;
            R /= max;
            G /= max;
            B /= max;
        }

        public Color ToColor()
        {
            return Color.FromArgb(
                (int)(A * 255.99f),
                (int)(R * 255.99f),
                (int)(G * 255.99f),
                (int)(B * 255.99f)
            );
        }

        public static ColorFloat operator +(ColorFloat lhs, ColorFloat rhs)
            => FromARGB(lhs.A + rhs.A, lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B);

        public static ColorFloat operator -(ColorFloat lhs, ColorFloat rhs)
            => FromARGB(lhs.A + rhs.A, lhs.R + rhs.R, lhs.G + rhs.G, lhs.B + rhs.B);

        public static ColorFloat operator *(ColorFloat lhs, float s)
            => FromARGB(lhs.A * s, lhs.R * s, lhs.G * s, lhs.B * s);

        public static ColorFloat operator /(ColorFloat lhs, float s)
            => FromARGB(lhs.A / s, lhs.R / s, lhs.G / s, lhs.B / s);

        public static ColorFloat operator *(ColorFloat lhs, ColorFloat rhs)
            => FromARGB(lhs.A * rhs.A, lhs.R * rhs.R, lhs.G * rhs.G, lhs.B * rhs.B);


        public static ColorFloat FromARGB(float a, float r, float g, float b)
        {
            return new ColorFloat
            {
                A = a,
                R = r,
                G = g,
                B = b
            };
        }

        public static ColorFloat FromARGB(float r, float g, float b)
        {
            return FromARGB(1.0f, r, g, b);
        }

        public static ColorFloat FromVector3(Vector3 vec)
        {
            return FromARGB(vec.X, vec.Y, vec.Z);
        }
    }
}
