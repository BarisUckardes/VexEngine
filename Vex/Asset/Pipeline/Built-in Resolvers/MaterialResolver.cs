using OpenTK.Mathematics;
using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using Vex.Framework;

namespace Vex.Asset
{
    public sealed class MaterialResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(Material);
            }
        }

        protected override object ReadAsset(IParser parser,AssetPool pool)
        {
            /*
             * Initialize variables
             */
            Guid programAssetID;
            ShaderProgram shaderProgram = null;
            List<ShaderStage> stages = new List<ShaderStage>();
            List<List<MaterialParameterField<float>>> floatParameters = new List<List<MaterialParameterField<float>>>();
            List<List<MaterialParameterField<Vector2>>> vector2Parameters = new List<List<MaterialParameterField<Vector2>>>();
            List<List<MaterialParameterField<Vector3>>> vector3Parameters = new List<List<MaterialParameterField<Vector3>>>();
            List<List<MaterialParameterField<Vector4>>> vector4Parameters = new List<List<MaterialParameterField<Vector4>>>();
            List<List<MaterialParameterField<Matrix4>>> matrix4x4Parameters = new List<List<MaterialParameterField<Matrix4>>>();
            List<List<MaterialParameterField<Texture2D>>> texture2DParameters = new List<List<MaterialParameterField<Texture2D>>>();

            /*
             * Move to program id
             */
            parser.MoveNext();

            /*
             * Get category
             */
            programAssetID = Guid.Parse(GetParserValue(parser));

            /*
             * Move to shader stages begin
             */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Iterate shader stages
             */
            while (GetParserValue(parser) != "Stage Parameters End")
            {
                /*
                 * Move to shader stage type
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Get shader stage
                 */
                stages.Add((ShaderStage)(int.Parse(GetParserValue(parser))));

                /*
                 * Move to float parameters begin
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Iterate flaot parameters
                 */
                List<MaterialParameterField<float>> floatParameterBlock = new List<MaterialParameterField<float>>();
                while (GetParserValue(parser) != "Float Parameters End")
                {
                    /*
                     * Get parameter name
                     */
                    string parameterName = GetParserValue(parser);

                    /*
                     * Move to value
                     */
                    parser.MoveNext();

                    /*
                     * Get parameter value
                     */
                    float value = float.Parse(GetParserValue(parser));

                    /*
                     * Move to next parameters
                     */
                    parser.MoveNext();

                    /*
                     * Register float params
                     */
                    floatParameterBlock.Add(new MaterialParameterField<float>(parameterName, value));
                }


                /*
                 * Move to vector2 parameters
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Iterate vector2 parameters
                 */
                List<MaterialParameterField<Vector2>> vector2ParameterBlock = new List<MaterialParameterField<Vector2>>();
                while (GetParserValue(parser) != "Vector2 Parameters End")
                {
                    /*
                     * Get parameter name
                     */
                    string parameterName = GetParserValue(parser);

                    /*
                     * Move to value
                     */
                    parser.MoveNext();

                    /*
                     * Get parameter value
                     */
                    string[] vectorXY = GetParserValue(parser).Split(" ");
                    Vector2 value = new Vector2(float.Parse(vectorXY[0]),float.Parse(vectorXY[1]));

                    /*
                     * Move to next parameters
                     */
                    parser.MoveNext();

                    /*
                     * Register Vector2 params
                     */
                    vector2ParameterBlock.Add(new MaterialParameterField<Vector2>(parameterName, value));
                }

                /*
                * Move to vector3 parameters
                */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Iterate vector3 parameters
                 */
                List<MaterialParameterField<Vector3>> vector3ParameterBlock = new List<MaterialParameterField<Vector3>>();
                while (GetParserValue(parser) != "Vector3 Parameters End")
                {
                    /*
                     * Get parameter name
                     */
                    string parameterName = GetParserValue(parser);

                    /*
                     * Move to value
                     */
                    parser.MoveNext();

                    /*
                     * Get parameter value
                     */
                    string[] vectorXYZ = GetParserValue(parser).Split(" ");
                    Vector3 value = new Vector3(float.Parse(vectorXYZ[0]), float.Parse(vectorXYZ[1]),float.Parse(vectorXYZ[2]));

                    /*
                     * Move to next parameters
                     */
                    parser.MoveNext();

                    /*
                     * Register Vector2 params
                     */
                    vector3ParameterBlock.Add(new MaterialParameterField<Vector3>(parameterName, value));
                }


                /*
                * Move to vector4 parameters
                */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Iterate vector4 parameters
                 */
                List<MaterialParameterField<Vector4>> vector4ParameterBlock = new List<MaterialParameterField<Vector4>>();
                while (GetParserValue(parser) != "Vector4 Parameters End")
                {
                    /*
                     * Get parameter name
                     */
                    string parameterName = GetParserValue(parser);

                    /*
                     * Move to value
                     */
                    parser.MoveNext();

                    /*
                     * Get parameter value
                     */
                    string[] vectorXY = GetParserValue(parser).Split(" ");
                    Vector4 value = new Vector4(float.Parse(vectorXY[0]), float.Parse(vectorXY[1]), float.Parse(vectorXY[2]),float.Parse(vectorXY[3]));

                    /*
                     * Move to next parameters
                     */
                    parser.MoveNext();

                    /*
                     * Register Vector2 params
                     */
                    vector4ParameterBlock.Add(new MaterialParameterField<Vector4>(parameterName, value));
                }

                /*
                * Move to matrix4x4 parameters
                */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Iterate matrix4x4 parameters
                 */
                List<MaterialParameterField<Matrix4>> matrix4x4ParameterBlock = new List<MaterialParameterField<Matrix4>>();
                while (GetParserValue(parser) != "Matrix4x4 Parameters End")
                {
                    /*
                     * Get parameter name
                     */
                    string parameterName = GetParserValue(parser);

                    /*
                     * Move to value
                     */
                    parser.MoveNext();

                    /*
                     * Get parameter value
                     */
                    string[] matrixElements = GetParserValue(parser).Split(" ");
                    Matrix4 value = new Matrix4(
                    float.Parse(matrixElements[0]), float.Parse(matrixElements[1]), float.Parse(matrixElements[2]), float.Parse(matrixElements[3]),
                    float.Parse(matrixElements[4]), float.Parse(matrixElements[5]), float.Parse(matrixElements[6]), float.Parse(matrixElements[7]),
                    float.Parse(matrixElements[8]), float.Parse(matrixElements[9]), float.Parse(matrixElements[10]), float.Parse(matrixElements[11]),
                    float.Parse(matrixElements[12]), float.Parse(matrixElements[13]), float.Parse(matrixElements[14]), float.Parse(matrixElements[15]));

                    /*
                     * Move to next parameters
                     */
                    parser.MoveNext();

                    /*
                     * Register Vector2 params
                     */
                    matrix4x4ParameterBlock.Add(new MaterialParameterField<Matrix4>(parameterName, value));
                }


                /*
                * Move to texture2D parameters
                */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Iterate texture2D parameters
                 */
                List<MaterialParameterField<Texture2D>> texture2DParameterBlock = new List<MaterialParameterField<Texture2D>>();
                while (GetParserValue(parser) != "Texture2D Parameters End")
                {
                    /*
                     * Get parameter name
                     */
                    string parameterName = GetParserValue(parser);

                    /*
                     * Move to value
                     */
                    parser.MoveNext();

                    /*
                     * Get parameter value
                     */
                    Texture2D texture = pool.GetOrLoadAsset(Guid.Parse(GetParserValue(parser))) as Texture2D;

                    /*
                     * Move to next parameters
                     */
                    parser.MoveNext();

                    /*
                     * Register Vector2 params
                     */
                    texture2DParameterBlock.Add(new MaterialParameterField<Texture2D>(parameterName, texture));
                }

                /*
                 * Register parameters
                 */
                floatParameters.Add(floatParameterBlock);
                vector2Parameters.Add(vector2ParameterBlock);
                vector3Parameters.Add(vector3ParameterBlock);
                vector4Parameters.Add(vector4ParameterBlock);
                matrix4x4Parameters.Add(matrix4x4ParameterBlock);
                texture2DParameters.Add(texture2DParameterBlock);

                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

            }

            /*
             * Move to mapping end
             */
            parser.MoveNext();

            /*
             * Try load shader program
             */
            VexObject shaderProgramObject = pool.GetOrLoadAsset(programAssetID);
            shaderProgram = shaderProgramObject == null ? null : shaderProgramObject as ShaderProgram;

            /*
             * Create material
             */
            Material material = new Material(shaderProgram);

            /*
             * Set all parameters
             */
            for (int i = 0; i < stages.Count; i++)
            {
                MaterialStageParameters stageParametersRef = material.GetStageParameters(stages[i]);

                /*
                 * Set float parameters
                 */
                for (int p = 0; p < floatParameters[i].Count; p++)
                {
                    stageParametersRef.SetFloatParameter(floatParameters[i][p].Name, floatParameters[i][p].Data);
                }

                /*
               * Set vector2 parameter
               */
                for (int p = 0; p < vector2Parameters[i].Count; p++)
                {
                    stageParametersRef.SetVector2Parameter(vector2Parameters[i][p].Name, vector2Parameters[i][p].Data);
                }

                /*
               * Set vector3 parameter
               */
                for (int p = 0; p < vector3Parameters[i].Count; p++)
                {
                    stageParametersRef.SetVector3Parameter(vector3Parameters[i][p].Name, vector3Parameters[i][p].Data);
                }

                /*
                * Set vector4 parameter
                */
                for (int p = 0; p < vector4Parameters[i].Count; p++)
                {
                    stageParametersRef.SetVector4Parameter(vector4Parameters[i][p].Name, vector4Parameters[i][p].Data);
                }

                /*
                 * Set matrix4x4 parameter
                 */
                for (int p = 0; p < matrix4x4Parameters[i].Count; p++)
                {
                    stageParametersRef.SetMatrix4x4Parameter(matrix4x4Parameters[i][p].Name, matrix4x4Parameters[i][p].Data);
                }

                /*
                 * Set texture2D parameter
                 */
                for (int p = 0; p < texture2DParameters[i].Count; p++)
                {
                    stageParametersRef.SetTexture2DParameter(texture2DParameters[i][p].Name, texture2DParameters[i][p].Data);
                }

            }

            return material;
        }

        protected override void WriteAsset(IEmitter emitter, object engineObject)
        {
            /*
             * Get material
             */
            Material material = engineObject as Material;

            /*
            * Emit shaders
            */
            string programYaml;
            if (material.Program == null)
                programYaml = Guid.Empty.ToString();
            else
                programYaml = material.Program.ID.ToString();

            /*
             * Mapping start
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

           
            /*
             * Write shader program id
             */
            emitter.Emit(new Scalar(null, "Shader Program"));
            emitter.Emit(new Scalar(null, programYaml));

            /*
             * Emit shader stage parameters
             */
            MaterialStageParameters[] stageParameters = material.StageParameters;

            /*
             * Set stage parameters begin
             */
            emitter.Emit(new Scalar(null, "Stage Parameters Begin"));
            emitter.Emit(new Scalar(null, ""));
            for (int stageIndex = 0; stageIndex < stageParameters.Length; stageIndex++)
            {
                /*
                 * Emit stage signal
                 */
                emitter.Emit(new Scalar(null, "Stage Begin"));
                emitter.Emit(new Scalar(null, ""));

                /*
                 * Emit stage begin
                 */
                emitter.Emit(new Scalar(null, "Stage"));
                emitter.Emit(new Scalar(null, ((int)stageParameters[stageIndex].Stage).ToString()));

                /*
                 * Signal float parameters begin
                 */
                emitter.Emit(new Scalar(null, "Float Parameters Begin"));
                emitter.Emit(new Scalar(null, ""));

                /*
                 * Begin float parameters
                 */
                MaterialParameterField<float>[] floatParameters = stageParameters[stageIndex].FloatParameters;
                for (int parameterIndex = 0; parameterIndex < floatParameters.Length; parameterIndex++)
                {
                    emitter.Emit(new Scalar(null, floatParameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, floatParameters[parameterIndex].Data.ToString()));
                }

                /*
                 * Signal float parameters end
                 */
                emitter.Emit(new Scalar(null, "Float Parameters End"));
                emitter.Emit(new Scalar(null, ""));

                /*
                 * Signal vector2 parameters begin
                 */
                emitter.Emit(new Scalar(null, "Vector2 Parameters Begin"));
                emitter.Emit(new Scalar(null, ""));

                /*
                 * Begin vector2 parameters
                 */
                MaterialParameterField<Vector2>[] vector2Parameters = stageParameters[stageIndex].Vector2Parameters;
                for (int parameterIndex = 0; parameterIndex < vector2Parameters.Length; parameterIndex++)
                {
                    Vector2 vector = vector2Parameters[parameterIndex].Data;
                    string vectorYaml = vector.X.ToString() + " " + vector.Y.ToString();


                    emitter.Emit(new Scalar(null, vector2Parameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, vectorYaml));
                }

                /*
                 * Signal vector3 parameters end
                 */
                emitter.Emit(new Scalar(null, "Vector2 Parameters End"));
                emitter.Emit(new Scalar(null, ""));

                /*
                * Signal vector3 parameters begin
                */
                emitter.Emit(new Scalar(null, "Vector3 Parameters Begin"));
                emitter.Emit(new Scalar(null, ""));

                /*
                 * Begin vector3 parameters
                 */
                MaterialParameterField<Vector3>[] vector3Parameters = stageParameters[stageIndex].Vector3Parameters;
                for (int parameterIndex = 0; parameterIndex < vector3Parameters.Length; parameterIndex++)
                {
                    Vector3 vector = vector3Parameters[parameterIndex].Data;
                    string vectorYaml = vector.X.ToString() + " " + vector.Y.ToString() + " " + vector.Z.ToString();


                    emitter.Emit(new Scalar(null, vector3Parameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, vectorYaml));
                }

                /*
                 * Signal vector3 parameters end
                 */
                emitter.Emit(new Scalar(null, "Vector3 Parameters End"));
                emitter.Emit(new Scalar(null, ""));


                /*
                * Signal vector3 parameters begin
                */
                emitter.Emit(new Scalar(null, "Vector4 Parameters Begin"));
                emitter.Emit(new Scalar(null, ""));

                /*
                 * Begin vector4 parameters
                 */
                MaterialParameterField<Vector4>[] vector4Parameters = stageParameters[stageIndex].Vector4Parameters;
                for (int parameterIndex = 0; parameterIndex < vector4Parameters.Length; parameterIndex++)
                {
                    Vector4 vector = vector4Parameters[parameterIndex].Data;
                    string vectorYaml = vector.X.ToString() + " " + vector.Y.ToString() + " " + vector.Z.ToString() + " " + vector.W.ToString();


                    emitter.Emit(new Scalar(null, vector4Parameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, vectorYaml));
                }

                /*
                 * Signal vector3 parameters end
                 */
                emitter.Emit(new Scalar(null, "Vector4 Parameters End"));
                emitter.Emit(new Scalar(null, ""));


                /*
                * Signal matrix4 parameters start
                */
                emitter.Emit(new Scalar(null, "Matrix4x4 Parameters Begin"));
                emitter.Emit(new Scalar(null, ""));

                /*
                * Begin Matrix4x4 parameters
                 */
                MaterialParameterField<Matrix4>[] matrix4x4Parameters = stageParameters[stageIndex].Matrix4x4Parameters;
                for (int parameterIndex = 0; parameterIndex < matrix4x4Parameters.Length; parameterIndex++)
                {
                    Matrix4 matrix = matrix4x4Parameters[parameterIndex].Data;
                    string matrixYaml =
                        matrix.Row0.X + " " + matrix.Row0.Y + " " + matrix.Row0.Z + " " + matrix.Row0.W +
                        " " + matrix.Row1.X + " " + matrix.Row1.Y + " " + matrix.Row1.Z + " " + matrix.Row1.W +
                        " " + matrix.Row2.X + " " + matrix.Row2.Y + " " + matrix.Row2.Z + " " + matrix.Row2.W +
                        " " + matrix.Row3.X + " " + matrix.Row3.Y + " " + matrix.Row3.Z + " " + matrix.Row3.W;

                    emitter.Emit(new Scalar(null, matrix4x4Parameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, matrixYaml));
                }


                /*
                * Signal matrix4 parameters end
                */
                emitter.Emit(new Scalar(null, "Matrix4x4 Parameters End"));
                emitter.Emit(new Scalar(null, ""));

                /*
                * Signal texture2d parameters begin
                */
                emitter.Emit(new Scalar(null, "Texture2D Parameters Begin"));
                emitter.Emit(new Scalar(null, ""));

                /*
                 * Begin Texture2D parameters
                 */
                MaterialParameterField<Texture2D>[] texture2DParameters = stageParameters[stageIndex].Texture2DParameters;
                for (int parameterIndex = 0; parameterIndex < texture2DParameters.Length; parameterIndex++)
                {
                    string textureYaml;
                    if (texture2DParameters[parameterIndex].Data == null)
                    {
                        textureYaml = Guid.Empty.ToString();
                    }
                    else
                    {
                        textureYaml = texture2DParameters[parameterIndex].Data.ID.ToString();
                    }

                    emitter.Emit(new Scalar(null, texture2DParameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, textureYaml));
                }

                /*
               * Signal texture2d parameters begin
               */
                emitter.Emit(new Scalar(null, "Texture2D Parameters End"));
                emitter.Emit(new Scalar(null, ""));

                /*
                * Signal stage end
                */
                emitter.Emit(new Scalar(null, "Stage End"));
                emitter.Emit(new Scalar(null, ""));

            }
            emitter.Emit(new Scalar(null, "Stage Parameters End"));
            emitter.Emit(new Scalar(null, ""));

            /*
             * Mapping end
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
