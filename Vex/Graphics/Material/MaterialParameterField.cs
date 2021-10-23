using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Single material parameter field for specified generic type
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class  MaterialParameterField<TData>
    {
        public MaterialParameterField(string name, TData data,int nativeHandle)
        {
            Name = name;
            Data = data;
            NativeHandle = nativeHandle;
        }

        /// <summary>
        /// Name of the parameter
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Native api handle of the parameter
        /// </summary>
        public readonly int NativeHandle;

        /// <summary>
        /// Generic data
        /// </summary>
        public TData Data;


    }
}
