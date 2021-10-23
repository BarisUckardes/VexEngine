using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vex.Asset;
namespace Bite.Core
{
    public class DomainFolder
    {
        public DomainFolder(string[] files,string[] folders)
        {
            /*
             * Initialize local variables
             */
            m_SubFolders = new List<DomainFolder>();
            m_Files = new List<DomainFile>();

            /*
             * Collect files
             */
            for(int i=0;i<files.Length;i++)
            { 
                /*
                 * Create meta data
                 */
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                string extension = Path.GetExtension(files[i]);
                string folderPath = Path.GetDirectoryName(files[i]);
                string expectedAssetPath = folderPath + @"\" + fileName + ".rasset";

                /*
                * Validate if its an asset definition file
                */
                if (extension != ".rdefinition")
                {
                    continue;
                }

                /*
                 * Load definition file content
                 */
                string definitionFileContent = File.ReadAllText(files[i]);

                /*
                 * Validate source .rasset file
                 */
                DomainFileState state;
                if (File.Exists(folderPath + @"\" + fileName + extension))
                    state = DomainFileState.Valid;
                else
                    state = DomainFileState.Missing;

                /*
                 * Read domain file definition
                 */
                AssetInterface<AssetDefinitionResolver> resolver = new AssetInterface<AssetDefinitionResolver>();
                AssetDefinition definition = resolver.GetObject(definitionFileContent) as AssetDefinition;

                /*
                 * Validate if source asset needs to be resolved
                 * IF NOT only create domain file with a state
                 */
                if(state == DomainFileState.Valid)
                {
                    /*
                     * Get content
                     */
                    string assetContent = File.ReadAllText(expectedAssetPath);

                }
            }
        }


        private List<DomainFolder> m_SubFolders;
        private List<DomainFile> m_Files;
    }
}
