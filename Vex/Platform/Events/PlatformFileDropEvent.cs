using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Vex.Platform
{
    /// <summary>
    /// A platform event which encapsulates the file drop properties
    /// </summary>
    public sealed class PlatformFileDropEvent : PlatformEvent
    {
        public PlatformFileDropEvent(string[] filePaths)
        {
            /*
             * Get file paths
             */
            m_FilePaths = new List<string>(filePaths);
            m_RootPaths = new List<string>(filePaths.Length);

            /*
             * Get file names&extensions
             */
            m_FileNames = new List<string>(filePaths.Length);
            m_FileExtensions = new List<string>(filePaths.Length);
            foreach(string path in m_FilePaths)
            {
                m_FileNames.Add(Path.GetFileNameWithoutExtension(path));
                m_FileExtensions.Add(Path.GetExtension(path));
                m_RootPaths.Add(Path.GetDirectoryName(path));
            }

        }

        /// <summary>
        /// Returns the file paths
        /// </summary>
        public List<string> FilePaths
        {
            get
            {
                return m_FilePaths;
            }
        }

        /// <summary>
        /// Returns the root of the file path
        /// </summary>
        public List<string> RootPaths
        {
            get
            {
                return m_RootPaths;
            }
        }

        /// <summary>
        /// Returns the file names
        /// </summary>
        public List<string> FileNames
        {
            get
            {
                return m_FileNames;
            }
        }

        /// <summary>
        /// Returns the extensions
        /// </summary>
        public List<string> FileExtensions
        {
            get
            {
                return m_FileExtensions;
            }
        }
        public override PlatformEventType Type
        {
            get
            {
                return PlatformEventType.FileDrop;
            }
        }

        public override PlatformEventCategory Category
        {
            get
            {
                return PlatformEventCategory.File;
            }
        }

        public override string AsString => throw new NotImplementedException();

        private List<string> m_FilePaths;
        private List<string> m_RootPaths;
        private List<string> m_FileNames;
        private List<string> m_FileExtensions;
    }
}
