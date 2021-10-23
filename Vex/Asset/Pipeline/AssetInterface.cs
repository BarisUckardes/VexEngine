using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Vex.Asset
{
    /// <summary>
    /// An interface for asset creation
    /// </summary>
    /// <typeparam name="TAssetResolver"></typeparam>
    public class AssetInterface<TAssetResolver> where TAssetResolver:AssetResolver,new()
    {
        public AssetInterface()
        {
            m_Serializer = new SerializerBuilder().WithTypeConverter(new TAssetResolver()).Build();
            m_Deserializer = new DeserializerBuilder().WithTypeConverter(new TAssetResolver()).Build();
        }

        /// <summary>
        /// Get asset object via yamlText
        /// </summary>
        /// <param name="yamlText"></param>
        /// <returns></returns>
        public object GetObject(string yamlText)
        {
            return m_Deserializer.Deserialize(yamlText,new TAssetResolver().ExpectedAssetType);
        }

        /// <summary>
        /// Gets the object yaml via object
        /// </summary>
        /// <param name="engineObject"></param>
        /// <returns></returns>
        public string GetObjectString(in object engineObject)
        {
            return m_Serializer.Serialize(engineObject);
        }


        private ISerializer m_Serializer;
        private IDeserializer m_Deserializer;
    }
}
