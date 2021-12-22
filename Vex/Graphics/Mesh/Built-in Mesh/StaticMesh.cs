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
            AssimpContext context = new AssimpContext();
            Scene assimpMeshObject = context.ImportFile(path, PostProcessPreset.TargetRealTimeMaximumQuality);
            List<Assimp.Mesh> meshes = assimpMeshObject.Meshes;

            Assimp.Mesh aMesh = meshes[0];
            List<StaticMeshVertex> vertexes = new List<StaticMeshVertex>(aMesh.VertexCount);
            for(int i = 0;i < aMesh.VertexCount;i++)
            {
                vertexes.Add(
                    new StaticMeshVertex(
                        aMesh.Vertices[i].GetAsOpenTK(),
                        aMesh.Normals[i].GetAsOpenTK(),
                        aMesh.Tangents[i].GetAsOpenTK(),
                        aMesh.BiTangents[i].GetAsOpenTK(),
                        new Vector2(aMesh.TextureCoordinateChannels[0][i].X,- aMesh.TextureCoordinateChannels[0][i].Y)
                    ));
            }
            StaticMesh mesh = new StaticMesh();
            mesh.SetVertexData(vertexes.ToArray());
            mesh.SetTriangleData(aMesh.GetIndices());
            return mesh;

            ///*
            // * Load line by line
            // */
            //string[] lines = File.ReadAllLines(path);

            //List<StaticMeshVertex> vertexes = new List<StaticMeshVertex>();
            //List<int> triangles = new List<int>();

            //List<Vector3> positions = new List<Vector3>();
            //List<Vector3> normals = new List<Vector3>();
            //List<Vector2> uvs = new List<Vector2>();
            //List<Vector3> tangents;
            //List<Vector3> bitangents;

            //List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();

            ///*
            // * Iterate lines
            // */
            //for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            //{
            //    /*
            //     * Get index
            //     */
            //    string line = lines[lineIndex];

            //    /*
            //     * Get header
            //     */
            //    string header = line.Substring(0, line.IndexOf(" "));
            //    string content = line.Substring(line.IndexOf(" ") + 1, line.Length - line.IndexOf(" ") - 1);

            //    /*
            //    * Catch vertex,normal and uv
            //    */
            //    switch (header)
            //    {
            //        case "v":
            //            {
            //                /*
            //                 * Split into components x,y,z
            //                 */
            //                string[] splitSubString = content.Split(" ");

            //                /*
            //                 * Add position
            //                 */
            //                positions.Add(new Vector3(float.Parse(splitSubString[0] ), float.Parse(splitSubString[1] ), float.Parse(splitSubString[2] )));
            //                break;
            //            }
            //        case "vn":
            //            {
            //                /*
            //                 * Sptlit into component x,y,z
            //                 */
            //                string[] splitSubString = content.Split(" ");

            //                /*
            //                 * Add normal
            //                 */
            //                normals.Add(new Vector3(float.Parse(splitSubString[0]), float.Parse(splitSubString[1] ), float.Parse(splitSubString[2] )));
            //                break;
            //            }
            //        case "vt":
            //            {
            //                /*
            //                 * Sptlit into component x,y
            //                 */
            //                string[] splitSubString = content.Split(" ");

            //                /*
            //                 * Add uv
            //                 */
            //                uvs.Add(new Vector2(float.Parse(splitSubString[0] ), float.Parse(splitSubString[1] )));
            //                break;
            //            }
            //        case "f":
            //            {
            //                /*
            //                 * Get each section start-end indexes
            //                 */
            //                string[] sections = content.Split(" ");

            //                /*
            //                 * Iterate face properties
            //                 */
            //                for (int sectionIndex = 0; sectionIndex < 3; sectionIndex++)
            //                {
            //                    /*
            //                     * Get Properties
            //                     */
            //                    int vertexIndex = int.Parse(sections[sectionIndex].Split("/")[0]);
            //                    int uvIndex = int.Parse(sections[sectionIndex].Split("/")[1]);
            //                    int normalIndex = int.Parse(sections[sectionIndex].Split("/")[2]);

            //                    faces.Add(new Tuple<int, int, int>(vertexIndex, uvIndex, normalIndex));
            //                }
            //                break;
            //            }
            //        default:
            //            break;
            //    }
            //}

            ///*
            // * Create vertexes with only positons
            // */
            //for (int vertexIndex = 0; vertexIndex < positions.Count; vertexIndex++)
            //{
            //    /*
            //     * Initialize vertex
            //     */
            //    Vector3 position = positions[vertexIndex];
            //    Vector3 normal = Vector3.Zero;
            //    Vector2 uv = Vector2.Zero;

            //    /*
            //     * Look for normal and uv
            //     */
            //    for (int faceIndex = 0; faceIndex < faces.Count; faceIndex++)
            //    {
            //        /*
            //        * Get face
            //        */
            //        Tuple<int, int, int> face = faces[faceIndex];

            //        /*
            //         * Validate normal and uv
            //         */
            //        if (face.Item1 - 1 == vertexIndex) // vertex found
            //        {
            //            normal = normals[face.Item3 - 1];
            //            uv = uvs[face.Item2 - 1];
            //            break;
            //        }
            //    }

            //    /*
            //     * Add vertex
            //     */
            //    vertexes.Add(new StaticMeshVertex(position, normal,Vector3.Zero,Vector3.Zero, uv));
            //}


            //for (int faceIndex = 0; faceIndex < faces.Count; faceIndex++)
            //{
            //    /*
            //     * Get face
            //     */
            //    Tuple<int, int, int> face = faces[faceIndex];

            //    /*
            //     * Add triangle
            //     */
            //    triangles.Add(face.Item1 - 1);
            //}

            ///*
            // * Calculate tangent and bitangent
            // */
            //for(int triangleIndex = 0;triangleIndex < triangles.Count;triangleIndex+=3)
            //{
            //    /*
            //     * Get triangles
            //     */
            //    int triangle0 = triangles[triangleIndex];
            //    int triangle1 = triangles[triangleIndex+1];
            //    int triangle2 = triangles[triangleIndex+2];

            //    /*
            //     * Get positions
            //     */
            //    Vector3 position0 = vertexes[triangle0].Position;
            //    Vector3 position1 = vertexes[triangle1].Position;
            //    Vector3 position2 = vertexes[triangle2].Position;

            //    /*
            //     * Get uvs
            //     */
            //    Vector2 uv0 = vertexes[triangle0].Uv;
            //    Vector2 uv1 = vertexes[triangle1].Uv;
            //    Vector2 uv2 = vertexes[triangle2].Uv;

            //    /*
            //     * Calculate edges
            //     */
            //    Vector3 edge0 = position1 - position0;
            //    Vector3 edge1 = position2 - position0;

            //    /*
            //     * Calculate uv deltas
            //     */
            //    Vector2 uvDelta0 = uv1 - uv0;
            //    Vector2 uvDelta1 = uv2 - uv0;

            //    /*
            //     * Calculate tangent bitangent
            //     */
            //    Vector3 tangent;
            //    Vector3 bitangent;
            //    float f = 1.0f / (uvDelta0.X * uvDelta1.Y - uvDelta1.X * uvDelta0.Y);
            //    tangent.X = f * (uvDelta1.Y * edge0.X - uvDelta0.Y * edge1.X);
            //    tangent.Y = f * (uvDelta1.Y * edge0.Y - uvDelta0.Y * edge1.Y);
            //    tangent.Z = f * (uvDelta1.Y * edge0.Z - uvDelta0.Y * edge1.Z);

            //    bitangent.X = f * (-uvDelta1.X * edge0.X + uvDelta0.X * edge1.X);
            //    bitangent.Y = f * (-uvDelta1.Y * edge0.X + uvDelta0.X * edge1.X);
            //    bitangent.Z = f * (-uvDelta1.X * edge0.X + uvDelta0.X * edge1.X);

            //    /*
            //     * Set tangent and bitangents
            //     */
            //    Vector3 normal = vertexes[triangle0].Normal;

            //    vertexes[triangle0] = new StaticMeshVertex(position0,normal,tangent,bitangent,uv0);
            //    vertexes[triangle1] = new StaticMeshVertex(position1, normal, tangent, bitangent, uv1);
            //    vertexes[triangle2] = new StaticMeshVertex(position2, normal, tangent, bitangent, uv2);
            //}
            
            /*
             * Create mesh
            // */
            //StaticMesh mesh = new StaticMesh();
            //mesh.SetVertexData(vertexes.ToArray());
            //mesh.SetTriangleData(triangles.ToArray());

            //return mesh;
        }
        public StaticMesh()
        {
           
        }
        public override VertexLayout Layout
        {
            get
            {
                List<VertexLayoutElement> elements = new List<VertexLayoutElement>();
                elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_Position"));
                elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_Normal"));
                elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_Tangent"));
                elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float3, "v_BiTangent"));
                elements.Add(new VertexLayoutElement(VertexLayoutDataType.Float2, "v_Uv"));
                return new VertexLayout(elements.ToArray());
            }
        }


        /// <summary>
        /// Return the sprite vertex data
        /// </summary>
        public StaticMeshVertex[] VertexData
        {
            get
            {
                return m_CpuVertexes;
            }
        }

        /// <summary>
        /// Return the triangle data
        /// </summary>
        public int[] TriangleData
        {
            get
            {
                return m_CpuTriangles;
            }
        }


        /// <summary>
        /// Set vertex data of this mesh
        /// </summary>
        /// <param name="vertexes"></param>
        public void SetVertexData(StaticMeshVertex[] vertexes)
        {
            /*
             * Set vertex data
             */
            SetVertexBufferData(vertexes);

            /*
             * Set Cpu vertexes
             */
            m_CpuVertexes = vertexes;
        }

        /// <summary>
        /// Set triangle data of this mesh
        /// </summary>
        /// <param name="triangles"></param>
        public void SetTriangleData(int[] triangles)
        {
            /*
             * Set Index data
             */
            SetIndexBufferData(triangles);

            /*
             * Set cpu triangles
             */
            m_CpuTriangles = triangles;
        }

        public override void Destroy()
        {
            VertexBuffer.Destroy();
            IndexBuffer.Destroy();
            VertexBuffer = null;
            IndexBuffer = null;
            m_CpuTriangles = null;
            m_CpuVertexes = null;
        }

        private int[] m_CpuTriangles;
        private StaticMeshVertex[] m_CpuVertexes;
    }
}
