using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vex.Platform;
using Vex.Asset;
using Vex.Graphics;

namespace Bite.Core
{
    public sealed class EditorResourcesLoaderCommand : CoreCommand
    {
        
        public override void OnAttach()
        {
            List<EditorResource> resources = new List<EditorResource>();

            /*
             * Validate editor files and load them
             */
            bool validationSuccessful = true;
            for(int validationIndex = 0;validationIndex < m_ValidateList.Length; validationIndex++)
            {
                /*
                 * Get file absolute path
                 */
                string fileAbsolutePath = PlatformPaths.ProgramfilesDirectory + @"\Vex\" + m_ValidateList[validationIndex];
                string name = Path.GetFileNameWithoutExtension(fileAbsolutePath);

                /*
                 * Get file asset type
                 */
                AssetType assetType = m_AssetTypes[validationIndex];

                /*
                 * Validate
                 */
                if(!File.Exists(fileAbsolutePath))
                {
                    validationSuccessful = false;
                    break;
                }

                /*
                 * Load asset
                 */
                switch (assetType)
                {
                    case AssetType.Undefined:
                        break;
                    case AssetType.Texture2D:
                        resources.Add(new EditorResource(Texture2D.LoadTextureFromPath(fileAbsolutePath),name,assetType));
                        break;
                    case AssetType.Shader:
                        break;
                    case AssetType.ShaderProgram:
                        break;
                    case AssetType.Material:
                        break;
                    case AssetType.Framebuffer1D:
                        break;
                    case AssetType.Framebuffer2D:
                        break;
                    case AssetType.Framebuffer3D:
                        break;
                    case AssetType.World:
                        break;
                    case AssetType.EntityPrefab:
                        break;
                    case AssetType.Definition:
                        break;
                    default:
                        break;
                }

            }

            /*
             * If validation is not successful shutdown
             */
            if(!validationSuccessful)
            {
                EditorSession.SendApplicationShutdownRequest($"Editor resource are failed to load");
                return;
            }

            /*
             * Upload editor resources to the editor session
             */
            EditorSession.SetEditorResources(resources);
        }

        public override void OnDetach()
        {
            /*
             * Do nothing for now
             */
        }

        private readonly string[] m_ValidateList = new string[] 
        {
            @"Bite\Resources\FolderIcon.png",
            @"Bite\Resources\BackButtonIcon.png",
            @"Bite\Resources\ShaderFileIcon.png",
            @"Bite\Resources\ShaderProgramFileIcon.png",
            @"Bite\Resources\MaterialFileIcon.png",
            @"Bite\Resources\Texture2DFileIcon.png",
            @"Bite\Resources\ComputerPathIcon.png",
            @"Bite\Resources\MeshFileIcon.png",
            @"Bite\Resources\EntityIcon.png",
            @"Bite\Resources\WorldFileIcon.png"


        };
        private readonly AssetType[] m_AssetTypes = new AssetType[]
        {
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D,
            AssetType.Texture2D
        };
        
    }
}
