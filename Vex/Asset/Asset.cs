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
        public AssetType Type
        {
            get
            {
                return m_AssetType;
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

        internal void Rename(string name)
        {
            /*
             * Get folder path
             */
            string folderPath = Path.GetDirectoryName(m_AssetAbsolutePath);
            string oldPath = m_AssetAbsolutePath;

            /*
             * Set absolute paths
             */
            m_AssetAbsolutePath = folderPath + @"\" + name + @".vasset";
            m_DefinitionAbsolutePath = folderPath + @"\" + name + @".vdefinition";

            /*
             * Update asset definition file
             */
            AssetInterface assetInterface = new AssetInterface(null);
            AssetDefinition newDefinition = new AssetDefinition(name, m_AssetID, m_AssetType);
            string yamlText = assetInterface.GenerateObjectString(AssetType.Definition, newDefinition);

            /*
             * Write to disk
             */
            File.WriteAllText(m_DefinitionAbsolutePath, yamlText);
        }

        internal void RenamePaths(string oldRoot,string newRoot)
        {
            /*
             * Rename self paths
             */
            m_AssetAbsolutePath = m_AssetAbsolutePath.Replace(oldRoot, newRoot);
            m_DefinitionAbsolutePath = m_DefinitionAbsolutePath.Replace(oldRoot, newRoot);
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
            AssetObject asset = null;
            switch (m_AssetType)
            {
                case AssetType.Undefined:
                    break;
                case AssetType.Texture2D:
                    asset = assetInterface.GenerateAsset(AssetType.Texture2D, assetYaml) as AssetObject;
                    break;
                case AssetType.Shader:
                    asset = assetInterface.GenerateAsset(AssetType.Shader, assetYaml) as AssetObject;
                    break;
                case AssetType.ShaderProgram:
                    asset = assetInterface.GenerateAsset(AssetType.ShaderProgram, assetYaml) as AssetObject;
                    break;
                case AssetType.Material:
                    asset = assetInterface.GenerateAsset(AssetType.Material, assetYaml) as AssetObject;
                    break;
                case AssetType.Framebuffer1D:
                    break;
                case AssetType.Framebuffer2D:
                    break;
                case AssetType.Framebuffer3D:
                    break;
                case AssetType.World:
                    asset = assetInterface.GenerateAsset(AssetType.World, assetYaml) as AssetObject;
                    break;
                case AssetType.EntityPrefab:
                    break;
                case AssetType.Definition:
                    break;
                case AssetType.Mesh:
                    asset = assetInterface.GenerateAsset(AssetType.Mesh, assetYaml) as AssetObject;
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
        }

        /// <summary>
        /// Frees asset
        /// </summary>
        public void Unload()
        {
            m_Object?.Destroy();
        }

        public void DeletePhysicalFiles()
        {
            File.Delete(m_AssetAbsolutePath);
            File.Delete(m_DefinitionAbsolutePath);
        }

        public void UpdateAssetPath(string oldRoot,string newRoot)
        {
            /*
             * Set asset apth
             */
            m_AssetAbsolutePath = m_AssetAbsolutePath.Replace(oldRoot, newRoot);
            m_DefinitionAbsolutePath = m_DefinitionAbsolutePath.Replace(oldRoot, newRoot);
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
        }

        private AssetObject m_Object;
        private string m_Name;
        private string m_DefinitionAbsolutePath;
        private string m_AssetAbsolutePath;
        private AssetType m_AssetType;
        private Guid m_AssetID;
    }
}
