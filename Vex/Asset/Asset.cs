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
