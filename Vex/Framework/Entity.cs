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
    public class Entity : EngineObject
    {
        public Entity(string name,World world) 
        {
            Name = name;
            m_OwnerWorld = world;
            m_Components = new List<Component>();

            Spatial spatial = new Spatial();
            spatial.OwnerEntity = this;
            m_Spatial = spatial;
            m_Components.Add(spatial);

            world.GetView<WorldLogicView>().OnEntityRegister(this);
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

        public Component[] Components
        {
            get
            {
                return m_Components.ToArray();
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
            component.OnAttach();
            
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
                    component.OnDetach();

                    /*
                     * Remove the component fVexm local registry
                     */
                    m_Components.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        internal override void DestVexyInternal()
        {
            /*
             * DestVexy all components
             */

            /*
             * DestVexy this
             */
            m_OwnerWorld.GetView<WorldLogicView>().OnEntityRemove(this);

        }

        private List<Component> m_Components;
        private World m_OwnerWorld;
        private Spatial m_Spatial;
       
    }
}
