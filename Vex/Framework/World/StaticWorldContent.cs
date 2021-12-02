using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vex.Application;
using Vex.Types;

namespace Vex.Framework
{
    /// <summary>
    /// Static content which holds required minimal metadata for the creation of a world in runtime
    /// </summary>
    public sealed class StaticWorldContent : AssetObject
    {
        /// <summary>
        /// Necessary default constructor for the yaml 
        /// </summary>
        public StaticWorldContent()
        {

        }
        public StaticWorldContent(List<Tuple<string,Guid>> entityPairs,List<string> existingComponentTypes, List<Guid> assetIds,List<Tuple<int,int,string,Guid>> componentEntries,List<StaticComponentMetaData> componentMetaDatas)
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
            foreach(Tuple<int,int,string,Guid> componentMetaData in componentEntries)
            {
                /*
                 * Get tuple values
                 */
                int localOwnerEntityIndex = componentMetaData.Item1;
                int localTypeIndex = componentMetaData.Item2;
                string componentName = componentMetaData.Item3;
                Guid componentID = componentMetaData.Item4;

                /*
                 * Create new component meta data
                 */
                m_ComponentEntries.Add(new StaticComponentEntry(localOwnerEntityIndex, localTypeIndex, componentName, componentID));
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
                List<Tuple<string, string,string>> fieldEntries = componentMetaData.FieldEntries;

                /*
                 * Create required parameters
                 */
                Type targetComponentType = m_ComponentTypeEntries[m_ComponentEntries[localComponentIndex].LocalTypeID];

                /*
                 * Register component data
                 */
                m_ComponentDatas.Add(new StaticWorldComponentData(localComponentIndex,targetComponentType,fieldEntries));
            }
        }

        /// <summary>
        /// Returns the total entity count in this static world content
        /// </summary>
        public int EntityCount
        {
            get
            {
                return m_EntityEntries.Count;
            }
        }

        /// <summary>
        /// Returns the total unique component count in this static world content
        /// </summary>
        public int ComponentTypeCount
        {
            get
            {
                return m_ComponentTypeEntries.Count;
            }
        }

        /// <summary>
        /// Returns the total asset count in this static world content
        /// </summary>
        public int AssetCount
        {
            get
            {
                return m_AssetEntries.Count;
            }
        }

        /// <summary>
        /// Returns the total component count in this static world content
        /// </summary>
        public int ComponentCount
        {
            get
            {
                return m_ComponentEntries.Count;
            }
        }

        /// <summary>
        /// Creates a world out of this static content
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public World CreateFromThis(ApplicationSession session)
        {
            /*
             * Create new world
             */
            World world = new World(session,new WorldSettings(typeof(DefaultLogicResolver),new List<Type>() {typeof(ForwardGraphicsResolver)}));
            world.AddView<WorldLogicView>();
            world.AddView<WorldGraphicsView>();

            /*
             * Load assets
             */
            List<AssetObject> assets = new List<AssetObject>();
            foreach(Guid assetID in m_AssetEntries)
            {
                assets.Add(session.AssetPool.GetOrLoadAsset(assetID) as AssetObject);
            }

            /*
             * Create entities
             */
            List<Entity> entities = new List<Entity>();
            foreach(Tuple<string,Guid> entityEntry in m_EntityEntries)
            {
                Entity newEntity = new Entity(entityEntry.Item1, world);
                newEntity.ID = entityEntry.Item2;
                entities.Add(newEntity);
            }

            /*
             * Create and add components
             */
            List<Component> components = new List<Component>();
            foreach(StaticComponentEntry componentEntry in m_ComponentEntries)
            {
                /*
                 * Get target component type
                 */
                Type targetComponentType = m_ComponentTypeEntries[componentEntry.LocalTypeID];

                /*
                 * Get owner entity
                 */
                Entity ownerEntity = entities[componentEntry.LocalOwnerEntityID];

                /*
                 * Validate if its a spatial
                 */
                if(targetComponentType == typeof(Spatial))
                {
                    components.Add(ownerEntity.Spatial);
                    continue;
                }

                /*
                 * Validate component type
                 */
                if (targetComponentType == null)
                {
                    components.Add(null);
                    continue;
                }
                    

                /*
                 * Create component
                 */
                Component component = ownerEntity.AddComponent(targetComponentType);
                component.Name = componentEntry.ComponentName;
                component.ID = componentEntry.ComonentID;

                /*
                 * Register component
                 */
                components.Add(component);
            }

            /*
             * Fill component fields
             */
            foreach(StaticWorldComponentData componentData in m_ComponentDatas)
            {
                /*
                 * Get target component
                 */
                Component targetComponent = components[componentData.LocalComponentIndex];

                /*
                 * Validate target component
                 */
                if (targetComponent == null)
                    continue;

                /*
                 * Get Fields
                 */
                List<StaticComponentField> fields = componentData.Fields;

                /*
                 * Iterate and set fields
                 */
                foreach(StaticComponentField field in fields)
                {
                    /*
                     * Get field info
                     */
                    FieldInfo fieldInfo = TypeUtils.GetField(targetComponent.GetType(), field.ExpectedFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    /*
                     * Validate and set fields
                     */
                    switch (field.FieldType)
                    {
                        case StaticComponentFieldType.Invalid:
                            break;
                        case StaticComponentFieldType.Raw:
                            TypeUtils.SetDefaultFieldValue(targetComponent, fieldInfo, field.FieldDataString);
                            break;
                        case StaticComponentFieldType.Component:
                            break;
                        case StaticComponentFieldType.Asset:

                            /*
                             * Get asset index
                             */
                            int assetIndex = int.Parse(field.FieldDataString);

                            /*
                             * Validate asset index
                             */
                            if (assetIndex == -1)
                                break;

                            /*
                             * Validate if field found
                             */
                            if(fieldInfo == null)
                            {
                                break;
                            }

                            /*
                             * Set field value
                             */
                            fieldInfo.SetValue(targetComponent,assets[assetIndex]);
                            break;
                        default:
                            break;
                    }
                }
            }

            return world;
        }
        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        private List<Tuple<string, Guid>> m_EntityEntries;
        private List<Type> m_ComponentTypeEntries;
        private List<Guid> m_AssetEntries;
        private List<StaticComponentEntry> m_ComponentEntries;
        private List<StaticWorldComponentData> m_ComponentDatas;

       
    }
}
