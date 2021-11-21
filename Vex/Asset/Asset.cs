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
            m_AssetAbsolutePath = record.AssetPath;
            m_DefinitionAbsolutePath = record.DefinitionPath;
            m_Name = record.Name;
            m_AssetID = record.ID;
            m_AssetType = record.Type;
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

        public string Name
        {
            get
            {
                return m_Name;
            }
        }
        public string AssetAbsolutePath
        {
            get
            {
                return m_AssetAbsolutePath;
            }
        }
        public string DefinitionAbsolutePath
        {
            get
            {
                return m_DefinitionAbsolutePath;
            }
        }
        public Guid AssetID
        {
            get
            {
                return m_AssetID;
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
        public void Load(AssetPool targetPool)
        {
            /*
             * Create asset interface
             */
            AssetInterface assetInterface = new AssetInterface(targetPool);

            /*
             * First try load asset yaml text
             */
            string assetDefinitionYaml = File.ReadAllText(m_DefinitionAbsolutePath);
            string assetYaml = File.ReadAllText(m_AssetAbsolutePath);

            /*
             * Create asset definition
             */
            AssetDefinition definition = assetInterface.GenerateAsset(AssetType.Definition, assetDefinitionYaml) as AssetDefinition;

            /*
             * Create Asset
             */
            VexObject asset = null;
            Console.WriteLine("Load as " + m_AssetType.ToString());
            switch (m_AssetType)
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
                    asset = assetInterface.GenerateAsset(AssetType.Material, assetYaml);
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

        public void UpdateAssetPath(string oldRoot,string newRoot)
        {
            /*
             * Set asset apth
             */
            m_AssetAbsolutePath = m_AssetAbsolutePath.Replace(oldRoot, newRoot);
            m_DefinitionAbsolutePath = m_DefinitionAbsolutePath.Replace(oldRoot, newRoot);
            Console.WriteLine("Asset path renamed: " + m_AssetAbsolutePath);
        }
        public void UpdateAssetContentOnPath(VexObject asset,AssetPool pool)
        {
            /*
             * Create asset interface
             */
            AssetInterface assetInterface = new AssetInterface(pool);

            /*
             * Create asset body
             */
            string assetYaml = assetInterface.GenerateObjectString(m_AssetType,asset);

            /*
             * Write asset 
             */
            System.IO.File.WriteAllText(m_AssetAbsolutePath, assetYaml);

            Console.WriteLine($"Asset[{m_AssetID.ToString()}] updated");
        }

        private VexObject m_Object;
        private string m_Name;
        private string m_DefinitionAbsolutePath;
        private string m_AssetAbsolutePath;
        private AssetType m_AssetType;
        private Guid m_AssetID;
    }
}
