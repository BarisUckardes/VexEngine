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
    public sealed class WorldWriter : AssetResolver
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
            return null;
        }

        protected override void WriteAsset(IEmitter emitter, object targetObject)
        {
            /*
             * Get as world
             */
            World world = targetObject as World;

            /*
             * Get entities and components
             */
            List<Entity> totalEntities = world.Entities;

            /*
             * Emit start mapping
             */
            emitter.Emit(new MappingStart(null, null, false, MappingStyle.Block));

            /*
             * Iterate each entity and write id and index
             */
            emitter.Emit(new Scalar(null, "Entities"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for (int entityIndex = 0; entityIndex < totalEntities.Count; entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                 * Validate if this entity is a debug entity
                 */
                if (entity.IsDebugOnly)
                    continue;

                /*
                 * Emit local index and and id
                 */
                emitter.Emit(new Scalar(null, $"[{entity.Name}] " + entity.ID.ToString()));
            }
            emitter.Emit(new SequenceEnd());

            /*
             * Iterate each entity and write unique component types
             */
            List<Type> totalTypes = new List<Type>();
            emitter.Emit(new Scalar(null, "Component Types"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for (int entityIndex = 0; entityIndex < totalEntities.Count; entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                * Validate if this entity is a debug entity
                */
                if (entity.IsDebugOnly)
                    continue;

                /*
                 * Get components
                 */
                List<Component> components = entity.Components;

                /*
                 * Iterate each component
                 */
                for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
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
                    if (!totalTypes.Contains(componentType)) // its a unique type
                    {
                        /*
                         * Register unique type
                         */
                        totalTypes.Add(componentType);

                        /*
                         * Emit type
                         */
                        emitter.Emit(new Scalar(null, componentType.Name));
                    }
                }
            }
            emitter.Emit(new SequenceEnd());


            /*
             * Iterate each entity and their components and collect unique assets
             */
            List<Guid> totalAssets = new List<Guid>(100);
            emitter.Emit(new Scalar(null, "Assets"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            for (int entityIndex = 0; entityIndex < totalEntities.Count; entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                * Validate if this entity is a debug entity
                */
                if (entity.IsDebugOnly)
                    continue;

                /*
                 * Get components
                 */
                List<Component> components = entity.Components;

                /*
                 * Iterate each component
                 */
                for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
                {
                    /*
                     * Get component
                     */
                    Component component = components[componentIndex];

                    /*
                     * Get field and property infos
                     */
                    FieldInfo[] fields = TypeUtils.GetAllFields(component.GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToArray();
                    PropertyInfo[] properties = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

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
                            if (targetAssetObject != null && !totalAssets.Contains(targetAssetObject.ID) && !totalAssets.Contains(targetAssetObject.ID)) //unique asset
                            {
                                /*
                                 * Register unique asset
                                 */
                                totalAssets.Add(targetAssetObject.ID);

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
            int entityEntry = 0;
            for (int entityIndex = 0; entityIndex < totalEntities.Count; entityIndex++)
            {
                /*
                * Get entity
                */
                Entity entity = totalEntities[entityIndex];

                /*
                * Validate if this entity is a debug entity
                */
                if (entity.IsDebugOnly)
                {
                    continue;
                }
                    

                /*
                 * Get components
                 */
                List<Component> components = entity.Components;

                /*
                 * Iterate each component
                 */
                for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
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
                    emitter.Emit(new Scalar(null, entityEntry + " " + totalTypes.IndexOf(component.GetType()) + " [" + component.Name + "] " + component.ID.ToString()));
                }

                /*
                 * Increment entity entry
                 */
                entityEntry++;
            }
            emitter.Emit(new SequenceEnd());

            /*
             * Iterate each entity and their components and collect their exposeable field and properties
             */
            emitter.Emit(new Scalar(null, "Begin Components"));
            emitter.Emit(new Scalar(null, ""));
            for (int entityIndex = 0; entityIndex < totalEntities.Count; entityIndex++)
            {
                /*
                 * Get entity
                 */
                Entity entity = totalEntities[entityIndex];

                /*
                * Validate if this entity is a debug entity
                */
                if (entity.IsDebugOnly)
                    continue;

                /*
                 * Get local index
                 */
                int localIndex = entityIndex;

                /*
                 * Get components
                 */
                List<Component> components = entity.Components;

                /*
                 * Iterate components
                 */
                for (int componentIndex = 0; componentIndex < components.Count; componentIndex++)
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
                    FieldInfo[] fields = TypeUtils.GetAllFields(component.GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToArray();

                    /*
                     * Iterate and emit fields
                     */
                    emitter.Emit(new Scalar(null, "Component Content"));
                    emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
                    for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
                    {
                        /*
                         * Get field
                         */
                        FieldInfo field = fields[fieldIndex];

                        /*
                         * Validate field type is exposable
                         */
                        if (field.IsPublic || field.GetCustomAttribute<ExposeThis>() != null) // validated
                        {
                            /*
                             * Validate if its a asset object or component
                             */
                            if (field.FieldType.IsSubclassOf(typeof(AssetObject))) // its a asset object
                            {
                                /*
                                 * Try get value
                                 */
                                AssetObject assetObject = field.GetValue(component) as AssetObject;

                                /*
                                 * Emit
                                 */
                                if (assetObject == null)
                                    emitter.Emit(new Scalar(null, "Asset" + " " + field.Name + " " + "-1"));
                                else
                                    emitter.Emit(new Scalar(null, "Asset" + " " + field.Name + " " + totalAssets.IndexOf(assetObject.ID)));

                            }
                            else if (field.FieldType.IsSubclassOf(typeof(Component))) // its a component
                            {
                                /*
                                 * Try get value
                                 */
                                Component targetComponent = field.GetValue(component) as Component;

                                /*
                                 * Emit
                                 */
                                if (targetComponent == null)
                                    emitter.Emit(new Scalar(null, "Component" + " " + field.Name + " " + Guid.Empty.ToString()));
                                else
                                    emitter.Emit(new Scalar(null, "Component" + " " + field.Name + " " + components.IndexOf(targetComponent)));
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
                                string rawValueString = (targetRawValue == null ? "null" : targetRawValue.ToString());

                                string entryString = "Raw" + " " + field.Name + " " + rawValueString;
                                emitter.Emit(new Scalar(null, entryString));
                                
                            }
                        }

                    }
                    emitter.Emit(new SequenceEnd());



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
             * Emit views
             */
            List<WorldView> views = world.Views;

            emitter.Emit(new Scalar(null, "Views"));
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
            foreach(WorldView view in views)
            {
                /*
                 * Emit view type
                 */
                emitter.Emit(new Scalar(null, view.GetType().Name));

                /*
                 * Get views
                 */
                List<IWorldResolver> resolvers = view.Resolvers;

                /*
                 * Emit view resolvers
                 */
                emitter.Emit(new Scalar(null, "Resolvers"));
                emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
                foreach (IWorldResolver resolver in resolvers)
                {
                    emitter.Emit(new Scalar(null, resolver.GetType().Name));
                }
                emitter.Emit(new SequenceEnd());

            }
            emitter.Emit(new SequenceEnd());

            /*
             * End mapping end
             */
            emitter.Emit(new MappingEnd());
        }
    }
}
