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
        public object GenerateAsset(AssetType type,string yamlText)
        {
            switch (type)
            {
                case AssetType.Undefined:
                    break;
                case AssetType.Texture2D:
                    {
                        Texture2DResolver resolver = new Texture2DResolver();
                        resolver.SetTargetPool(m_Pool);
                        return new DeserializerBuilder().WithTypeConverter(resolver).Build().Deserialize<Texture2D>(yamlText);
                    }
                case AssetType.Shader:
                    {
                        ShaderResolver resolver = new ShaderResolver();
                        resolver.SetTargetPool(m_Pool);
                        return new DeserializerBuilder().WithTypeConverter(resolver).Build().Deserialize<Shader>(yamlText);
                    }
                case AssetType.ShaderProgram:
                    {
                        ShaderProgramResolver resolver = new ShaderProgramResolver();
                        resolver.SetTargetPool(m_Pool);
                        return new DeserializerBuilder().WithTypeConverter(resolver).Build().Deserialize<ShaderProgram>(yamlText);
                    }
                case AssetType.Material:
                    {
                        MaterialResolver resolver = new MaterialResolver();
                        resolver.SetTargetPool(m_Pool);
                        return new DeserializerBuilder().WithTypeConverter(resolver).Build().Deserialize<Material>(yamlText);
                    }
                case AssetType.Framebuffer1D:
                    break;
                case AssetType.Framebuffer2D:
                    break;
                case AssetType.Framebuffer3D:
                    break;
                case AssetType.World:
                    {
                        WorldStaticContentReader resolver = new WorldStaticContentReader();
                        resolver.SetTargetPool(m_Pool);
                        return new DeserializerBuilder().WithTypeConverter(resolver).Build().Deserialize<StaticWorldContent>(yamlText);
                    }
                case AssetType.EntityPrefab:
                    break;
                case AssetType.Definition:
                    {
                        AssetDefinitionResolver resolver = new AssetDefinitionResolver();
                        resolver.SetTargetPool(m_Pool);
                        return new DeserializerBuilder().WithTypeConverter(new AssetDefinitionResolver()).Build().Deserialize<AssetDefinition>(yamlText);
                    }
                case AssetType.Mesh:
                    {
                        StaticMeshResolver resolver = new StaticMeshResolver();
                        resolver.SetTargetPool(m_Pool);
                        return new DeserializerBuilder().WithTypeConverter(resolver).Build().Deserialize<StaticMesh>(yamlText);
                    }
            }

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
                    return new SerializerBuilder().WithTypeConverter(new WorldWriter()).Build().Serialize(engineObject);
                    break;
                case AssetType.EntityPrefab:
                    break;
                case AssetType.Definition:
                    return new SerializerBuilder().WithTypeConverter(new AssetDefinitionResolver()).Build().Serialize(engineObject);
                    break;
                case AssetType.Mesh:
                    return new SerializerBuilder().WithTypeConverter(new StaticMeshResolver()).Build().Serialize(engineObject);
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
