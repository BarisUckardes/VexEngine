using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;

namespace Vex.Compute
{
    public sealed class ComputeShader : AssetObject
    {
        public ComputeShader()
        {

        }

        public string LastCompileErrorMessage
        {
            get
            {
                return m_LastCompileErrorMessage;
            }
        }

        public void Compile(string source)
        {
            /*
             * Set source
             */
            m_Source = source;

            /*
             * Compile
             */
        }
        public void Dispatch(int handle,string entryPoint,int threadX,int threadY,int threadZ)
        {
            /*
             * Start dispatch
             */

            /*
             * Wait for it to finish
             */
        }
        public void SetTexture3D()
        {

        }
        public void SetTexture2D()
        {

        }
        public void SetTexture1D()
        {

        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        private string m_Source;
        private string m_LastCompileErrorMessage;
    }
}
