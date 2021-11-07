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
            LoadAs(m_Record.Type,m_Record.AssetPath);
        }

        /// <summary>
        /// Internal loader
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        private void LoadAs(AssetType type,string path)
        {
            /*
             * Load file text
             */
            string fileContent = File.ReadAllText(path);

            /*
             * Load file as
             */
            switch (m_Record.Type)
            {
                case AssetType.Undefined:
                    Console.WriteLine("Undefined asset type!, Asset load failed");
                    break;
                case AssetType.Texture2D:
                    AssetInterface<Texture2DResolver> assetInterface = new AssetInterface<Texture2DResolver>();
                    m_Object = assetInterface.GetObject(fileContent) as VexObject;
                    break;
                default:
                    break;
            }
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
