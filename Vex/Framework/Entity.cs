using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// An entity is a most primal element of a world
    /// </summary>
    public class Entity : VexObject
    {
        public Entity(string name,World world) 
        {
            /*
             * Set properties
             */
            Name = name;
            m_OwnerWorld = world;
            m_Components = new List<Component>();
            m_Debug = false;

            /*
             * Create new spatial
             */
            Spatial spatial = new Spatial();
            spatial.OwnerEntity = this;
            m_Spatial = spatial;
            m_Components.Add(spatial);

            /*
             * Register this entity to world logic view
             */
            world.RegisterEntity(this);
        }
       
        /// <summary>
        /// Returns the world which this entity currently in
        /// </summary>
        public World World
        {
            get
            {
                return m_OwnerWorld;
            }
        }

        /// <summary>
        /// Returns the spatial component of this entity
        /// </summary>
        public Spatial Spatial
        {
            get
            {
                return m_Spatial;
            }
        }

        /// <summary>
        /// Returns the total list of components of this entity
        /// </summary>
        public List<Component> Components
        {
            get
            {
                return m_Components;
            }
        }

        /// <summary>
        /// Get set debug state of this entity
        /// </summary>
        public bool IsDebugOnly
        {
            get
            {
                return m_Debug;
            }
            set
            {
                m_Debug = value;
            }
        }

        /// <summary>
        /// Adds a new component with the specified type
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public TComponent AddComponent<TComponent>() where TComponent: Component,new()
        {
            /*
             * Create new component
             */
            TComponent component = new TComponent();

            /*
             * Set this component as its owner entity
             */
            component.OwnerEntity = this;

            /*
             * Call on attach
             */
            component.OnAttachInternal();
            
            /*
             * Add it to local registry
             */
            m_Components.Add(component);

            return component;
        }

        /// <summary>
        /// Adds anew component with the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Component AddComponent(Type type)
        {
            /*
             * Create component
             */
            Component component = Activator.CreateInstance(type) as Component;

            /*
             * Set this component as its owner entity
             */
            component.OwnerEntity = this;

            /*
             * Call on attach
             */
            component.OnAttachInternal();

            /*
             * Add it to local registry
             */
            m_Components.Add(component);

            return component;
        }

        /// <summary>
        /// Return the component with the specified typw
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public TComponent GetComponent<TComponent>() where TComponent: Component
        {
            /*
             * Iterate each component and return the one with the specified type
             */
            for (int i = 0; i < m_Components.Count; i++)
            {
                if (m_Components[i].GetType() == typeof(TComponent)) // found
                {
                    return m_Components[i] as TComponent;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns whether the component with the specified type exists in this entity
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public bool HasComponent<TComponent>() where TComponent: Component
        {
            ///Iterate each component and return true if one found with the specified type
            for (int i = 0; i < m_Components.Count; i++)
            {
                if (m_Components[i].GetType() == typeof(TComponent)) // found
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes the component with the specified type
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public bool DeleteComponent<TComponent>() where TComponent : Component
        {
            /*
             * Iterate each component and delete if found a component with the specified type
             */
            for(int i=0;i<m_Components.Count;i++)
            {
                if(m_Components[i].GetType() == typeof(TComponent)) // found
                {
                    /*
                     * Detach component
                     */
                    Component component = m_Components[i];
                    component.OnDetachInternal();

                    /*
                     * Remove the component fVexm local registry
                     */
                    m_Components.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Deletes the component with the target object specified
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public bool DeleteComponent(Component targetComponent)
        {
            /*
             * Iterate each component and delete if found a component with the specified type
             */
            for (int i = 0; i < m_Components.Count; i++)
            {
                if (m_Components[i] == targetComponent) // found
                {
                    /*
                     * Detach component
                     */
                    Component component = m_Components[i];
                    component.OnDetachInternal();

                    /*
                     * Remove the component fVexm local registry
                     */
                    m_Components.RemoveAt(i);

                    return true;
                }
            }

            return false;
        }

        public void Destroy()
        {
            /*
             * Destroy all components
             */
            foreach (Component component in m_Components)
                component.Destroy();
            m_Components.Clear();

            /*
             * DestVexy this
             */
            m_OwnerWorld.RemoveEntity(this);

            /*
             * Set this destroyed
             */
            IsDestroyed = true;
        }

        private List<Component> m_Components;
        private World m_OwnerWorld;
        private Spatial m_Spatial;
        private bool m_Debug;
       
    }
}
