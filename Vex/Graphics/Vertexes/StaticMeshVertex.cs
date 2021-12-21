using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Vex.Graphics
{
    public struct  StaticMeshVertex
    {
        public StaticMeshVertex(in Vector3 position,in Vector3 normal,in Vector3 tangent,in Vector3 bitangent,in Vector2 uv)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            BiTangent = tangent;
            Uv = uv;
        }
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector3 BiTangent;
        public Vector2 Uv;

       
    }
}
