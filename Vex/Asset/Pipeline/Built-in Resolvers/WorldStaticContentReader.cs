using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using System.Reflection;
using Vex.Types;

namespace Vex.Asset
{
    public sealed class WorldStaticContentReader : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(StaticWorldContent);
            }
        }

        protected override object ReadAsset(IParser parser, AssetPool pool)
        {
            /*
             * Initialize variables
             */
            StaticWorldContent worldContent = null;
            List<Tuple<string, Guid>> entityEntries = new List<Tuple<string, Guid>>();
            List<string> compoentTypeEntries = new List<string>();
            List<Guid> assetEntries = new List<Guid>();
            List<Tuple<int,int, string, Guid>> componentEntries = new List<Tuple<int,int, string, Guid>>();
            List<StaticComponentMetaData> componentMetaDataEntries = new List<StaticComponentMetaData>();

            /*
             * Read entities
             */
            parser.MoveNext();
            parser.MoveNext();
            while (parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get parser string
                 */
                string parserValue = GetParserValue(parser);

                /*
                 * Read entity entry
                 */
                int firstBracket = parserValue.IndexOf("[")+1;
                int secondBracket = parserValue.IndexOf("]");

                /*
                 * Get entity name
                 */
                string entityName = parserValue.Substring(firstBracket,secondBracket-firstBracket);

                /*
                 * Get entity guid
                 */
                Guid id = Guid.Parse(parserValue.Substring(parserValue.LastIndexOf(" "),parserValue.Length - parserValue.LastIndexOf(" ")));

                /*
                 * Register entity
                 */
                entityEntries.Add(new Tuple<string, Guid>(entityName, id));

                /*
                 * Move to next entity entry
                 */
                parser.MoveNext();
            }
            
            /*
             * Move to component types
             */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Read component types
             */
            while (parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get type as string
                 */
                compoentTypeEntries.Add(GetParserValue(parser));

                /*
                 * Move to next type
                 */
                parser.MoveNext();
            }

            /*
            * Move to asset entries
            */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Read assets
             */
            while (parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get asset id
                 */
                Guid id = Guid.Parse(GetParserValue(parser));

                /*
                 * Register asset
                 */
                assetEntries.Add(id);

                /*
                 * Move to next asset entry
                 */
                parser.MoveNext();
            }

            /*
             * Move to component entries
             */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Read component entries
             */
            while(parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get parser value
                 */
                string parserValue = GetParserValue(parser);

                /*
                 * Get owner entity index
                 */
                int componentOwnerEntityIndex = int.Parse(parserValue.Substring(0,parserValue.IndexOf(" ")));

                /*
                 * Get component type index
                 */
                int componentTypeIndex = int.Parse(parserValue.Substring(parserValue.IndexOf(" ") + 1, parserValue.IndexOf(" ", parserValue.IndexOf(" ") + 1)- parserValue.IndexOf(" ")));

                /*
                 * Get component name
                 */
                string componentName = parserValue.Substring(parserValue.IndexOf("[")+1,parserValue.LastIndexOf("]") - parserValue.IndexOf("[")-1);

                /*
                 * Get component id
                 */
                Guid componentId = Guid.Parse(parserValue.Substring(parserValue.LastIndexOf(" "),parserValue.Length - parserValue.LastIndexOf(" ")));

                /*
                 * Register component entry
                 */
                componentEntries.Add(new Tuple<int,int,string, Guid>(componentOwnerEntityIndex, componentTypeIndex, componentName, componentId));

                /*
                 * Move to next component entry
                 */
                parser.MoveNext();
            }

            /*
             * Move to component meta data
             */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();

            while(GetParserValue(parser) != "End Components")
            {
                /*
                 * Move to component local index
                 */
                parser.MoveNext();

                /*
                 * Get local component index
                 */
                int componentLocalIndex = int.Parse(GetParserValue(parser));

                /*
                 * Move to component content
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Read component content
                 */
                List<Tuple<string,string, string>> fieldMetaDatas = new List<Tuple<string,string, string>>();
                while (parser.Current.GetType() != typeof(SequenceEnd))
                {
                    /*
                     * Get parser value
                     */
                    string parserValue = GetParserValue(parser);

                    /*
                     * Calculate indexes
                     */
                    int firstSpaceIndex = parserValue.IndexOf(" ");
                    int secondSpaceIndex = parserValue.IndexOf(" ", firstSpaceIndex + 1);

                    /*
                     * Get component type
                     */
                    string fieldType = parserValue.Substring(0,firstSpaceIndex);

                    /*
                     * Get field
                     */
                    string fieldName = parserValue.Substring(firstSpaceIndex+1,secondSpaceIndex-firstSpaceIndex-1);

                    /*
                     * Get fieldvalue
                     */
                    string fieldValue = parserValue.Substring(secondSpaceIndex+1,parserValue.Length-secondSpaceIndex-1);

                    /*
                     * Register component field meta
                     */
                    fieldMetaDatas.Add(new Tuple<string,string, string>(fieldType,fieldName, fieldValue));

                    /*
                     * Move to next field
                     */
                    parser.MoveNext();
                }

                /*
                 * Register component meta data
                 */
                StaticComponentMetaData componentMetaData = new StaticComponentMetaData(componentLocalIndex, fieldMetaDatas);
                componentMetaDataEntries.Add(componentMetaData);

                /*
                 * Move to next component
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();
            }

            /*
             * Move to views
             */
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();
            parser.MoveNext();

            /*
             * Iterate and read views
             */
            List<StaticViewResolverEntry> viewResolverEntries = new List<StaticViewResolverEntry>();
            while(parser.Current.GetType() != typeof(SequenceEnd))
            {
                /*
                 * Get emit view type
                 */
                string viewTypeString = GetParserValue(parser);

                /*
                 * Move to first element of the resolvers
                 */
                parser.MoveNext();
                parser.MoveNext();
                parser.MoveNext();

                /*
                 * Iterate and read resolvers
                 */
                List<string> resolverStrings = new List<string>();
                while(parser.Current.GetType() != typeof(SequenceEnd))
                {
                    resolverStrings.Add(GetParserValue(parser));
                    parser.MoveNext();
                }

                /*
                 * Register new entry
                 */
                viewResolverEntries.Add(new StaticViewResolverEntry(viewTypeString, resolverStrings));

                /*
                 * Move to next view
                 */
                parser.MoveNext();
            }

            /*
             * Create world content
             */
            worldContent = new StaticWorldContent(entityEntries, compoentTypeEntries, assetEntries, componentEntries, componentMetaDataEntries,viewResolverEntries);
            return worldContent;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
        {
            
        }
    }
}
