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
        public ShaderParameterMetaData(string name, int handle, ShaderParameterType type)
        {
            Name = name;
            Handle = handle;
            Type = type;
        }

        /// <summary>
        /// Parameter name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Graphics api handle
        /// </summary>
        public readonly int Handle;

        /// <summary>
        /// Type of the shader parameter
        /// </summary>
        public readonly ShaderParameterType Type;
      
    }
}
