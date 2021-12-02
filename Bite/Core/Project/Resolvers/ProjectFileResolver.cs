using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Asset;
using Vex.Framework;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
namespace Bite.Core
{
    /// <summary>
    /// File resolver for project file
    /// </summary>
    public sealed class ProjectFileResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(ProjectFileContent);
            }
        }

        protected override object ReadAsset(IParser parser, AssetPool pool)
        {
            /*
             * Intiailize required variables
             */
            string projectName;
            int projectVersion;
            Guid projectID;

            /*
             * Move to name content
             */
            parser.MoveNext();

            /*
             * Read name
             */
            projectName = GetParserValue(parser);

            /*
             * Moe to version content
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Read version
             */
            projectVersion = int.Parse(GetParserValue(parser));

            /*
             * Move to project id content
             */
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Read id
             */
            projectID = Guid.Parse(GetParserValue(parser));

            /*
             * Create content
             */
            ProjectFileContent content = new ProjectFileContent(projectName,projectVersion,projectID);

            return content;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
        {
            /*
             * Get content
             */
            ProjectFileContent content = (ProjectFileContent)targetObject;

            /*
             * Start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Write project name
             */
            emitter.Emit(new Scalar(null, "Project Name"));
            emitter.Emit(new Scalar(null, content.ProjectName));

            /*
             * Write project version
             */
            emitter.Emit(new Scalar(null, "Project Version"));
            emitter.Emit(new Scalar(null, content.ProjectVersion.ToString()));

            /*
             * Write project id
             */
            emitter.Emit(new Scalar(null, "Project ID"));
            emitter.Emit(new Scalar(null, content.ProjectID.ToString()));

            /*
             * End mapping
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
