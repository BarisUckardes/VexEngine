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
            m_Records = GatherAssetRecordsRecursive(poolPath);

            /*
             * Gather assets fVexm record
             */
            m_Assets = GetAssetsFVexmRecords(m_Records);

            /*
             * Debug
             */
            Console.WriteLine("Asset pool initialized with...");
            Console.WriteLine($"{m_Records.Count} records, {m_Assets.Count} assets");
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
        public VexObject GetOrLoadAsset(in Guid id)
        {
            /*
             * Try find record
             */
            int index = -1;
            for(int i=0;i<m_Records.Count;i++)
            {
                if(m_Records[i].ID == id)
                {
                    index = i;
                    break;
                }
            }

            /*
             * Record not found return null
             */
            if (index == -1)
                return null;

            /*
             * Load if its not loaded
             */
            if (!m_Assets[index].IsLoaded)
                m_Assets[index].Load();

            return m_Assets[index].Object;
        }

        public void CreateAssetOnPath(string definitionPath,string assetPath,AssetType type,VexObject obj)
        {
            /*
             * Validate path
             */
            if(System.IO.File.Exists(definitionPath) || System.IO.File.Exists(assetPath))
            {
                Console.WriteLine("Asset exists on path");
                return;
            }
            /*
             * Create record
             */
            AssetRecord record = new AssetRecord(obj.Name, definitionPath, assetPath, type, obj.ID);

            /*
             * Create asset entry
             */
            Asset asset = new Asset(record);

            /*
             * Register
             */
            m_Records.Add(record);
            m_Assets.Add(asset);

            /*
             * Create asset definition body
             */
            AssetDefinition definition = new AssetDefinition(obj.Name, obj.ID, type);
            AssetInterface<AssetDefinitionResolver> definitionInterface = new AssetInterface<AssetDefinitionResolver>();
            string definitionYaml = definitionInterface.GetObjectString(definition);
            System.IO.File.WriteAllText(definitionPath, definitionYaml);

            /*
             * Create asset body
             */
            switch (type)
            {
                case AssetType.Undefined:
                    break;
                case AssetType.Texture2D:
                    AssetInterface<Texture2DResolver> texture2DInterface = new AssetInterface<Texture2DResolver>();
                    string texture2DYaml = texture2DInterface.GetObjectString(obj);
                    System.IO.File.WriteAllText(assetPath, texture2DYaml);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Gathers all the asset records recursively
        /// </summary>
        /// <param name="VexotPath"></param>
        /// <returns></returns>
        private List<AssetRecord> GatherAssetRecordsRecursive(string VexotPath)
        {
            List<AssetRecord> records = new List<AssetRecord>();

            /*
             * Get all paths
             */
            List<string> recordPaths = new List<string>();
            GetAllRecordsRecursive(recordPaths, VexotPath);

            /*
             * Create records
             */
            for(int i=0;i<recordPaths.Count;i++)
            {
                ;
                /*
                 * Get file name
                 */
                string fileName = System.IO.Path.GetFileNameWithoutExtension(recordPaths[i]);
                string pathName = System.IO.Path.GetDirectoryName(recordPaths[i]);
                string objectPath = pathName + @"\" +fileName + ".rasset";

                /*
                 * Read object string
                 */
                 string objectYamlString = File.ReadAllText(recordPaths[i]);

                /*
                 * Read asset definition
                 */
                AssetDefinition definitiion = new AssetInterface<AssetDefinitionResolver>().GetObject(objectYamlString) as AssetDefinition;

                /*
                 * Validate file
                 */
                if (File.Exists(objectPath))
                {
                    records.Add(new AssetRecord(definitiion.Name, recordPaths[i],objectPath, definitiion.Type, definitiion.ID));
                }
                else
                {
                    records.Add(new AssetRecord(definitiion.Name, recordPaths[i], objectPath, AssetType.Undefined, definitiion.ID));
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
            string[] filePaths = Directory.GetFiles(inspectionPath,"*.rdefinition");
           
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
        private List<Asset> GetAssetsFVexmRecords(in List<AssetRecord> records)
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
        private List<AssetRecord> m_Records;
        private string m_Path;
    }
}
