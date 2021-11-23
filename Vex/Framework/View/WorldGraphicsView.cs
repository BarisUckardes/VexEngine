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
        }


        /// <summary>
        /// Returns the graphics resolvers of this view
        /// </summary>
        public GraphicsResolver[] Resolvers
        {
            get
            {
                return m_Resolvers.ToArray();
            }
        }

        /// <summary>
        /// Registers an observer to the resolvers
        /// </summary>
        /// <param name="observer"></param>
        public void RegisteVexbserver(ObserverComponent observer)
        {
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
            for (int i = 0; i < m_Resolvers.Count; i++)
            {
                if (observer.GetType() == m_Resolvers[i].ExpectedObserverType)
                {
                    m_Resolvers[i].OnObserverRemoved(observer);
                }
            }
        }

        public void RegisterRenderable(RenderableComponent renderable)
        {
            for (int i = 0; i < m_Resolvers.Count; i++)
            {
                if (renderable.GetType() == m_Resolvers[i].ExpectedRenderableType)
                {
                    m_Resolvers[i].OnRenderableRegistered(renderable);
                }
            }
        }
        public void RemoveRenderable(RenderableComponent renderable)
        {
            for (int i = 0; i < m_Resolvers.Count; i++)
            {
                if (renderable.GetType() == m_Resolvers[i].ExpectedRenderableType)
                {
                    m_Resolvers[i].OnRenderableRemoved(renderable);
                }
            }
        }

        /// <summary>
        /// Registers a graphics resolver to this view
        /// </summary>
        /// <typeparam name="TResolver"></typeparam>
        public void RegisterResolver<TResolver>() where TResolver : GraphicsResolver,new()
        {
            m_Resolvers.Add(new TResolver());
        }

        public override void InitializeWithWorldSettings(in WorldSettings settings)
        {
            /*
             * Register each resolver via world settings
             */
            for(int resolverIndex = 0;resolverIndex < settings.GraphicsResolvers.Count;resolverIndex++)
            {
                m_Resolvers.Add(Activator.CreateInstance(settings.GraphicsResolvers[resolverIndex]) as GraphicsResolver);
            }
        }
        private List<GraphicsResolver> m_Resolvers;
    }
}
