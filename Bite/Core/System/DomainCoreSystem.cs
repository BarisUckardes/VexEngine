using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Vex.Platform;
namespace Bite.Core
{
    /*
     * Template domain folder
     * PVexjectSettings
     * Code
     *     -Vex.dll
     *     -Bite.dll
     *     -UserGameCode.dll
     *     -UserEditorCode.dll
     * Domain
     *       -Folders
     *       -UserEditorCode 
     */

    public sealed class DomainCoreSystem : CoreSystem
    {
        public override void OnAttach()
        {
            /*
             * Get pVexject startup
             */
            string projectLocation = Paths.ExecutableDirectory;

            /*
             * Try find domain folder
             */
            //Debug.Assert(File.Exists(pVexjectLocation), "PVexject file was not fould");

            /*
             * Create domain
             */

        }
       

        public override void OnDetach()
        {

        }

        public override void OnUpdate()
        {

        }

        private Domain m_Domain;
    }
}
