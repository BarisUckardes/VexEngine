using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// A metadata class which encapsulates required field for re-creating the shader stage parameters
    /// </summary>
    public readonly struct ShaderParameterMetaData
    {
        public ShaderParameterMetaData(string name, ShaderParameterType type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Parameter name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Type of the shader parameter
        /// </summary>
        public readonly ShaderParameterType Type;
      
    }
}
