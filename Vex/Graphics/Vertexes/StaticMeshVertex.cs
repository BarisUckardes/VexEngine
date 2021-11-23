﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Vex.Graphics
{
    public readonly struct StaticMeshVertex
    {
        public StaticMeshVertex(Vector3 position, Vector3 normal, Vector2 uv)
        {
            Position = position;
            Normal = normal;
            Uv = uv;
        }
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Vector2 Uv;

       
    }
}
