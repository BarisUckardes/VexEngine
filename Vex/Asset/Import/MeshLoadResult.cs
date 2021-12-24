using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;

namespace Vex.Asset
{
    public sealed class MeshLoadResult
    {
        public MeshLoadResult(StaticMesh mesh, Material material, List<Texture2D> textures)
        {
            m_Mesh = mesh;
            m_Material = material;
            m_Textures = textures;
        }

        private StaticMesh m_Mesh;
        private Material m_Material;
        private List<Texture2D> m_Textures;
    }
}
