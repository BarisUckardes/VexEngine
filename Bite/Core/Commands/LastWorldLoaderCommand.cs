using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Platform;

namespace Bite.Core
{
    public sealed class LastWorldLoaderCommand : CoreCommand
    {
        public override void OnAttach()
        {
            /*
             * Get last world settings path
             */
            string lastWorldPath = PlatformPaths.DomainRootDirectoy + @"\Project Settings\LastWorld.vsettings";

            /*
             * Validate file existing
             */
            if(!File.Exists(lastWorldPath))
            {
                /*
                 * Create file
                 */
                File.WriteAllText(lastWorldPath, Guid.Empty.ToString());
                return;
            }

            /*
             * Load file content
             */
            string fileContent = File.ReadAllText(lastWorldPath);

            /*
             * Get world id
             */
            Guid worldID = Guid.Parse(fileContent);

            /*
             * Validate Setup a world
             */
            if(worldID != Guid.Empty)
                EditorSession.SetupEditorWorld(worldID);
        }

        public override void OnDetach()
        {
            /*
             * Get last world settings path
             */
            string lastWorldPath = PlatformPaths.DomainRootDirectoy + @"\Project Settings\LastWorld.vsettings";

            /*
             * Get current editor root world
             */
            Guid rootWorldID = EditorSession.EditorRootWorldID;

            /*
             * Write to file
             */
            File.WriteAllText(lastWorldPath, rootWorldID.ToString());
        }
    }
}
