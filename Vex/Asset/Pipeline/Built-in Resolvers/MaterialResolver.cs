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
            string category;
            string categoryName;
            ShaderProgram ShaderProgram;
            List<ShaderStage> stages = new List<ShaderStage>();
            List<List<MaterialParameterField<float>>> floatParameters = new List<List<MaterialParameterField<float>>>();
            List<List<MaterialParameterField<Vector4>>> vector4Parameters = new List<List<MaterialParameterField<Vector4>>>();
            List<List<MaterialParameterField<Matrix4>>> matrix4x4Parameters = new List<List<MaterialParameterField<Matrix4>>>();
            List<List<MaterialParameterField<Texture2D>>> texture2DParameters = new List<List<MaterialParameterField<Texture2D>>>();


            /*
             * Move to category
             */
            parser.MoveNext();

            /*
             * Get category
             */
            category = GetParserValue(parser);

            /*
             * Move to category name
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get category name
             */
            categoryName = GetParserValue(parser);

            /*
             * Move to shader pVexgram
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get shader pVexgram
             */
            {
                string pVexgramYaml = GetParserValue(parser);

                /*
                 * Validate pVexgram
                 */
                if(pVexgramYaml.Contains("[NULL]"))
                {
                    ShaderProgram = null;
                }
                else
                {
                    /*
                     * Validate guid
                     */
                    Guid id;
                    bool parsed = Guid.TryParse(pVexgramYaml.Replace("[ASSET]", ""), out id);

                    if(parsed) // parse successful
                    {
                        ShaderProgram = pool.GetOrLoadAsset(id) as ShaderProgram;
                    }
                    else // parse failed
                    {
                        ShaderProgram = null;
                    }
                }
            }

            /*
             * Move to stage count
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Get stage parameter count
             */
            int stageCount = Convert.ToInt32(GetParserValue(parser));

            /*
             * Get stage parameters
             */
            for(int stageIndex = 0; stageIndex < stageCount;stageIndex++)
            {
                /*
                 * Move to stage type
                 */
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Get stage type
                 */
                Console.WriteLine("Stage Type: " + GetParserValue(parser));
                ShaderStage stage = (ShaderStage)(Convert.ToInt32(GetParserValue(parser)));

                /*
                 * Move to float parameters
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Get float parameters
                 */
                List<MaterialParameterField<float>> floatParams = new List<MaterialParameterField<float>>();
                while(GetParserValue(parser) != "End Parameters")
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
                     * Get data
                     */
                    float value = float.Parse(GetParserValue(parser));

                    /*
                    * Add parameter
                    */
                    floatParams.Add(new MaterialParameterField<float>(parameterName, value, 0));

                    /*
                     * Move to next parameter
                     */
                    parser.MoveNext();
                }

                /*
                 * Move to matrix4x4 parameters
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Get matrix4x4 parameters
                 */
                List<MaterialParameterField<Matrix4>> matrix4x4Params = new List<MaterialParameterField<Matrix4>>();
                while(GetParserValue(parser) != "End Parameters")
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
                     * Get data
                     */
                    string[] matrixElements = GetParserValue(parser).Split(" ");
                    Matrix4 value = new Matrix4(
                        float.Parse(matrixElements[0]), float.Parse(matrixElements[1]), float.Parse(matrixElements[2]), float.Parse(matrixElements[3]),
                        float.Parse(matrixElements[4]), float.Parse(matrixElements[5]), float.Parse(matrixElements[6]), float.Parse(matrixElements[7]),
                        float.Parse(matrixElements[8]), float.Parse(matrixElements[9]), float.Parse(matrixElements[10]), float.Parse(matrixElements[11]),
                        float.Parse(matrixElements[12]), float.Parse(matrixElements[13]), float.Parse(matrixElements[14]), float.Parse(matrixElements[15]));


                    /*
                    * Add parameter
                    */
                    matrix4x4Params.Add(new MaterialParameterField<Matrix4>(parameterName, value, 0));

                    /*
                     * Move to next parameter
                     */
                    parser.MoveNext();
                }

                /*
                * Move to vector4 parameters
                */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Get vecto4 parameters
                 */
                List<MaterialParameterField<Vector4>> vector4Params = new List<MaterialParameterField<Vector4>>();
                while (GetParserValue(parser) != "End Parameters")
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
                     * Get data
                     */
                    string[] vectorElements = GetParserValue(parser).Split(" ");
                    Vector4 value = new Vector4(float.Parse(vectorElements[0]), float.Parse(vectorElements[1]), float.Parse(vectorElements[2]), float.Parse(vectorElements[3]));

                    /*
                    * Add parameter
                    */
                    vector4Params.Add(new MaterialParameterField<Vector4>(parameterName, value, 0));

                    /*
                     * Move to next parameter
                     */
                    parser.MoveNext();
                }

                /*
                * Move to texture2D parameters
                */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Get texture2D parameters
                 */
                List<MaterialParameterField<Texture2D>> texture2DParams = new List<MaterialParameterField<Texture2D>>();
                while (GetParserValue(parser) != "End Parameters")
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
                     * Get data
                     */
                    Texture2D texture = null;
                    string textureYaml = GetParserValue(parser);
                    if(!textureYaml.Contains("[NULL]"))
                    {
                        string guidYaml = textureYaml.Replace("[ASSET]","");

                        Guid id;

                        /*
                         * Validate guid
                         */
                        bool parsed = Guid.TryParse(guidYaml, out id);

                        /*
                         * If parsed try load asset
                         */
                        if(parsed)
                        {
                            texture = pool.GetOrLoadAsset(id) as Texture2D;
                        }
                    }


                    /*
                    * Add parameter
                    */
                    texture2DParams.Add(new MaterialParameterField<Texture2D>(parameterName, texture, 0));

                    /*
                     * Move to next parameter
                     */
                    parser.MoveNext();
                }

                /*
                 * Move to next stage
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Set stage and parameters
                 */
                stages.Add(stage);
                floatParameters.Add(floatParams);
                matrix4x4Parameters.Add(matrix4x4Params);
                vector4Parameters.Add(vector4Params);

            }


            /*
             * Create material
             */
            Material material = new Material(ShaderProgram);

            /*
             * Validate material and return
             */
            if (ShaderProgram == null)
                return material;

            /*
             * Set all parameters
             */
            for(int i=0;i<stages.Count;i++)
            {
                MaterialStageParameters stageParametersRef = material.GetStageParameters(stages[i]);

                /*
                 * Set float parameters
                 */
                for(int p=0;p<floatParameters[i].Count;p++)
                {
                    stageParametersRef.SetFloatParameter(floatParameters[i][p].Name, floatParameters[i][p].Data);
                }

                /*
                 * Set matrix4x4 parameter
                 */
                for (int p = 0; p < matrix4x4Parameters[i].Count; p++)
                {
                    stageParametersRef.SetMatrix4x4Parameter(matrix4x4Parameters[i][p].Name, matrix4x4Parameters[i][p].Data);
                }

                /*
                 * Set vector4 parameter
                 */
                for (int p = 0; p < vector4Parameters[i].Count; p++)
                {
                    stageParametersRef.SetVector4Parameter(vector4Parameters[i][p].Name, vector4Parameters[i][p].Data);
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
             * Mapping start
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

      

            /*
             * Emit shaders
             */
            string programYaml;
            if (material.Program == null)
                programYaml = Guid.Empty.ToString();
            else
                programYaml = material.Program.ID.ToString();

            emitter.Emit(new Scalar(null, "Shader PVexgram"));
            emitter.Emit(new Scalar(null, programYaml));

            /*
             * Emit shader stage parameters
             */
            MaterialStageParameters[] stageParameters = material.StageParameters;

            /*
             * Emit stage count
             */
            emitter.Emit(new Scalar(null, "Stage count"));
            emitter.Emit(new Scalar(null, stageParameters.Length.ToString()));

            /*
             * Set stage parameters begin
             */
            for(int stageIndex=0;stageIndex<stageParameters.Length; stageIndex++)
            {
                /*
                 * Emit stage begin
                 */
                emitter.Emit(new Scalar(null, "Stage Begin"));
                emitter.Emit(new Scalar(null, ((int)stageParameters[stageIndex].Stage).ToString()));

                /*
                 * Emit float parameters
                 */
                emitter.Emit(new Scalar(null, "Begin Parameters"));
                emitter.Emit(new Scalar(null, "Float"));

                MaterialParameterField<float>[] floatParameters = stageParameters[stageIndex].FloatParameters;
                for(int parameterIndex = 0; parameterIndex < floatParameters.Length; parameterIndex++)
                {
                    emitter.Emit(new Scalar(null, floatParameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, floatParameters[parameterIndex].Data.ToString()));
                }
               
                emitter.Emit(new Scalar(null, "End Parameters"));
                emitter.Emit(new Scalar(null, "Float"));

                /*
                * Emit Matrix4x4 parameters
                 */
                emitter.Emit(new Scalar(null, "Begin Parameters"));
                emitter.Emit(new Scalar(null, "Matrix4x4"));

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

                emitter.Emit(new Scalar(null, "End Parameters"));
                emitter.Emit(new Scalar(null, "Matrix4x4"));

                /*
                * Emit Vector4 parameters
                 */
                emitter.Emit(new Scalar(null, "Begin Parameters"));
                emitter.Emit(new Scalar(null, "Vecto4"));

                MaterialParameterField<Vector4>[] vector4Parameters = stageParameters[stageIndex].Vector4Parameters;
                for (int parameterIndex = 0; parameterIndex < vector4Parameters.Length; parameterIndex++)
                {
                    Vector4 vector = vector4Parameters[parameterIndex].Data;
                    string vectorYaml = vector.X.ToString() + " " + vector.Y.ToString() + " " + vector.Z.ToString() + " " + vector.W.ToString();
                       

                    emitter.Emit(new Scalar(null, vector4Parameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, vectorYaml));
                }

                emitter.Emit(new Scalar(null, "End Parameters"));
                emitter.Emit(new Scalar(null, "Vector4"));

                /*
                 * Emit Texture2D parameters
                 */
                emitter.Emit(new Scalar(null, "Begin Parameters"));
                emitter.Emit(new Scalar(null, "Texture2D"));

                MaterialParameterField<Texture2D>[] texture2DParameters = stageParameters[stageIndex].Texture2DParameters;
                for (int parameterIndex = 0; parameterIndex < texture2DParameters.Length; parameterIndex++)
                {
                    string textureYaml;
                    if(texture2DParameters[parameterIndex].Data == null)
                    {
                        textureYaml = "[NULL]";
                    }
                    else
                    {
                        textureYaml = "[ASSET]" + texture2DParameters[parameterIndex].Data.ID.ToString();
                    }
                    emitter.Emit(new Scalar(null, texture2DParameters[parameterIndex].Name));
                    emitter.Emit(new Scalar(null, textureYaml));
                }

                emitter.Emit(new Scalar(null, "End Parameters"));
                emitter.Emit(new Scalar(null, "Texture2D"));

                /*
                 * Emit stage end
                 */
                emitter.Emit(new Scalar(null, "Stage End"));
                emitter.Emit(new Scalar(null, ((int)stageParameters[stageIndex].Stage).ToString()));
            }
          

            /*
             * Mapping end
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
