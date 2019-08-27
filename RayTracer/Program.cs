using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

using RayTracer.Material;

namespace RayTracer
{
    class Program
    {
        static readonly int WIDTH = 400;
        static readonly int HEIGHT = 200;

        /// <summary>
        /// Number of samples to take per pixel.  The more samples, the better the image but slower rendering time.
        /// </summary>
        static readonly int NUM_SAMPLES = 100;

        /// <summary>
        /// Maximum number of times to bounce a ray.
        /// </summary>
        static readonly int MAX_DEPTH = 50;

        private static readonly Random rng = new Random();

        private static ColorFloat CastRay(in Ray ray, in HittableList world, int depth = 0)
        {
            ColorFloat cf;
            RayHitInfo hit;

            if (world.Hit(ray, 0.001f, float.MaxValue, out hit))
            {
                ColorFloat attenuation;
                Ray scatterRay;
                if (depth < MAX_DEPTH && hit.Material.Scatter(ray, hit, out attenuation, out scatterRay))
                {
                    return attenuation * CastRay(scatterRay, world, depth + 1);
                }
                else
                {
                    return ColorFloat.FromARGB(0, 0, 0);
                }
            }

            var unit = Vector3.Normalize(ray.Direction);
            var t = 0.5f * (unit.Y + 1.0f);
            var invT = 1.0f - t;
            cf = ColorFloat.FromARGB(invT + t * 0.5f, invT + t * 0.5f, invT + t);
            return cf;
        }

        private static HittableList GenRanomWorld()
        {
            int n = 500;
            HittableList list = new HittableList();
            list.Add(new Sphere(Vector3.UnitY * -1000, 1000, new Lambertian(0.5f, 0.5f, 0.5f)));
            for (int a = -11; a < 11; a += 2)
            {
                for (int b = -11; b < 11; b += 2)
                {
                    var chooseMat = MathUtils.RNG.NextDouble();
                    var center = new Vector3(a + 0.9f * (float)MathUtils.RNG.NextDouble(), 0.2f, b + 0.9f * (float)MathUtils.RNG.NextDouble());
                    if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9f)
                    {
                        if (chooseMat < 0.8)
                        {
                            list.Add(new Sphere
                            {
                                Center = center,
                                Radius = 0.2f,
                                Material = new Lambertian(
                                    (float)(MathUtils.RNG.NextDouble() * MathUtils.RNG.NextDouble()),
                                    (float)(MathUtils.RNG.NextDouble() * MathUtils.RNG.NextDouble()),
                                    (float)(MathUtils.RNG.NextDouble() * MathUtils.RNG.NextDouble())
                                )
                            });
                        }
                        else if (chooseMat < 0.95)
                        {
                            list.Add(new Sphere
                            {
                                Center = center,
                                Radius = 0.2f,
                                Material = new Metal(
                                    (float)(0.5 * (1 + MathUtils.RNG.NextDouble())),
                                    (float)(0.5 * (1 + MathUtils.RNG.NextDouble())),
                                    (float)(0.5 * (1 + MathUtils.RNG.NextDouble())),
                                    (float)(0.5 * MathUtils.RNG.NextDouble())
                                )
                            });
                        }
                        else
                        {
                            list.Add(new Sphere
                            {
                                Center = center,
                                Radius = 0.2f,
                                Material = new Dielectric(ColorFloat.FromARGB(1, 1, 1), 1.5f)
                            });
                        }
                    }
                }
            }

            return list;
        }

        static void Main(string[] args)
        {
            using (var bmp = new Bitmap(WIDTH, HEIGHT))
            {
                var ray = new Ray(Vector3.Zero, -Vector3.UnitZ);
                var cam = new Camera((float)(Math.PI * 0.5), WIDTH / HEIGHT, new Vector3(0, 0.1f, 1), -Vector3.UnitZ, Vector3.UnitY);

                //var world = GenRanomWorld();
                var world = new HittableList();
                world.Add(new Sphere { Center = -Vector3.UnitZ, Radius = 0.5f, Material = new Lambertian(0.1f, 0.2f, 0.5f) });
                world.Add(new Sphere { Center = new Vector3(0.0f, -100.5f, -1.0f), Radius = 100.0f, Material = new Lambertian(0.8f, 0.8f, 0.0f) });
                world.Add(new Sphere { Center = new Vector3(1, 0, -1), Radius = 0.5f, Material = new Metal(0.8f, 0.6f, 0.2f, 0.1f) });
                world.Add(new Sphere { Center = new Vector3(-1, 0, -1), Radius = 0.5f, Material = new Dielectric(ColorFloat.FromARGB(1.0f, 1.0f, 1.0f), 1.5f) });

                for (int j = HEIGHT - 1; j >= 0; --j)
                {
                    for (int i = 0; i < WIDTH; ++i)
                    {
                        var colorAccumulator = new ColorFloat();

                        for (int s = 0; s < NUM_SAMPLES; s++)
                        {
                            float u = (float)(i + rng.NextDouble()) / WIDTH;
                            float v = (float)(j + rng.NextDouble()) / HEIGHT;
                            ray = cam.getRay(u, v, out ray);
                            var color = CastRay(ray, world);
                            colorAccumulator += color;
                        }

                        colorAccumulator /= NUM_SAMPLES;
                        //colorAccumulator.A = 1.0f;

                        // FIXME: do gamma correction because most image viewers assume that an image has already been gamma corrected???
                        colorAccumulator.R = (float)Math.Sqrt(colorAccumulator.R);
                        colorAccumulator.G = (float)Math.Sqrt(colorAccumulator.G);
                        colorAccumulator.B = (float)Math.Sqrt(colorAccumulator.B);

                        bmp.SetPixel(
                            i,
                            HEIGHT - j - 1,
                            colorAccumulator.ToColor()
                        );
                    }
                }

                bmp.Save("out.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
