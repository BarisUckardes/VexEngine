using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vex.Framework;

namespace Vex.Asset
{
    /// <summary>
    /// Represents a single asset in hdd
    /// </summary>
    public class Asset
    {
        public Asset(in AssetRecord record)
        {
            m_Record = record;
        }

        /// <summary>
        /// Return the loaded object of this asset
        /// </summary>
        public VexObject Object
        {
            get
            {
                return m_Object;
            }
        }

        /// <summary>
        /// Returns whether this asset is already loaded
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return m_Object != null;
            }
        }

        /// <summary>
        /// Loads this asset
        /// </summary>
        public void Load()
        {
            /*
             * Create asset interface
             */
            AssetInterface assetInterface = new AssetInterface(null);

            /*
             * First try load asset yaml text
             */
            string assetDefinitionYaml = File.ReadAllText(m_Record.DefinitionPath);
            string assetYaml = File.ReadAllText(m_Record.AssetPath);

            /*
             * Create asset definition
             */
            AssetDefinition definition = assetInterface.GenerateAsset(AssetType.Definition, assetDefinitionYaml) as AssetDefinition;

            /*
             * Create Asset
             */
            VexObject asset = null;
            switch (m_Record.Type)
            {
                case AssetType.Undefined:
                    break;
                case AssetType.Texture2D:
                    break;
                case AssetType.Shader:
                    asset = assetInterface.GenerateAsset(AssetType.Shader, assetYaml);
                    break;
                case AssetType.ShaderProgram:
                    asset = assetInterface.GenerateAsset(AssetType.ShaderProgram, assetYaml);
                    break;
                case AssetType.Material:
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
                    break;
                default:
                    break;
            }

            /*
             * Set asset characteristics
             */
            asset.Name = definition.Name;
            asset.ID = definition.ID;

            m_Object = asset;

            Console.WriteLine("Asset from the pool loaded as: " + asset.GetType().Name);
        }

        /// <summary>
        /// Frees asset
        /// </summary>
        public void Unload()
        {

        }

        private VexObject m_Object;
        private readonly AssetRecord m_Record;
    }
}
