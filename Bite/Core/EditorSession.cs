using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Application;
using Vex.Asset;
using Vex.Framework;
using Vex.Graphics;

namespace Bite.Core
{
    public sealed class EditorSession
    {
        public EditorSession(ApplicationSession applicationSession)
        {
            m_ApplicationSession = applicationSession;
            m_Resources = new List<EditorResource>();
        }

        public IReadOnlyCollection<World> Worlds
        {
            get
            {
                return m_ApplicationSession.Worlds;
            }
        }
        public string ApplicationWindowTitle
        {
            get
            {
                return m_ApplicationSession.WindowTitle;
            }
            set
            {
                m_ApplicationSession.WindowTitle = value;
            }
        }

        public DomainView FileDomain
        {
            get
            {
                return m_Domain;
            }
            internal set
            {
                m_Domain = value;
            }
        }

        public ProjectFileContent ProjectFile
        {
            get
            {
                return m_ProjectFile;
            }
            internal set
            {
                m_ProjectFile = value;
            }
        }
        public void UpdateDomainAsset(Guid id,VexObject asset)
        {
            /*
             * Try find asset record
             */
            AssetRecord record;
            bool recordFound = m_ApplicationSession.AssetPool.FindRecord(id, out record);

            /*
             * Validate found result
             */
            if(!recordFound)
            {
                Console.WriteLine("No asset found to update");
                return;
            }

            /*
             * Update asset path
             */
            m_ApplicationSession.AssetPool.UpdateAssetOnPath(record.AssetPath,record.Type, asset);
        }
        public void CreateShaderDomainContent(DomainFolderView folder,string fileName,ShaderStage stage,string source)
        {
            /*
             * Create new shader
             */
            Shader shader = new Shader(stage);
            shader.Name = fileName;
            shader.Compile(source);

            /*
             * Create physical shader file
             */
            string definitionPath = folder.FolderPath + @"\" + fileName + @".vdefinition";
            string assetPath = folder.FolderPath + @"\" + fileName + @".vasset";
            AssetDefinition definition = m_ApplicationSession.AssetPool.CreateAssetOnPath(definitionPath,assetPath,AssetType.Shader,shader);

            /*
             * Register new content to domain folder view
             */
            folder.CreateNewFile(fileName, definitionPath,assetPath,definition);
        }
        public void CreateShaderProgramContent(DomainFolderView folder,string fileName,string category,string categoryName)
        {
            /*
             * Create new shader program
             */
            ShaderProgram program = new ShaderProgram(category, categoryName);
            program.Name = fileName;

            /*
             * Create physical shader program file
             */
             string definitionPath = folder.FolderPath + @"\" + fileName + @".vdefinition";
            string assetPath = folder.FolderPath + @"\" + fileName + @".vasset";
            AssetDefinition definition = m_ApplicationSession.AssetPool.CreateAssetOnPath(definitionPath,assetPath,AssetType.ShaderProgram,program);

            /*
             * Register new content to domain folder view
             */
            folder.CreateNewFile(fileName, definitionPath,assetPath,definition);
        }

        public VexObject GetOrLoadAsset(Guid id)
        {
            return m_ApplicationSession.AssetPool.GetOrLoadAsset(id);
        }
        public VexObject GetEditorResource(string name,AssetType type)
        {
            /*
             * Iterate each resource and validate type
             */
            for(int resourceIndex =0;resourceIndex < m_Resources.Count;resourceIndex++)
            {
                /*
                 * Get resource
                 */
                EditorResource resource = m_Resources[resourceIndex];

                /*
                 * Validate type
                 */
                if(resource.Type == type)
                {
                    /*
                     * Validate name
                     */
                    if (name == resource.Name)
                        return resource.Resource;
                }
            }

            return null;
        }

        internal void SetEditorResources(List<EditorResource> resources)
        {
            m_Resources = resources;
        }
        internal void ShutdownRequest()
        {
            m_ApplicationSession.HasShutdownRequest = true;
        }
        internal void Shutdown()
        {

        }

        private List<EditorResource> m_Resources;
        private ApplicationSession m_ApplicationSession;
        private DomainView m_Domain;
        private ProjectFileContent m_ProjectFile;
    }
}
