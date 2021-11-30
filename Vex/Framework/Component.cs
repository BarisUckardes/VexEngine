using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Types;
namespace Vex.Framework
{
    /// <summary>
    /// Component is an element which can be attached to an entity
    /// <para>Components can represent data and logic or both</para>
    /// </summary>
    public abstract class Component : VexObject
    {
        /// <summary>
        /// Retunrs the entity which this component attached to
        /// </summary>
        [DontExposeThis]
        public Entity OwnerEntity
        {
            get
            {
                return m_OwnerEntity;
            }
            internal set
            {
                m_OwnerEntity = value;
            }
        }

        /// <summary>
        /// Retunrs the spatial component of the owner entity
        /// </summary>
        public Spatial Spatial
        {
            get
            {
                return m_OwnerEntity.Spatial;
            }
        }

        /// <summary>
        /// Adds a component to owner entity
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public TComponent AddComponent<TComponent>() where TComponent:Component,new()
        {
            return m_OwnerEntity.AddComponent<TComponent>();
        }

        /// <summary>
        /// Gets a component fVexm the owner entity
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public TComponent GetComponent<TComponent>() where TComponent : Component
        {
            return m_OwnerEntity.GetComponent<TComponent>();
        }

        /// <summary>
        /// Finds a component fVexm the owner entity
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public bool HasComponent<TComponent>() where TComponent : Component
        {
            return m_OwnerEntity.HasComponent<TComponent>();
        }

        /// <summary>
        /// Called when attached to an entity
        /// </summary>
        internal virtual void OnAttach()
        {
            if(ShouldTick)
                OwnerEntity.World.GetView<WorldLogicView>().OnRegisterComponent(this);
        }

        /// <summary>
        /// Called when removed from an entity
        /// </summary>
        internal virtual void OnDetach()
        {
            if(ShouldTick)
                OwnerEntity.World.GetView<WorldLogicView>().OnRegisterComponent(this);
        }

        public abstract bool ShouldTick { get; }

        public virtual void OnLogicUpdate() { }
     
        public void Destroy()
        {
            OnDetach();
            IsDestroyed = true;
        }
        private Entity m_OwnerEntity;
    }
}
