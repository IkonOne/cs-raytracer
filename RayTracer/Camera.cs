using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace RayTracer
{
    public class Camera
    {
        public Vector3 Origin;
        public float Aspect;

        public Matrix4x4 cameraMatrix;
        public Matrix4x4 projectionMatrix;
        public Matrix4x4 viewMatrix;
        public Matrix4x4 viewProjectionMatrix;

        // TODO: Camera does not tilt when raised on the y axis  (fix matrix math)
        public Camera(float vFov, float aspect, Vector3 lookFrom, Vector3 lookAt, Vector3 up)
        {
            Origin = lookFrom;
            Aspect = aspect;

            //cameraMatrix = Matrix4x4.CreateLookAt(lookFrom, lookAt, up);
            cameraMatrix = MathUtils.LookAt(lookFrom, lookAt, up);
            Matrix4x4.Invert(cameraMatrix, out viewMatrix);
            projectionMatrix = MathUtils.Perspective(vFov, 1, 1, 2000);
            //projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(vFov, aspect, 1, 2000);
            viewProjectionMatrix = Matrix4x4.Multiply(projectionMatrix, viewMatrix);
        }

        public Ray getRay(float u, float v, out Ray ray)
        {
            var direction = new Vector3(-Aspect + Aspect * u * 2, -1 + v * 2, 1);
            direction = Vector3.Transform(direction, viewProjectionMatrix);
            ray = new Ray
            {
                Origin = Origin,
                Direction = direction
            };

            return ray;
        }
    }
}
