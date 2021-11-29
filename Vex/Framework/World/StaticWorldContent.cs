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
    public sealed class StaticWorldContent : AssetObject
    {
        public StaticWorldContent()
        {

        }
        public StaticWorldContent(List<Tuple<string,Guid>> entityPairs,List<string> existingComponentTypes, List<Guid> assetIds,List<Tuple<int,int,string,Guid>> componentEntries,List<StaticComponentMetaData> componentMetaDatas)
        {
            Console.WriteLine("Creating static world content...");
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
            foreach(Tuple<string, Guid> entityEntry in entityPairs)
            {
                Console.WriteLine($"    Entity: [{entityEntry.Item1}]:[{entityEntry.Item2.ToString()}]");
            }

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
                Console.WriteLine($"    Component Type: [{matchedType.ToString()}]");
            }

            /*
             * Set asset ids
             */
            m_AssetEntries = assetIds;
            foreach(Guid assetId in assetIds)
            {
                Console.WriteLine($"    Asset ID: [{assetId.ToString()}]");
            }

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
                Console.WriteLine($"    Component Entry: [{localOwnerEntityIndex}][{localTypeIndex}] [{componentName}] [{componentID.ToString()}]");
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
                Console.WriteLine("Component index: " + localComponentIndex);
                Console.WriteLine("Type id : " + m_ComponentEntries[localComponentIndex].LocalTypeID);
                Type targetComponentType = m_ComponentTypeEntries[m_ComponentEntries[localComponentIndex].LocalTypeID];

                /*
                 * Register component data
                 */
                m_ComponentDatas.Add(new StaticWorldComponentData(localComponentIndex,targetComponentType,fieldEntries));
                Console.WriteLine($"    Component Index: [{localComponentIndex}] [{targetComponentType.Name}]");
            }
            Console.WriteLine("Creating static world content... DONE");
        }

        public int EntityCount
        {
            get
            {
                return m_EntityEntries.Count;
            }
        }
        public int ComponentTypeCount
        {
            get
            {
                return m_ComponentTypeEntries.Count;
            }
        }
        public int AssetCount
        {
            get
            {
                return m_AssetEntries.Count;
            }
        }
        public int ComponentCount
        {
            get
            {
                return m_ComponentEntries.Count;
            }
        }
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
                 * Create component
                 */
                Component component = ownerEntity.AddComponent(m_ComponentTypeEntries[componentEntry.LocalTypeID]);
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
                 * Get Fields
                 */
                List<StaticComponentField> fields = componentData.Fields;

                /*
                 * Iterate and set fields
                 */
                foreach(StaticComponentField field in fields)
                {
                    /*
                     * Validate and set fields
                     */
                    switch (field.FieldType)
                    {
                        case StaticComponentFieldType.Invalid:
                            break;
                        case StaticComponentFieldType.Raw:
                           // componentData.ComponentType.GetField(field.ExpectedFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(targetComponent, field.FieldDataString);
                            break;
                        case StaticComponentFieldType.Component:
                            //AssetObject asset = assets[int.Parse(field.FieldDataString)];
                            //componentData.ComponentType.GetField(field.ExpectedFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(targetComponent, asset);
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
                             * Get component via map
                             */
                            Component component = components[int.Parse(field.FieldDataString)];

                            /*
                             * Set field data
                             */
                            componentData.ComponentType.GetField(field.ExpectedFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(targetComponent,assets[assetIndex]);
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
