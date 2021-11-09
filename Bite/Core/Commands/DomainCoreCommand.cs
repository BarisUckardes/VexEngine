﻿using System;
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
    public sealed class DomainCoreCommand : CoreCommand
    {
        public override void OnAttach()
        {
            /*
             * Get project startup
             */
            string domainPath = @"C:\Users\PC\Desktop\Test Domain";

            /*
             * Try find domain folder
             */
            Console.WriteLine(domainPath);
            Debug.Assert(Directory.Exists(domainPath), "Project file was not fould");

            /*
             * Create domain
             */
            Domain domain = new Domain(domainPath);

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
