using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using Vex.Framework;
using Vex.Graphics;

namespace Vex.Asset
{
    /// <summary>
    /// An interface for asset creation
    /// </summary>
    public class AssetInterface
    {
        public AssetInterface(AssetPool pool)
        {
            m_Pool = pool;
        }

        /// <summary>
        /// Get asset object via yamlText
        /// </summary>
        /// <param name="yamlText"></param>
        /// <returns></returns>
        public VexObject GenerateAsset(AssetType type,string yamlText)
        {
            Console.WriteLine("Try generate asset with type " + type.ToString());
            switch (type)
            {
                case AssetType.Undefined:
                    break;
                case AssetType.Texture2D:
                    return new DeserializerBuilder().WithTypeConverter(new Texture2DResolver()).Build().Deserialize<Texture2D>(yamlText);
                    break;
                case AssetType.Shader:
                    return new DeserializerBuilder().WithTypeConverter(new ShaderResolver()).Build().Deserialize<Shader>(yamlText);
                    break;
                case AssetType.ShaderProgram:
                    return new DeserializerBuilder().WithTypeConverter(new ShaderProgramResolver()).Build().Deserialize<ShaderProgram>(yamlText);
                    break;
                case AssetType.Material:
                    return new DeserializerBuilder().WithTypeConverter(new MaterialResolver()).Build().Deserialize<Material>(yamlText);
                    break;
                case AssetType.Framebuffer1D:
                    break;
                case AssetType.Framebuffer2D:
                    break;
                case AssetType.Framebuffer3D:
                    break;
                case AssetType.World:
                    break;
                case AssetType.EntityPrefab:
                    break;
                case AssetType.Definition:
                    IDeserializer deserializer = new DeserializerBuilder().WithTypeConverter(new AssetDefinitionResolver()).Build();
                    AssetDefinition def = deserializer.Deserialize<AssetDefinition>(yamlText);
                    return def;
                    break;
            }

            Console.WriteLine("Return null");
            return null;
        }

        /// <summary>
        /// Gets the object yaml via object
        /// </summary>
        /// <param name="engineObject"></param>
        /// <returns></returns>
        public string GenerateObjectString(AssetType type,VexObject engineObject)
        {
            switch (type)
            {
                case AssetType.Undefined:
                    break;
                case AssetType.Texture2D:
                    return new SerializerBuilder().WithTypeConverter(new Texture2DResolver()).Build().Serialize(engineObject);
                    break;
                case AssetType.Shader:
                    return new SerializerBuilder().WithTypeConverter(new ShaderResolver()).Build().Serialize(engineObject);
                    break;
                case AssetType.ShaderProgram:
                    return new SerializerBuilder().WithTypeConverter(new ShaderProgramResolver()).Build().Serialize(engineObject);
                    break;
                case AssetType.Material:
                    return new SerializerBuilder().WithTypeConverter(new MaterialResolver()).Build().Serialize(engineObject);
                    break;
                case AssetType.Framebuffer1D:
                    break;
                case AssetType.Framebuffer2D:
                    break;
                case AssetType.Framebuffer3D:
                    break;
                case AssetType.World:
                    break;
                case AssetType.EntityPrefab:
                    break;
                case AssetType.Definition:
                    return new SerializerBuilder().WithTypeConverter(new AssetDefinitionResolver()).Build().Serialize(engineObject);
                    break;
                default:
                    break;
            }

            return "Empty Yaml";
        }

        private AssetPool m_Pool;
        private ISerializer m_Serializer;
        private IDeserializer m_Deserializer;
    }
}
