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
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector3> tangents = new List<Vector3>();
            List<Vector3> bitangents = new List<Vector3>();
            List<Vector2> textureCoordinates = new List<Vector2>();
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
                    float.Parse(vertexString[0]),
                    float.Parse(vertexString[1]),
                    float.Parse(vertexString[2]));
                Vector3 normal = new Vector3(
                    float.Parse(vertexString[3]),
                    float.Parse(vertexString[4]),
                    float.Parse(vertexString[5]));
                Vector3 tangent = new Vector3(
                   float.Parse(vertexString[6]),
                   float.Parse(vertexString[7]),
                   float.Parse(vertexString[8]));
                Vector3 biTangent = new Vector3(
                   float.Parse(vertexString[9]),
                   float.Parse(vertexString[10]),
                   float.Parse(vertexString[11]));
                Vector2 uv = new Vector2(
                    float.Parse(vertexString[12]),
                    float.Parse(vertexString[13]));
                positions.Add(position);
                normals.Add(normal);
                tangents.Add(tangent);
                bitangents.Add(biTangent);
                textureCoordinates.Add(uv);

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
            mesh.Positions = positions;
            mesh.Normals = normals;
            mesh.Tangents = tangents;
            mesh.BiTangents = bitangents;
            mesh.TextureCoordinates = textureCoordinates;
            mesh.Triangles = triangles;
            mesh.ApplyChanges();
            return mesh;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
        {
            /*
             * Get static mesh
             */
            StaticMesh mesh = targetObject as StaticMesh;
            List<Vector3> positions = mesh.Positions;
            List<Vector3> normals = mesh.Normals;
            List<Vector3> tangents = mesh.Tangents;
            List<Vector3> bitangents = mesh.BiTangents;
            List<Vector2> textureCoordinates =mesh.TextureCoordinates;
            List<int> triangles = mesh.Triangles;

            /*
             * Start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Emit vertexes
             */
            emitter.Emit(new Scalar(null, "Vertexes"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for(int vertexIndex = 0;vertexIndex < positions.Count;vertexIndex++)
            {
                /*
                 * Get vertex
                 */
                Vector3 position = positions[vertexIndex];
                Vector3 normal = normals[vertexIndex];
                Vector3 tangent = tangents[vertexIndex];
                Vector3 bitangent = bitangents[vertexIndex];
                Vector2 textureCoordinate = textureCoordinates[vertexIndex];

                string vertexText =
                    position.X.ToString() + " " + position.Y.ToString() + " " + position.Z.ToString() +
                    " " + normal.X.ToString() + " " + normal.Y.ToString() + " " + normal.Z.ToString() +
                    " " + tangent.X.ToString() + " " + tangent.Y.ToString() + " " + tangent.Z.ToString() +
                    " " + bitangent.X.ToString() + " " + bitangent.Y.ToString() + " " + bitangent.Z.ToString() +
                    " " + textureCoordinate.X.ToString() + " " + textureCoordinate.Y.ToString();

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
            for (int triangleIndex = 0; triangleIndex < triangles.Count; triangleIndex+=3)
            {
                /*
                 * Emiter triangle
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
