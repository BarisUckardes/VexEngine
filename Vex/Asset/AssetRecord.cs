using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Asset
{
    /// <summary>
    /// Primitive for storing meta data about the asset
    /// </summary>
    public readonly struct AssetRecord
    {

        public AssetRecord(string name, string definitionPath,string assetPath, AssetType type, Guid iD)
        {
            Name = name;
            DefinitionPath = definitionPath;
            AssetPath = assetPath;
            Type = type;
            ID = iD;
        }

        /// <summary>
        /// Name of the asset
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Absolute path of the asset definition
        /// </summary>
        public readonly string DefinitionPath;

        /// <summary>
        /// Absolute path of the asset object
        /// </summary>
        public readonly string AssetPath;

        /// <summary>
        /// Type of the asset
        /// </summary>
        public readonly AssetType Type;

        /// <summary>
        /// Unique id for the asset
        /// </summary>
        public readonly Guid ID;
    }
}
