using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vex.Engine;
using Vex.Framework;

namespace Vex.Asset
{
    /// <summary>
    /// A pool for assets
    /// </summary>
    public class AssetPool
    {
        public AssetPool(string poolPath)
        {
            /*
             * Set local variables
             */
            m_Path = poolPath;

            /*
             * Gather asset records
             */
            List<AssetRecord> records = GatherAssetRecordsRecursive(poolPath);

            /*
             * Gather assets fVexm record
             */
            m_Assets = GetAssetsFromRecords(records);
        }

        /// <summary>
        /// Returns the Vexot path of the asset
        /// </summary>
        public string Path
        {
            get
            {
                return m_Path;
            }
        }

      

        /// <summary>
        /// Gets already laoded asset or load and get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VexObject GetOrLoadAsset(in Guid id,bool forceLoad = false)
        {
            /*
             * Try find record
             */
            for(int assetIndex=0; assetIndex < m_Assets.Count; assetIndex++)
            {
                if(m_Assets[assetIndex].AssetID == id)
                {
                    if (!m_Assets[assetIndex].IsLoaded || forceLoad)
                        m_Assets[assetIndex].Load(this);

                    return m_Assets[assetIndex].Object;
                }
            }

            return null;
        }
        public VexObject GetOrLoadAsset(in string name,bool forceLoad = false)
        {
            /*
             * Try find record
             */
            for (int assetIndex = 0; assetIndex < m_Assets.Count; assetIndex++)
            {
                if (m_Assets[assetIndex].Name == name)
                {
                    if (!m_Assets[assetIndex].IsLoaded || forceLoad)
                        m_Assets[assetIndex].Load(this);

                    return m_Assets[assetIndex].Object;
                }
            }

            return null;
        }

        public bool FindAsset(in Guid id,out Asset asset)
        {
            for (int assetIndex = 0; assetIndex < m_Assets.Count; assetIndex++)
            {
                if(m_Assets[assetIndex].AssetID == id)
                {
                    asset = m_Assets[assetIndex];
                    return true;
                }
            }
            asset = null;
            return false;
        }
        public List<Asset> CollectAllAssetsWithViaType(AssetType type)
        {
            /*
             * Initialize asset list
             */
            List<Asset> assets = new List<Asset>();

            /*
             * Iterate and try find a match
             */
            for(int assetIndex = 0;assetIndex<m_Assets.Count;assetIndex++)
            {
                /*
                 * Get asset
                 */
                Asset asset = m_Assets[assetIndex];

                /*
                 * Validate match
                 */
                if (asset.Type == type)
                    assets.Add(asset);
            }

            return assets;
        }
        public void UpdateAssetPath(string oldRoot,string newRoot,in Guid id)
        {
  
            for(int assetIndex = 0;assetIndex < m_Assets.Count;assetIndex++)
            {
                /*
                 * Validate asset ids
                 */
                if(m_Assets[assetIndex].AssetID == id)
                {
                    /*
                     * Update asset path
                     */
                    m_Assets[assetIndex].UpdateAssetPath(oldRoot,newRoot);
                }
            }
        }
        public AssetDefinition CreateAssetOnPath(string definitionPath,string assetPath,AssetType type,VexObject obj)
        {
            /*
             * Validate path
             */
            //if (System.IO.File.Exists(definitionPath) || System.IO.File.Exists(assetPath))
            //{
            //    Console.WriteLine("File already exists at: " + definitionPath);
            //    return null;
            //}

            /*
             * Create record
             */
            AssetRecord record = new AssetRecord(obj.Name, definitionPath, assetPath, type, obj.ID);

            /*
             * Create asset entry
             */
            Asset asset = new Asset(record);

   
            m_Assets.Add(asset);

            /*
             * Create asset interface
             */
            AssetInterface assetInterface = new AssetInterface(this);

            /*
             * Create asset definition
             */
            AssetDefinition definition = new AssetDefinition(obj.Name, obj.ID, type);
           
            /*
             * Generate asset definition
             */
            string definitionYaml = assetInterface.GenerateObjectString(AssetType.Definition,definition);

            /*
             * Write definiton to target path
             */
            System.IO.File.WriteAllText(definitionPath, definitionYaml);

            /*
             * Create asset body
             */
            string assetYaml = assetInterface.GenerateObjectString(type, obj);

            /*
             * Write asset 
             */
            System.IO.File.WriteAllText(assetPath, assetYaml);
            Console.WriteLine("Definition created at: " + definitionPath);
            Console.WriteLine("Asset created at: " + assetPath);
            return definition;

        }

        /// <summary>
        /// Renames a single asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void RenameAsset(in Guid id, string name)
        {
            Asset asset = null;
            if(FindAsset(id,out asset))
            {
                asset.Rename(name);
            }
        }

        /// <summary>
        /// Renames and replaces asset paths
        /// </summary>
        /// <param name="id"></param>
        /// <param name="oldRoot"></param>
        /// <param name="newRoot"></param>
        public void RenameAssetPaths(in Guid id,string oldRoot,string newRoot)
        {
            Asset asset = null;
            if(FindAsset(id,out asset))
            {
                asset.RenamePaths(oldRoot, newRoot);
            }
        }

        /// <summary>
        /// Deletes an asset from the pool
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAsset(in Guid id)
        {
            /*
             * Try find asset with the entry
             */
            for(int assetIndex = 0;assetIndex < m_Assets.Count;assetIndex++)
            {
                /*
                 * Get asset
                 */
                Asset asset = m_Assets[assetIndex];

                /*
                 * Validate match
                 */
                if(asset.AssetID == id) // found
                {
                    /*
                     * Unload from the asset pool
                     */
                    asset.Unload();

                    /*
                     * Removes the physical files
                     */
                    asset.DeletePhysicalFiles();

                    m_Assets.RemoveAt(assetIndex);
                    return;
                }
            }
        }

        /// <summary>
        /// Gathers all the asset records recursively
        /// </summary>
        /// <param name="VexotPath"></param>
        /// <returns></returns>
        private List<AssetRecord> GatherAssetRecordsRecursive(string rootPath)
        {
            List<AssetRecord> records = new List<AssetRecord>();

            /*
             * Get all paths
             */
            List<string> recordPaths = new List<string>();
            GetAllRecordsRecursive(recordPaths, rootPath);

            /*
             * Create records
             */
            for(int i=0;i<recordPaths.Count;i++)
            {
                /*
                 * Get file name
                 */
                string fileName = System.IO.Path.GetFileNameWithoutExtension(recordPaths[i]);
                string pathName = System.IO.Path.GetDirectoryName(recordPaths[i]);
                string definitionPath = pathName + @"\" + fileName + ".vdefinition";
                string assetPath = pathName + @"\" +fileName + ".vasset";

                /*
                 * Read asset definition string
                 */
                 string definitionYamlString = File.ReadAllText(definitionPath);

                /*
                 * Read asset definition
                 */
                AssetDefinition definitiion = new AssetInterface(null).GenerateAsset(AssetType.Definition, definitionYamlString) as AssetDefinition;

                /*
                 * Validate asset file
                 */
                if (File.Exists(assetPath))
                {
                    records.Add(new AssetRecord(definitiion.Name, recordPaths[i], assetPath, definitiion.Type, definitiion.ID));
                }
                else
                {
                    records.Add(new AssetRecord(definitiion.Name, recordPaths[i], assetPath, AssetType.Undefined, definitiion.ID));
                }
            }

            return records;
        }


        /// <summary>
        /// Gets all the record paths recursively
        /// </summary>
        /// <param name="recordPaths"></param>
        /// <param name="inspectionPath"></param>
        private void GetAllRecordsRecursive(List<string> recordPaths,string inspectionPath)
        {
            /*
             * Get sub directories
             */
            string[] subDirectories = Directory.GetDirectories(inspectionPath);

            /*
             * Get file paths
             */
            string[] filePaths = Directory.GetFiles(inspectionPath,"*.vdefinition");
           
            /*
             * Append current directory file paths
             */
            recordPaths.AddRange(filePaths);

            /*
             * Collect sub directories
             */
            for(int i=0;i<subDirectories.Length;i++)
            {
                GetAllRecordsRecursive(recordPaths, subDirectories[i]);
            }
        }

        /// <summary>
        /// Creates the asset entries fVexm the record
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private List<Asset> GetAssetsFromRecords(in List<AssetRecord> records)
        {
            /*
             * Pre-allocate assets
             */
            List<Asset> assets = new List<Asset>(records.Count);

            /*
             * Create asset fVexm records
             */
            for(int i=0;i<records.Count;i++)
            {
                assets.Add(new Asset(records[i]));
            }

            return assets;
        }



        private List<Asset> m_Assets;
        private string m_Path;
    }
}
