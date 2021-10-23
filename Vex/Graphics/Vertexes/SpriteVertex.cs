using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Vex.Graphics
{
    /// <summary>
    /// Spirte vertex for sprite vertex mesh
    /// </summary>
    public struct SpriteVertex
    {
        public SpriteVertex(in Vector2 position,in Vector2 uv)
        {
            Position = position;
            Uv = uv;
        }
        public SpriteVertex(float positionX,float positionY,float uvX,float uvY)
        {
            Position = new Vector2(positionX, positionY);
            Uv = new Vector2(uvX, uvY);
        }

        /// <summary>
        /// The position of the vertex
        /// </summary>
        public readonly Vector2 Position;

        /// <summary>
        /// The uv coordinate of the vertex
        /// </summary>
        public readonly Vector2 Uv;
    }
}
