using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Types;

namespace Vex.Framework
{
    internal sealed class StaticWorldContent
    {
        public StaticWorldContent(List<Tuple<string,Guid>> entityPairs,List<string> existingComponentTypes, List<Guid> assetIds,List<Tuple<int,string,Guid>> componentEntries,List<StaticComponentMetaData> componentMetaDatas)
        {
            /*
             * Initialize 
             */
            m_ComponentDatas = new List<StaticWorldComponentData>();
            m_ComponentTypeEntries = new List<Type>();
            m_ComponentEntries = new List<StaticComponentEntry>();

            /*
             * Set entities
             */
            m_EntityEntries = entityPairs;

            /*
             * Set existing types
             */
            foreach(string componentTypeName in existingComponentTypes)
            {
                /*
                 * Try get component type
                 */
                Type matchedType = EmittedComponentTypes.GetTypeViaName(componentTypeName);
                m_ComponentTypeEntries.Add(matchedType);
            }

            /*
             * Set asset ids
             */
            m_AssetEntries = assetIds;

            /*
             * Create component meta datas
             */
            foreach(Tuple<int,string,Guid> componentMetaData in componentEntries)
            {
                /*
                 * Get tuple values
                 */
                int localOwnerEntityIndex = componentMetaData.Item1;
                string componentName = componentMetaData.Item2;
                Guid componentID = componentMetaData.Item3;

                /*
                 * Create new component meta data
                 */
                m_ComponentEntries.Add(new StaticComponentEntry(localOwnerEntityIndex, componentName, componentID));
            }

            /*
             * Create component datas
             */
            foreach(StaticComponentMetaData componentMetaData in componentMetaDatas)
            {
                /*
                 * Get metadata values
                 */
                int localComponentIndex = componentMetaData.LocalComponentIndex;
                List<Tuple<string, string>> fieldEntries = componentMetaData.FieldEntries;

                /*
                 * Create required parameters
                 */
                Type targetComponentType = m_ComponentTypeEntries[localComponentIndex];

                m_ComponentDatas.Add(new StaticWorldComponentData());
            }
            
        }

   
        private readonly List<Tuple<string, Guid>> m_EntityEntries;
        private readonly List<Type> m_ComponentTypeEntries;
        private readonly List<Guid> m_AssetEntries;
        private readonly List<StaticComponentEntry> m_ComponentEntries;
        private readonly List<StaticWorldComponentData> m_ComponentDatas;


    }
}
