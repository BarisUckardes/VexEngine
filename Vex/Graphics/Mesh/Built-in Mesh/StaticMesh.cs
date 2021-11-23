using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
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
            Console.WriteLine("Obj lines: " + lines.Length);
            /*
             * Iterate lines
             */
            for(int lineIndex = 0;lineIndex < lines.Length;lineIndex++)
            {
                /*
                 * Get index
                 */
                string line = lines[lineIndex];

                /*
                 * Get header
                 */
                string header = line.Substring(0, line.IndexOf(" "));
                string content =line.Substring(line.IndexOf(" ")+1,line.Length- line.IndexOf(" ")-1);

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
                             * Get components
                             */
                            positions.Add(new Vector3(float.Parse(splitSubString[0]), float.Parse(splitSubString[1]), float.Parse(splitSubString[2])));
                            break;
                        }
                    case "vn":
                        {
                            
                            break;
                        }
                    case "f":
                        {
                            /*
                             * Get each section start-end indexes
                             */
                            string[] sections = content.Split(" ");

                            /*
                             * Get only triangles
                             */
                            for(int sectionIndex = 0;sectionIndex < 3;sectionIndex++)
                            {
                                /*
                                 * Get triangle string
                                 */
                                string triangleIndexString = sections[sectionIndex].Split("/")[0];

                                /*
                                 * Get triangle index
                                 */
                                triangles.Add(int.Parse(triangleIndexString));
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            /*
             * Create vertexes
             */
            for(int positionIndex = 0;positionIndex < positions.Count;positionIndex++)
            {
                vertexes.Add(new StaticMeshVertex(positions[positionIndex], Vector3.Zero, Vector2.Zero));
            }

            /*
             * Create mesh
             */
            StaticMesh mesh = new StaticMesh();
            mesh.SetVertexData(vertexes.ToArray());
            mesh.SetTriangleData(triangles.ToArray());

            return mesh;
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

        private int[] m_CpuTriangles;
        private StaticMeshVertex[] m_CpuVertexes;
    }
}
