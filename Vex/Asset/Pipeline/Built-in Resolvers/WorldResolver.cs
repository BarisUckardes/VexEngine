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
    public sealed class WorldResolver : AssetResolver
    {
        public override Type ExpectedAssetType
        {
            get
            {
                return typeof(World);
            }
        }

        protected override object ReadAsset(IParser parser, AssetPool pool)
        {
            /*
             * Initialize variables
             */
            World world = null;

            return world;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
        {
            /*
             * Get as world
             */
            World world = targetObject as World;

            /*
             * Try get logic view
             */
            WorldLogicView logicView = world.GetView<WorldLogicView>();

            /*
             * Validate logic view
             */
            if (logicView == null)
                return;

            /*
             * Get entities and components
             */
            Entity[] totalEntities = logicView.Entities;

            /*
             * Emit start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Iterate each entity and write id and index
             */
            emitter.Emit(new Scalar(null, "Entities"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for(int entityIndex = 0;entityIndex<totalEntities.Length;entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                 * Emit local index and and id
                 */
                emitter.Emit(new Scalar(null,entity.ID.ToString()));
            }
            emitter.Emit(new SequenceEnd());

            /*
             * Iterate each entity and write unique component types
             */
            List<Type> totalTypes = new List<Type>();
            emitter.Emit(new Scalar(null, "Component Types"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for (int entityIndex = 0;entityIndex < totalEntities.Length;entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                 * Get components
                 */
                Component[] components = entity.Components;

                /*
                 * Iterate each component
                 */
                for(int componentIndex = 0;componentIndex < components.Length;componentIndex++)
                {
                    /*
                     * Get component
                     */
                    Component component = components[componentIndex];

                    /*
                     * Get component type
                     */
                    Type componentType = component.GetType();

                    /*
                     * Validate unique type
                     */
                    if(!totalTypes.Contains(componentType)) // its a unique type
                    {
                        /*
                         * Register unique type
                         */
                        totalTypes.Add(componentType);

                        /*
                         * Emit type
                         */
                        emitter.Emit(new Scalar(null,componentType.Name));
                    }
                }
            }
            emitter.Emit(new SequenceEnd());


            /*
             * Iterate each entity and their components and collect unique assets
             */
            List<AssetObject> totalAssets = new List<AssetObject>(1000);
            emitter.Emit(new Scalar(null, "Assets"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for (int entityIndex = 0; entityIndex < totalEntities.Length; entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                 * Get components
                 */
                Component[] components = entity.Components;

                /*
                 * Iterate each component
                 */
                for (int componentIndex = 0; componentIndex < components.Length; componentIndex++)
                {
                    /*
                     * Get component
                     */
                    Component component = components[componentIndex];

                    /*
                     * Get field and property infos
                     */
                    FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    PropertyInfo[] properties = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    Console.WriteLine($"Type {component.GetType().Name} || Found {fields.Length} fields, Found {properties.Length}");
                    /*
                     * Iterate fields to collect assets
                     */
                    for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                    {
                        /*
                         * Get field
                         */
                        FieldInfo fieldInfo = fields[fieldIndex];

                        /*
                         * Validate this field if its a sub type of the asset object
                         */
                        if (fieldInfo.FieldType.IsSubclassOf(typeof(AssetObject)) && (fieldInfo.IsPublic || fieldInfo.GetCustomAttribute<ExposeThis>() != null)) // its an asset
                        {
                            /*
                             * Get field value
                             */
                            AssetObject targetAssetObject = fieldInfo.GetValue(component) as AssetObject;

                            /*
                             * Validate uniqueness
                             */
                            if(targetAssetObject != null & !totalAssets.Contains(targetAssetObject)) //unique asset
                            {
                                /*
                                 * Register unique asset
                                 */
                                totalAssets.Add(targetAssetObject);

                                /*
                                 * Emit unique asset
                                 */
                                emitter.Emit(new Scalar(null, targetAssetObject.ID.ToString()));
                            }
                        }
                    }

                    /*
                     * Iterate properties to collect assets
                     */
                    for(int propertyIndex = 0;propertyIndex<properties.Length;propertyIndex++)
                    {
                        /*
                         * Get property
                         */
                        PropertyInfo propertyInfo = properties[propertyIndex];

                        /*
                         * Validate it has getter,setter and permitted to expose
                         */
                        MethodInfo getMethodInfo = propertyInfo.GetGetMethod();
                        MethodInfo setMethodInfo = propertyInfo.GetSetMethod();
                        if (getMethodInfo != null && setMethodInfo != null && propertyInfo.GetCustomAttribute<DontExposeThis>() == null)
                        {
                            /*
                            * Get target property object
                            */
                            AssetObject targetAssetObject = getMethodInfo.Invoke(component,null) as AssetObject;

                            /*
                             * Validate uniqueness
                             */
                            if(targetAssetObject != null && !totalAssets.Contains(targetAssetObject))
                            {
                                /*
                                 * Register unique asset
                                 */
                                totalAssets.Add(targetAssetObject);

                                /*
                                 * Emit unique asset
                                 */
                                emitter.Emit(new Scalar(null, targetAssetObject.ID.ToString()));
                            }
                        }

                    }

                }



            }
            emitter.Emit(new SequenceEnd());

            /*
             * Iterate each entity to their components
             */
            List<Component> totalComponents = new List<Component>(1000);
            emitter.Emit(new Scalar(null, "Components"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for (int entityIndex = 0;entityIndex < totalEntities.Length;entityIndex++)
            {
                /*
                * Get entity
                */
                Entity entity = totalEntities[entityIndex];

                /*
                 * Get components
                 */
                Component[] components = entity.Components;

                /*
                 * Iterate each component
                 */
                for (int componentIndex = 0; componentIndex < components.Length; componentIndex++)
                {
                    /*
                     * Get component
                     */
                    Component component = components[componentIndex];

                    /*
                     * Register to total components
                     */
                    totalComponents.Add(component);

                    /*
                     * Register component id and its local entity id
                     */
                    emitter.Emit(new Scalar(null, entityIndex + " " + component.Name + " " + component.ID.ToString()));
                }
            }
            emitter.Emit(new SequenceEnd());

            /*
             * Iterate each entity and their components and collect their exposeable field and properties
             */
            emitter.Emit(new Scalar(null, "Begin Components"));
            emitter.Emit(new Scalar(null, ""));
            for(int entityIndex = 0;entityIndex < totalEntities.Length;entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                 * Get local index
                 */
                int localIndex = entityIndex;

                /*
                 * Get components
                 */
                Component[] components = entity.Components;

                /*
                 * Iterate components
                 */
                for(int componentIndex = 0;componentIndex < components.Length;componentIndex++)
                {
                    /*
                     * Get component
                     */
                    Component component = components[componentIndex];


                    /*
                     * Emit begin component
                     */
                    emitter.Emit(new Scalar(null, "Begin Component"));
                    emitter.Emit(new Scalar(null, totalComponents.IndexOf(component).ToString()));

                    /*
                     * Get fields and properties
                     */
                    FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    PropertyInfo[] properties = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    /*
                     * Iterate and emit fields
                     */
                    emitter.Emit(new Scalar(null, "Component Content"));
                    emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
                    for (int fieldIndex = 0;fieldIndex < fields.Length;fieldIndex++)
                    {
                        /*
                         * Get field
                         */
                        FieldInfo field = fields[fieldIndex];

                        /*
                         * Validate field type is exposable
                         */
                        if(field.IsPublic | field.GetCustomAttribute<ExposeThis>() != null) // validated
                        {
                            /*
                             * Validate if its a asset object or component
                             */
                            if(field.FieldType.IsSubclassOf(typeof(AssetObject))) // its a asset object
                            {
                                /*
                                 * Try get value
                                 */
                                AssetObject assetObject = field.GetValue(component) as AssetObject;

                                /*
                                 * Emit
                                 */
                                if (assetObject == null)
                                    emitter.Emit(new Scalar(null, "A" + " " + field.Name + " " + Guid.Empty.ToString()));
                                else
                                    emitter.Emit(new Scalar(null, "A" + " " + field.Name + " " + assetObject.ID.ToString()));

                            }
                            else if(field.FieldType.IsSubclassOf(typeof(Component))) // its a component
                            {
                                /*
                                 * Try get value
                                 */
                                Component targetComponent = field.GetValue(component) as Component;

                                /*
                                 * Emit
                                 */
                                if (targetComponent == null)
                                    emitter.Emit(new Scalar(null, "C" + " " + field.Name + " " + Guid.Empty.ToString()));
                                else
                                    emitter.Emit(new Scalar(null, "C" + " " + field.Name + " " + targetComponent.ID.ToString()));
                            }
                            else // its a raw type
                            {
                                /*
                                 * Try get value
                                 */
                                object targetRawValue = field.GetValue(component) as object;

                                /*
                                 * Emit
                                 */
                                emitter.Emit(new Scalar(null, "R" + " " + field.Name + " " + targetRawValue.ToString()));
                            }
                        }

                    }
                    emitter.Emit(new SequenceEnd());

                    /*
                     * Iterate and emit properties
                     */
                    for (int propertyIndex = 0;propertyIndex < properties.Length;propertyIndex++)
                    {
                        
                    }

                    /*
                     * Emit end component
                     */
                    emitter.Emit(new Scalar(null, "End Component"));
                    emitter.Emit(new Scalar(null, ""));

                }
            }
            emitter.Emit(new Scalar(null, "End Components"));
            emitter.Emit(new Scalar(null, ""));

            /*
             * End mapping end
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
