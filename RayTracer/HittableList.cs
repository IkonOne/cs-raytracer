using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class HittableList : IHittable
    {
        public int Count { get { return _hittables.Count;  } }

        private List<IHittable> _hittables;

        public HittableList()
        {
            _hittables = new List<IHittable>();
        }

        public void Add(IHittable hittable)
        {
            _hittables.Add(hittable);
        }

        public bool remove(IHittable hittable)
        {
            return _hittables.Remove(hittable);
        }

        public bool Hit(in Ray ray, float tMin, float tMax, out RayHitInfo hit)
        {
            hit = new RayHitInfo();
            var hitAnything = false;
            var closest = tMax;

            RayHitInfo testHit;
            foreach (var hittable in _hittables)
            {
                if (hittable.Hit(ray, tMin, tMax, out testHit))
                {
                    hitAnything = true;
                    if (testHit.t < closest)
                    {
                        hit = testHit;
                        closest = hit.t;
                    }
                }
            }

            return hitAnything;
        }
    }
}
