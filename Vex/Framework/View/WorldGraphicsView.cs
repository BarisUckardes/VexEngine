using Vex.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// The graphics view of a world
    /// <para>Redirects the rendering actions to graphics resolvers </para>
    /// </summary>
    public class WorldGraphicsView : WorldView
    {
        public WorldGraphicsView()
        {
            m_Resolvers = new List<GraphicsResolver>();
            m_Observers = new List<ObserverComponent>();
            m_Renderables = new List<RenderableComponent>();
        }

        /// <summary>
        /// Registers an observer to the resolvers
        /// </summary>
        /// <param name="observer"></param>
        public void RegisterObserver(ObserverComponent observer)
        {
            /*
             * Register observer
             */
            m_Observers.Add(observer);

            /*
             * Broadcast to resolvers
             */
            for(int i=0;i<m_Resolvers.Count;i++)
            {
                if(observer.GetType() == m_Resolvers[i].ExpectedObserverType)
                {
                    m_Resolvers[i].OnObserverRegistered(observer);
                }
            }
        }

        /// <summary>
        /// Removes an observer fVexm the resolvers
        /// </summary>
        /// <param name="observer"></param>
        public void RemoveObserver(ObserverComponent observer)
        {
            /*
             * Remove observer
             */
            m_Observers.Remove(observer);

            /*
             * Broadcast to resolvers
             */
            for (int i = 0; i < m_Resolvers.Count; i++)
            {
                if (observer.GetType() == m_Resolvers[i].ExpectedObserverType)
                {
                    m_Resolvers[i].OnObserverRemoved(observer);
                }
            }
        }
        /// <summary>
        /// Registers a renderable component to the resolver
        /// </summary>
        /// <param name="renderable"></param>
        public void RegisterRenderable(RenderableComponent renderable)
        {
            /*
             * Register renderable
             */
            m_Renderables.Add(renderable);

            /*
             * Broadcast to resolvers
             */
            for (int i = 0; i < m_Resolvers.Count; i++)
            {
                if (renderable.GetType() == m_Resolvers[i].ExpectedRenderableType)
                {
                    m_Resolvers[i].OnRenderableRegistered(renderable);
                }
            }
        }

        /// <summary>
        /// Removes a renderable from the resolver
        /// </summary>
        /// <param name="renderable"></param>
        public void RemoveRenderable(RenderableComponent renderable)
        {
            /*
             * Remove renderable
             */
            m_Renderables.Remove(renderable);

            /*
             * Broadcast to resolvers
             */
            for (int i = 0; i < m_Resolvers.Count; i++)
            {
                if (renderable.GetType() == m_Resolvers[i].ExpectedRenderableType)
                {
                    m_Resolvers[i].OnRenderableRemoved(renderable);
                }
            }
        }

        public override List<IWorldResolver> Resolvers
        {
            get
            {
                List<IWorldResolver> resolvers = new List<IWorldResolver>();
                foreach (GraphicsResolver resolver in m_Resolvers)
                    resolvers.Add(resolver);

                return resolvers;
            }
        }

        public override void RegisterResolver(Type resolverType)
        {
            /*
             * Create resolver with default constructor
             */
            GraphicsResolver resolver = Activator.CreateInstance(resolverType) as GraphicsResolver;

            /*
             * Setup observers
             */
            for(int observerIndex = 0;observerIndex< m_Observers.Count;observerIndex++)
            {
                /*
                 * Get observer
                 */
                ObserverComponent observer = m_Observers[observerIndex];

                /*
                 * Broadcast
                 */
                if(resolver.ExpectedObserverType == observer.GetType())
                    resolver.OnObserverRegistered(observer);
            }

            /*
             * Setup renderables
             */
            for(int renderableIndex = 0;renderableIndex < m_Renderables.Count;renderableIndex++)
            {
                /*
                 * Get renderable
                 */
                RenderableComponent renderable = m_Renderables[renderableIndex];

                /*
                 * Broadcast
                 */
                if(resolver.ExpectedRenderableType == renderable.GetType())
                    resolver.OnRenderableRegistered(renderable);   
            }

            /*
             * Register graphics resolver
             */
            m_Resolvers.Add(resolver);
        }
        public override void RemoveResolver(Type resolverType)
        {
            for (int resolverIndex = 0;resolverIndex < m_Resolvers.Count;resolverIndex++)
            {
                if(m_Resolvers[resolverIndex].GetType() == resolverType)
                {
                    /*
                     * Remove 
                     */
                    m_Resolvers.RemoveAt(resolverIndex);
                    return;
                }
            }
        }


      
        private List<ObserverComponent> m_Observers;
        private List<RenderableComponent> m_Renderables;
        private List<GraphicsResolver> m_Resolvers;
    }
}
