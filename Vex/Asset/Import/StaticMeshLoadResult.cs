using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;

namespace Vex.Asset
{
    public sealed class StaticMeshLoadResult
    {
        public StaticMeshLoadResult(List<StaticMesh> meshes,List<Material> materials,List<Texture2D> textures)
        {
            m_Meshes = meshes;
            m_Materials = materials;
            m_Textures = textures;
        }

        public List<StaticMesh> Meshes
        {
            get
            {
                return m_Meshes;
            }
        }
        public List<Material> Materials
        {
            get
            {
                return m_Materials;
            }
        }
        public List<Texture2D> Textures
        {
            get
            {
                return m_Textures;
            }
        }
        private List<StaticMesh> m_Meshes;
        private List<Material> m_Materials;
        private List<Texture2D> m_Textures;
    }
}
