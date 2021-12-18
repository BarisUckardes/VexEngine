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
            m_GraphicComponents = new List<Component>();
            m_RegisterInformations = new List<List<GraphicsObjectRegisterInfo>>();
        }

        public override Type ExpectedBaseComponentType
        {
            get
            {
                return typeof(Component);
            }
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
                m_Resolvers[i].OnObserverRegistered(observer);
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
                m_Resolvers[i].OnObserverRemoved(observer);
            }
        }
        /// <summary>
        /// Registers a renderable component to the resolver
        /// </summary>
        /// <param name="renderable"></param>
        public void RegisterGraphicsObject(Component renderable)
        {
            /*
             * Register renderable
             */
            m_GraphicComponents.Add(renderable);

            /*
             * Broadcast to resolvers
             */
            for (int resolverIndex = 0; resolverIndex < m_Resolvers.Count; resolverIndex++)
            {
                /*
                 * Get resolver register informations
                 */
                List<GraphicsObjectRegisterInfo> registerInformations = m_RegisterInformations[resolverIndex];
                
                /*
                 * Iterate validate and register
                 */
                for(int registerInformationIndex = 0;registerInformationIndex<registerInformations.Count;registerInformationIndex++)
                {
                    /*
                     * Get register information
                     */
                    GraphicsObjectRegisterInfo registerInformation = registerInformations[registerInformationIndex];

                    /*
                     * Validate and register
                     */
                    if (registerInformation.ObjectType == renderable.GetType())
                        registerInformation.Register(renderable);
                }
            }
        }

        /// <summary>
        /// Removes a renderable from the resolver
        /// </summary>
        /// <param name="renderable"></param>
        public void RemoveGraphicsObject(Component renderable)
        {
            /*
             * Remove renderable
             */
            m_GraphicComponents.Remove(renderable);

            /*
             * Broadcast to resolvers
             */
            for (int resolverIndex = 0; resolverIndex < m_Resolvers.Count; resolverIndex++)
            {
                /*
                 * Get resolver register informations
                 */
                List<GraphicsObjectRegisterInfo> registerInformations = m_RegisterInformations[resolverIndex];

                /*
                 * Iterate validate and register
                 */
                for (int registerInformationIndex = 0; registerInformationIndex < registerInformations.Count; registerInformationIndex++)
                {
                    /*
                     * Get register information
                     */
                    GraphicsObjectRegisterInfo registerInformation = registerInformations[registerInformationIndex];

                    /*
                     * Validate and register
                     */
                    if (registerInformation.ObjectType == renderable.GetType())
                        registerInformation.Remove(renderable);
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
             * Get register information
             */
            List<GraphicsObjectRegisterInfo> resolverRegisterInformations = resolver.GetGraphicsComponentRegisterInformations();

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
                resolver.OnObserverRegistered(observer);
            }

            /*
             * Setup renderables
             */
            for(int graphicsObjectIndex = 0; graphicsObjectIndex < m_GraphicComponents.Count; graphicsObjectIndex++)
            {
                /*
                 * Get renderable
                 */
                Component renderable = m_GraphicComponents[graphicsObjectIndex];

                /*
                 * Broadcast
                 */
                for(int registerInformationIndex = 0; registerInformationIndex<resolverRegisterInformations.Count; registerInformationIndex++)
                {
                    /*
                     * Get register information
                     */
                    GraphicsObjectRegisterInfo registerInformation = resolverRegisterInformations[registerInformationIndex];

                    /*
                     * Validate and register
                     */
                    if(registerInformation.ObjectType == renderable.GetType())
                    {
                        registerInformation.Register(renderable);
                    }
                }
            }

            /*
             * Register graphics resolver
             */
            m_Resolvers.Add(resolver);

            /*
             * Register register informations
             */
            m_RegisterInformations.Add(resolverRegisterInformations);
        }
        public override void RemoveResolver(Type resolverType)
        {
            for (int resolverIndex = 0;resolverIndex < m_Resolvers.Count;resolverIndex++)
            {
                if(m_Resolvers[resolverIndex].GetType() == resolverType)
                {
                    /*
                     * SHOULD implement a shutdown&dispose routine
                     */
                    /*
                     * Remove resolver
                     */
                    m_Resolvers.RemoveAt(resolverIndex);

                    /*
                     * Remove register informations
                     */
                    m_RegisterInformations.RemoveAt(resolverIndex);
                    return;
                }
            }
        }

        internal override void Initialize(List<Component> components)
        {
            /*
             * Iterate component and register
             */
            foreach(Component component in components)
            {

                if (component.GetType().IsAssignableTo(typeof(ObserverComponent)))
                {
                    m_Observers.Add(component as ObserverComponent);
                }
                else if (component.GetType().IsAssignableTo(typeof(RenderableComponent)))
                {
                    m_GraphicComponents.Add(component);
                }
            }
        }

        private List<ObserverComponent> m_Observers;
        private List<Component> m_GraphicComponents;
        private List<GraphicsResolver> m_Resolvers;
        private List<List<GraphicsObjectRegisterInfo>> m_RegisterInformations;
    }
}
