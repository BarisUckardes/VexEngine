using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vex.Platform;

namespace Bite.Core
{
    public sealed class LastWindowPosSizeLoader : CoreCommand
    {
        public override void OnAttach()
        {
            /*
             * Get file path
             */
            string filePath = PlatformPaths.DomainRootDirectoy + @"\Project Settings\LastWindowState.vsettings";

            /*
             * Validate file existing
             */
            if (!File.Exists(filePath))
            {
                /*
                 * Create default content
                 */
                string content = "1280 720 0 0";

                /*
                 * Write default content
                 */
                File.WriteAllText(filePath, content);
                return;
            }

            /*
             * Load file content
             */
            string fileContent = File.ReadAllText(filePath);
            string[] splitContent = fileContent.Split(" ");
            int width = int.Parse(splitContent[0]);
            int height = int.Parse(splitContent[1]);
            int offsetX = int.Parse(splitContent[2]);
            int offsetY = int.Parse(splitContent[3]);

            /*
             * Set window pos size
             */
            EditorSession.SetWindowSize(width, height);
            EditorSession.SetWindowOffset(offsetX, offsetY);
        }

        public override void OnDetach()
        {
            /*
            * Get file path
            */
            string filePath = PlatformPaths.DomainRootDirectoy + @"\Project Settings\LastWindowState.vsettings";

            /*
             * Write file content
             */
            File.WriteAllText(filePath, $"{EditorSession.WindowWidth} {EditorSession.WindowHeight} {EditorSession.WindowOffsetX} {EditorSession.WindowOffsetY}");
        }

    }
}
