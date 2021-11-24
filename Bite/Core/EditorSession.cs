﻿using System;
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

        /// <summary>
        /// Returns the loaded worlds vex session has
        /// </summary>
        public IReadOnlyCollection<World> Worlds
        {
            get
            {
                return m_ApplicationSession.Worlds;
            }
        }

        /// <summary>
        /// Returns the window title
        /// </summary>
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

        /// <summary>
        /// Returns the domain file views this editor has
        /// </summary>
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

        /// <summary>
        /// Returns the project file content
        /// </summary>
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

        public void RenameAsset(in Guid id,string name)
        {
            m_ApplicationSession.AssetPool.RenameAsset(id, name);
        }
        public void RenameAssetPaths(in Guid id,string oldRoot,string newRoot)
        {
            m_ApplicationSession.AssetPool.RenameAssetPaths(id, oldRoot, newRoot);
        }
        /// <summary>
        /// Updates the asset content of a domain asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asset"></param>
        public void UpdateDomainAsset(in Guid id,VexObject asset)
        {
            /*
             * Try find asset record
             */
            Asset foundAsset = null;
            bool assetFound = m_ApplicationSession.AssetPool.FindAsset(id, out foundAsset);

            /*
             * Validate found result
             */
            if(!assetFound)
            {
                Console.WriteLine("No asset found to update");
                return;
            }

            /*
             * Update asset path
             */
            foundAsset.UpdateAssetContentOnPath(asset,m_ApplicationSession.AssetPool);
        }

      
        /// <summary>
        /// Creates a new shader asset in the domain
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="stage"></param>
        /// <param name="source"></param>
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
            folder.CreateNewFile(folder,fileName, definitionPath,assetPath,definition);
        }

        /// <summary>
        /// Creates a new shader program asset in the domain
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="category"></param>
        /// <param name="categoryName"></param>
        public void CreateShaderProgramDomainContent(DomainFolderView folder,string fileName,string category,string categoryName)
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
            folder.CreateNewFile(folder,fileName, definitionPath,assetPath,definition);
        }

        /// <summary>
        /// Creates anew material domain content
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        public void CreateMaterialDomianContent(DomainFolderView folder,string fileName)
        {
            /*
             * Create new material
             */
            Material material = new Material();
            material.Name = fileName;

            /*
             * Create physical material file
             */
            string definitionPath = folder.FolderPath + @"\" + fileName + @".vdefinition";
            string assetPath = folder.FolderPath + @"\" + fileName + @".vasset";
            AssetDefinition definition = m_ApplicationSession.AssetPool.CreateAssetOnPath(definitionPath, assetPath, AssetType.Material, material);

            /*
             * Register new content to domain foler view
             */
            folder.CreateNewFile(folder,fileName, definitionPath, assetPath, definition);
        }

        public void CreateTexture2DomainContent(DomainFolderView folder,string fileName,string filePath)
        {
            /*
             * Create new texture2D
             */
            Texture2D texture2D = Texture2D.LoadTextureFromPath(filePath);
            texture2D.Name = fileName;

            /*
             * Create physical texture file
             */
            string definitionPath = folder.FolderPath + @"\" + fileName + @".vdefinition";
            string assetPath = folder.FolderPath + @"\" + fileName + @".vasset";
            AssetDefinition definition = m_ApplicationSession.AssetPool.CreateAssetOnPath(definitionPath, assetPath, AssetType.Texture2D, texture2D);

            /*
             * Register new content to domain folder view
             */
            folder.CreateNewFile(folder,fileName, definitionPath, assetPath, definition);
        }

        public void CreateStaticMeshDomainContent(DomainFolderView folder, string fileName, string filePath)
        {
            /*
             * Create new texture2D
             */
            StaticMesh mesh = StaticMesh.LoadViaPath(filePath);
            mesh.Name = fileName;

            /*
             * Create physical texture file
             */
            string definitionPath = folder.FolderPath + @"\" + fileName + @".vdefinition";
            string assetPath = folder.FolderPath + @"\" + fileName + @".vasset";
            AssetDefinition definition = m_ApplicationSession.AssetPool.CreateAssetOnPath(definitionPath, assetPath, AssetType.Mesh, mesh);

            /*
             * Register new content to domain folder view
             */
            folder.CreateNewFile(folder,fileName, definitionPath, assetPath, definition);
        }

        /// <summary>
        /// Requests a load or get a specific asset via its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VexObject GetOrLoadAsset(in Guid id)
        {
            return m_ApplicationSession.AssetPool.GetOrLoadAsset(id);
        }
     
        /// <summary>
        /// Gets a already loaded editor resource
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public VexObject GetEditorResource(string name,AssetType type)
        {
            /*
             * Iterate each resource and validate type
             */
            for (int resourceIndex =0;resourceIndex < m_Resources.Count;resourceIndex++)
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

        /// <summary>
        /// Sets all the loaded editor resources
        /// </summary>
        /// <param name="resources"></param>
        internal void SetEditorResources(List<EditorResource> resources)
        {
            m_Resources = resources;
        }

        /// <summary>
        /// Send a shutdown request to vex session through the editor session
        /// </summary>
        internal void ShutdownRequest(string exitReason)
        {
            m_ApplicationSession.SetShutdownRequest(exitReason);
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
