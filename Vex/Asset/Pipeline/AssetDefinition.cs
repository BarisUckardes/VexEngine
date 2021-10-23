using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Asset
{
    /// <summary>
    /// Definition for target asset
    /// 
    /// </summary>
    public class AssetDefinition
    {
        public AssetDefinition(string name, Guid iD, AssetType type)
        {
            Name = name;
            ID = iD;
            Type = type;
        }

        /// <summary>
        /// Asset name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Asset unique id
        /// </summary>
        public readonly Guid ID;

        /// <summary>
        /// Asset type
        /// </summary>
        public readonly AssetType Type;
    }
}
