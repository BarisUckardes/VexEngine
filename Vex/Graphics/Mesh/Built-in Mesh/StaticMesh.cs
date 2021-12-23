using Assimp;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Extensions;
namespace Vex.Graphics
{
    public sealed class StaticMesh : Mesh
    {
        public static StaticMesh LoadViaPath(string path)
        {
            /*
             * Validate path
             */
            if(!File.Exists(path))
            {
                return null;
            }

            /*
             * Get extension
             */
            string extension = Path.GetExtension(path);

            /*
             * Load via x extension
             */
            switch (extension)
            {
                case ".obj":
                {
                        return LoadAsObj(path);
                        break;
                }
            }

            return null;

        }
        private static StaticMesh LoadAsObj(string path)
        {
            /*
             * Create new assimp context
             */
            AssimpContext context = new AssimpContext();

            /*
             * Load assimp scene via a file path
             */
            Scene assimpMeshObject = context.ImportFile(path, PostProcessPreset.TargetRealTimeMaximumQuality);

            List<Assimp.Mesh> meshes = assimpMeshObject.Meshes;
            Assimp.Mesh aMesh = meshes[0];
            List<Vector3> positions = new List<Vector3>(aMesh.VertexCount);
            List<Vector3> normals = new List<Vector3>(aMesh.VertexCount);
            List<Vector3> tangents = new List<Vector3>(aMesh.VertexCount);
            List<Vector3> bitangents = new List<Vector3>(aMesh.VertexCount);
            List<Vector2> textureCoordinates = new List<Vector2>(aMesh.VertexCount);
            for (int i = 0;i < aMesh.VertexCount;i++)
            {
                positions.Add(aMesh.Vertices[i].GetAsOpenTK());
                normals.Add(aMesh.Normals[i].GetAsOpenTK());
                tangents.Add(aMesh.Tangents[i].GetAsOpenTK());
                bitangents.Add(aMesh.BiTangents[i].GetAsOpenTK());
                textureCoordinates.Add(new Vector2(aMesh.TextureCoordinateChannels[0][i].X, -aMesh.TextureCoordinateChannels[0][i].Y));
            }
            StaticMesh mesh = new StaticMesh();
            mesh.Positions = positions;
            mesh.Normals = normals;
            mesh.Tangents = tangents;
            mesh.BiTangents = bitangents;
            mesh.TextureCoordinates = textureCoordinates;
            mesh.Triangles = new List<int>(aMesh.GetIndices());
            mesh.ApplyChanges();
            return mesh;
        }
        public StaticMesh()
        {
            m_CpuPositions = new List<Vector3>();
            m_CpuNormals = new List<Vector3>();
            m_CpuTangents = new List<Vector3>();
            m_CpuBitangents = new List<Vector3>();
            m_CpuTextureCoordinates = new List<Vector2>();
        }
    

        public List<Vector3> Positions
        {
            get
            {
                return m_CpuPositions;
            }
            set
            {
                m_CpuPositions = value;
            }
        }
        public List<Vector3> Normals
        {
            get
            {
                return m_CpuNormals;
            }
            set
            {
                m_CpuNormals = value;
            }
        }
        public List<Vector3> Tangents
        {
            get
            {
                return m_CpuTangents;
            }
            set
            {
                m_CpuTangents = value;
            }
        }
        public List<Vector3> BiTangents
        {
            get
            {
                return BiTangents;
            }
            set
            {
                m_CpuBitangents = value;
            }
        }
        public List<Vector2> TextureCoordinates
        {
            get
            {
                return m_CpuTextureCoordinates;
            }
            set
            {
                m_CpuTextureCoordinates = value;
            }
        }
        public List<int> Triangles
        {
            get
            {
                return m_CpuTriangles;
            }
            set
            {
                m_CpuTriangles = value;
            }
        }

        public void ApplyChanges()
        {
            InValidate();
        }
        public override void Destroy()
        {
            VertexBuffer.Destroy();
            IndexBuffer.Destroy();
            VertexBuffer = null;
            IndexBuffer = null;

        }

        private void InValidate()
        {
            /*
             * Create new vertex layout
             */
            List<VertexLayoutElement> elements = new List<VertexLayoutElement>();
            elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_Position"));
            elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_Normal"));
            elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_Tangent"));
            elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_BiTangent"));
            elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float2, "v_Uv"));

            /*
             * Create new layout
             */
            VertexLayout layout =  new VertexLayout(elements.ToArray());
            Layout = layout;

            /*
             * Create VBO IBO
             */
            List<StaticMeshVertex> vertexes = new List<StaticMeshVertex>(m_CpuPositions.Count);

            for(int vertexIndex = 0;vertexIndex < m_CpuPositions.Count;vertexIndex++)
            {
                vertexes.Add(new StaticMeshVertex(
                    m_CpuPositions[vertexIndex],
                    m_CpuNormals[vertexIndex],
                    m_CpuTangents[vertexIndex],
                    m_CpuBitangents[vertexIndex],
                    m_CpuTextureCoordinates[vertexIndex]));
            }

            /*
             * Set vertex data
             */
            SetVertexBufferData(vertexes.ToArray());

            /*
             * Set index data
             */
            SetIndexBufferData(m_CpuTriangles.ToArray());
        }
        private List<Vector3> m_CpuPositions;
        private List<Vector3> m_CpuNormals;
        private List<Vector3> m_CpuTangents;
        private List<Vector3> m_CpuBitangents;
        private List<Vector2> m_CpuTextureCoordinates;
        private List<int> m_CpuTriangles;
    }
}
