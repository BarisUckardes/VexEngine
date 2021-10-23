using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
namespace Vex.Asset
{
    public sealed class SpriteMeshResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(SpriteMesh);
            }
        }

        public override object GetObject(IParser parser, in AssetPool pool)
        {
            List<SpriteVertex> vertexes = new List<SpriteVertex>();
            List<int> triangles = new List<int>();


            /*
             * Move to vertexes
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Collect vertexes
             */
            while(parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get vertex data
                 */
                string[] vertexYaml = GetParserValue(parser).Split(" ");

                /*
                 * add vertexes
                 */
                SpriteVertex vertex = new SpriteVertex(float.Parse(vertexYaml[0]), float.Parse(vertexYaml[1]), float.Parse(vertexYaml[2]), float.Parse(vertexYaml[3]));
                vertexes.Add(vertex);

                /*
                 * Move next element
                 */
                parser.MoveNext();
            }

            /*
             * Move to triangles
             */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Collect triangles
             */
            while(parser.Current.GetType() != typeof(SequenceEnd))
            {
                triangles.Add(Convert.ToInt32(GetParserValue(parser)));
                parser.MoveNext();
            }

     
            /*
             * Create Sprite mesh
             */
            SpriteMesh mesh = new SpriteMesh();
            mesh.SetVertexData(vertexes.ToArray());
            mesh.SetTriangleData(triangles.ToArray());

            return mesh;
        }

        public override void GetYaml(IEmitter emitter, object engineObject)
        {
            /*
             * Get sprite mesh
             */
            SpriteMesh mesh = engineObject as SpriteMesh;
            SpriteVertex[] vertexData = mesh.VertexData;
            int[] triangleData = mesh.TriangleData;

            /*
             * Start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Set vertex data
             */
            emitter.Emit(new Scalar(null, "Vertexes"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            
            for(int i=0;i<vertexData.Length;i++)
            {
                SpriteVertex vertex = vertexData[i];
                emitter.Emit(new Scalar(null,vertex.Position.X + " " + vertex.Position.Y + " " + vertex.Uv.X + " " + vertex.Uv.Y ));
            }

            emitter.Emit(new SequenceEnd());

            /*
             * Set triangle data
             */
            emitter.Emit(new Scalar(null, "Triangles"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));

            for(int i=0;i<triangleData.Length;i++)
            {
                emitter.Emit(new Scalar(null, triangleData[i].ToString()));
            }

            emitter.Emit(new SequenceEnd());

            /*
             * End mapping
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
