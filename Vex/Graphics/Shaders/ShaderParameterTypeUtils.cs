using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Utility class for shader parameter type conversions
    /// </summary>
    public static class ShaderParameterTypeUtils
    {
        /// <summary>
        /// Returns the shader parameter type via string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ShaderParameterType GetTypeViaString(string name)
        {
            switch (name)
            {
                case "mat4":
                    {
                        return ShaderParameterType.Matrix4x4;
                    }
                case "sampler2D":
                    {
                        return ShaderParameterType.Texture2D;
                    }
                case "float":
                    {
                        return ShaderParameterType.Float;
                    }
                case "vec4":
                    {
                        return ShaderParameterType.Vector4;
                    }
            }
            return ShaderParameterType.Undefined;
        }
    }
}
