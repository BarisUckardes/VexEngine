using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Encapsualtes all the parameters for a specific shader stage
    /// </summary>
    public readonly struct ShaderStageParameters
    {
        public ShaderStageParameters(ShaderStage stage,List<ShaderParameterMetaData> parameters)
        {
            Stage = stage;
            Parameters = parameters.ToArray();
        }

        /// <summary>
        /// The stage
        /// </summary>
        public readonly ShaderStage Stage;

        /// <summary>
        /// The parameters of the stage
        /// </summary>
        public readonly ShaderParameterMetaData[] Parameters;
    }
}
