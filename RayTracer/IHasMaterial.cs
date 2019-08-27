using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public interface IHasMaterial
    {
        IMaterial Material { get; set; }
    }
}
