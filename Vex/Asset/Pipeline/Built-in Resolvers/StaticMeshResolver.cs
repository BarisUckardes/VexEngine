using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Graphics;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Vex.Asset
{
    public sealed class StaticMeshResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(StaticMesh);
            }
        }

        protected override object ReadAsset(IParser parser, AssetPool pool)
        {
            /*
             * Initialize
             */
            StaticMesh mesh = new StaticMesh();
            List<StaticMeshVertex> vertexes = new List<StaticMeshVertex>();
            List<int> triangles = new List<int>();

            /*
             * Move to vertexes seq start
             */
            parser.MoveNext();
            parser.MoveNext();
            while(parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get vertex
                 */
                string[] vertexString = GetParserValue(parser).Split(" ");

                /*
                 * Create and add new vertex
                 */
                Vector3 position = new Vector3(
                    float.Parse(vertexString[0], CultureInfo.InvariantCulture),
                    float.Parse(vertexString[1], CultureInfo.InvariantCulture),
                    float.Parse(vertexString[2], CultureInfo.InvariantCulture));
                Vector3 normal = new Vector3(
                    float.Parse(vertexString[3], CultureInfo.InvariantCulture),
                    float.Parse(vertexString[4], CultureInfo.InvariantCulture),
                    float.Parse(vertexString[5], CultureInfo.InvariantCulture));
                Vector2 uv = new Vector2(
                    float.Parse(vertexString[6], CultureInfo.InvariantCulture),
                    float.Parse(vertexString[7], CultureInfo.InvariantCulture));
                vertexes.Add(new StaticMeshVertex(position, normal, uv));


                /*
                 * Move to next vertex
                 */
                parser.MoveNext();
            }

            /*
             * Move to triangles
             */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();
            while(parser.Current.GetType() != typeof(SequenceEnd))
            {
                ///*
                // * Get triangle batch
                // */
                string[] triangleBatch = GetParserValue(parser).Split(" ");

                ///*
                // * Create and add new triangles
                // */
                triangles.Add(int.Parse(triangleBatch[0]));
                triangles.Add(int.Parse(triangleBatch[1]));
                triangles.Add(int.Parse(triangleBatch[2]));

                /*
                 * Move to next triangle
                 */
                parser.MoveNext();
            }

            /*
             * Set mesh data and return
             */
            mesh.SetVertexData(vertexes.ToArray());
            mesh.SetTriangleData(triangles.ToArray());
            return mesh;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
        {
            /*
             * Get static mesh
             */
            StaticMesh mesh = targetObject as StaticMesh;
            StaticMeshVertex[] vertexes = mesh.VertexData;
            int[] triangles = mesh.TriangleData;

            /*
             * Start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Emit vertexes
             */
            emitter.Emit(new Scalar(null, "Vertexes"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for(int vertexIndex = 0;vertexIndex < vertexes.Length;vertexIndex++)
            {
                /*
                 * Get vertex
                 */
                StaticMeshVertex vertex = vertexes[vertexIndex];

                string vertexText = vertex.Position.X.ToString(CultureInfo.InvariantCulture) + " " + vertex.Position.Y.ToString(CultureInfo.InvariantCulture) + " " + vertex.Position.Z.ToString(CultureInfo.InvariantCulture) +
                    " " + vertex.Normal.X.ToString(CultureInfo.InvariantCulture) + " " + vertex.Normal.Y.ToString(CultureInfo.InvariantCulture) + " " + vertex.Normal.Z.ToString(CultureInfo.InvariantCulture) +
                    " " + vertex.Uv.X.ToString(CultureInfo.InvariantCulture) + " " + vertex.Uv.Y.ToString(CultureInfo.InvariantCulture);

                /*
                 * Emiter vertex
                 */
                emitter.Emit(new Scalar(null, vertexText));
            }
            emitter.Emit(new SequenceEnd());

            /*
             * Emit triangles
             */
            emitter.Emit(new Scalar(null, "Triangles"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for (int triangleIndex = 0; triangleIndex < triangles.Length; triangleIndex+=3)
            {
                /*
                 * Emiter vertex
                 */
                emitter.Emit(new Scalar(null, triangles[triangleIndex].ToString() + " " + triangles[triangleIndex+1].ToString() + " " + triangles[triangleIndex+2].ToString()));

            }
            emitter.Emit(new SequenceEnd());

            /*
             * End mapping
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
