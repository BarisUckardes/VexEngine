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
    /// <summary>
    /// A core command which loads and unloads editor domain view
    /// </summary>
    public sealed class DomainCoreCommand : CoreCommand
    {
        public override void OnAttach()
        {
            /*
             * Get project startup
             */
            string domainPath = PlatformPaths.DomainDirectory;

            /*
             * Try find domain folder
             */
            Debug.Assert(Directory.Exists(domainPath), "Project file was not fould");

            /*
             * Create domain
             */
            DomainView domain = new DomainView(domainPath);

            /*
             * Register domain to editor session
             */
            EditorSession.FileDomain = domain;
        }

        public override void OnDetach()
        {
            /*
             * Nullfy file domain
             */
            EditorSession.FileDomain = null;
        }

    }
}
