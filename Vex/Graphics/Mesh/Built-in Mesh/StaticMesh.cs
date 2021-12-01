using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("Extension: " + extension);
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
             * Load line by line
             */
            string[] lines = File.ReadAllLines(path);

            List<StaticMeshVertex> vertexes = new List<StaticMeshVertex>();
            List<int> triangles = new List<int>();

            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();

            List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();

            /*
             * Iterate lines
             */
            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                /*
                 * Get index
                 */
                string line = lines[lineIndex];

                /*
                 * Get header
                 */
                string header = line.Substring(0, line.IndexOf(" "));
                string content = line.Substring(line.IndexOf(" ") + 1, line.Length - line.IndexOf(" ") - 1);

                /*
                * Catch vertex,normal and uv
                */
                switch (header)
                {
                    case "v":
                        {
                            /*
                             * Split into components x,y,z
                             */
                            string[] splitSubString = content.Split(" ");

                            /*
                             * Add position
                             */
                            positions.Add(new Vector3(float.Parse(splitSubString[0] ), float.Parse(splitSubString[1] ), float.Parse(splitSubString[2] )));
                            break;
                        }
                    case "vn":
                        {
                            /*
                             * Sptlit into component x,y,z
                             */
                            string[] splitSubString = content.Split(" ");

                            /*
                             * Add normal
                             */
                            normals.Add(new Vector3(float.Parse(splitSubString[0]), float.Parse(splitSubString[1] ), float.Parse(splitSubString[2] )));
                            break;
                        }
                    case "vt":
                        {
                            /*
                             * Sptlit into component x,y
                             */
                            string[] splitSubString = content.Split(" ");

                            /*
                             * Add uv
                             */
                            uvs.Add(new Vector2(float.Parse(splitSubString[0] ), float.Parse(splitSubString[1] )));
                            break;
                        }
                    case "f":
                        {
                            /*
                             * Get each section start-end indexes
                             */
                            string[] sections = content.Split(" ");

                            /*
                             * Iterate face properties
                             */
                            for (int sectionIndex = 0; sectionIndex < 3; sectionIndex++)
                            {
                                /*
                                 * Get Properties
                                 */
                                int vertexIndex = int.Parse(sections[sectionIndex].Split("/")[0]);
                                int uvIndex = int.Parse(sections[sectionIndex].Split("/")[1]);
                                int normalIndex = int.Parse(sections[sectionIndex].Split("/")[2]);

                                faces.Add(new Tuple<int, int, int>(vertexIndex, uvIndex, normalIndex));


                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            /*
             * Create vertexes with only positons
             */
            for (int vertexIndex = 0; vertexIndex < positions.Count; vertexIndex++)
            {
                /*
                 * Initialize vertex
                 */
                Vector3 position = positions[vertexIndex];
                Vector3 normal = Vector3.Zero;
                Vector2 uv = Vector2.Zero;

                /*
                 * Look for normal and uv
                 */
                for (int faceIndex = 0; faceIndex < faces.Count; faceIndex++)
                {
                    /*
                    * Get face
                    */
                    Tuple<int, int, int> face = faces[faceIndex];

                    /*
                     * Validate normal and uv
                     */
                    if (face.Item1 - 1 == vertexIndex) // vertex found
                    {
                        normal = normals[face.Item3 - 1];
                        uv = uvs[face.Item2 - 1];
                        break;
                    }
                }

                /*
                 * Add vertex
                 */
                vertexes.Add(new StaticMeshVertex(position, normal, uv));
            }


            for (int faceIndex = 0; faceIndex < faces.Count; faceIndex++)
            {
                /*
                 * Get face
                 */
                Tuple<int, int, int> face = faces[faceIndex];

                /*
                 * Add triangle
                 */
                triangles.Add(face.Item1 - 1);
            }


            /*
             * Create mesh
             */
            StaticMesh mesh = new StaticMesh();
            mesh.SetVertexData(vertexes.ToArray());
            mesh.SetTriangleData(triangles.ToArray());

            return mesh;
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
            throw new NotImplementedException();
        }

        private int[] m_CpuTriangles;
        private StaticMeshVertex[] m_CpuVertexes;
    }
}
