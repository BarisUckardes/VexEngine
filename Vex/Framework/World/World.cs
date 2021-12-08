using Vex.Application;
using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    /// <summary>
    /// Represents single world in a session
    /// </summary>
    public sealed class World : AssetObject
    {
        /// <summary>
        /// Creates and returns a default world
        /// </summary>
        public static World DefaultWorld
        {
            get
            {
                /*
                 * Creates an empty world
                 */
                World defaultWorld = new World(null);

                /*
                 * Create a default entity
                 */
                Entity entity = new Entity("Default entity", defaultWorld);
                return defaultWorld;
            }
        }

        /// <summary>
        /// Loads world with the target id and switches to it
        /// </summary>
        /// <param name="id"></param>
        public static void LoadAndSwitch(Guid id)
        {
            /*
             * Try load static world asset content
             */
            StaticWorldContent worldContent = Session.AssetPool.GetOrLoadAsset(id) as StaticWorldContent;

            /*
             * Destroy current world
             */
            Session.CurrentWorld?.Destroy();

            /*
             * Create world out of world content
             */
            World newWorld = worldContent.CreateFromThis(Session);

            /*
             * Register world 
             */
            newWorld.Register();
        }

        public static void LoadAndSwitch(string name)
        {
            /*
             * Try laod static world asset content
             */
            StaticWorldContent worldContent = Session.AssetPool.GetOrLoadAsset(name) as StaticWorldContent;

            /*
             * Destroy the current world
             */
            Session.CurrentWorld?.Destroy();

            /*
             * Create new world out of the world content
             */
            World newWorld = worldContent.CreateFromThis(Session);
            newWorld.Register();

        }

        /// <summary>
        /// Loads a world using the staticworldcontent and switches to it
        /// </summary>
        /// <param name="content"></param>
        public static void LoadAndSwitch(StaticWorldContent content)
        {
            /*
            * Destroy current world
            */
            Session.CurrentWorld?.Destroy();

            /*
             * Validate content
             */
            if (content == null)
                return;

            /*
            * Create world out of world content
            */
            World newWorld = content.CreateFromThis(Session);
            newWorld.ID = content.ID;
            newWorld.Name = content.Name;

            /*
             * Register world 
             */
            newWorld.Register();
        }

        /// <summary>
        /// Internal application session for world class
        /// </summary>
        internal static ApplicationSession Session { get; set; }

        public World(ApplicationSession session)
        {
            m_Views = new List<WorldView>();
            m_Entities = new List<Entity>(100);
            m_Session = session;
        }

        /// <summary>
        /// Returns the views which this world has
        /// </summary>
        public List<WorldView> Views
        {
            get
            {
                return new List<WorldView>(m_Views);
            }
        }

        /// <summary>
        /// Returns all the world entities
        /// </summary>
        public List<Entity> Entities
        {
            get
            {
                return new List<Entity>(m_Entities);
            }
        }

        public Entity GetEntityViaID(in Guid id)
        {
            for(int entityIndex = 0;entityIndex < m_Entities.Count;entityIndex++)
            {
                if (m_Entities[entityIndex].ID == id)
                    return m_Entities[entityIndex];
            }
            return null;
        }
        /// <summary>
        /// Registers this world to the current session
        /// </summary>
        internal void Register()
        {
            m_Session.SetCurrentWorld(this);
        }

        /// <summary>
        /// Returns the world view with the specified type
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <returns></returns>
        public TView GetView<TView>() where TView:WorldView
        {
            /*
             * Iterate all views and try find a world with the specified type
             */
            for(int i=0;i<m_Views.Count;i++)
            {
                if(m_Views[i].GetType() == typeof(TView))
                {
                    return m_Views[i] as TView;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns a world view via type
        /// </summary>
        /// <param name="viewType"></param>
        /// <returns></returns>
        public WorldView GetView(Type viewType)
        {
            /*
             * Iterate all views and try find a world with the specified type
             */
            for (int i = 0; i < m_Views.Count; i++)
            {
                if (m_Views[i].GetType() == viewType)
                {
                    return m_Views[i];
                }
            }
            return null;
        }
        /// <summary>
        /// Adds a world view with the specified type
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        public void AddView(Type viewType,bool bInitialize)
        {
            /*
             * Create the view
             */
            WorldView view = Activator.CreateInstance(viewType) as WorldView;

            /*
             * Set owner world
             */
            view.SetWorld(this);

            /*
             * Try populate view
             */
            if(bInitialize)
                view.Initialize(GetAllWorldComponents());

            /*
             * Register the view
             */
            m_Views.Add(view);
        }

        public void RegisterEntity(Entity entity)
        {
            m_Entities.Add(entity);
        }
        public void RemoveEntity(Entity entity)
        {
            m_Entities.Remove(entity);
        }
        public override void Destroy()
        {
            /*
             * Iterate and destory all
             */
            for(int entityIndex = 0; entityIndex<m_Entities.Count; entityIndex++)
            {
                m_Entities[entityIndex].Destroy();
                entityIndex--;
            }
                
            /*
             * Clear entity list
             */
            m_Entities.Clear();
        }

        /// <summary>
        /// Returns all the components in the world.(Used for view initialization on runtime)
        /// </summary>
        /// <returns></returns>
        private List<Component> GetAllWorldComponents()
        {
            /*
             * Pre-cache
             */
            List<Component> components = new List<Component>(m_Entities.Count*5);

            /*
             * Iterate entityes and add components
             */
            foreach(Entity entity in m_Entities)
            {
                components.AddRange(entity.Components);
            }

            return components;
        }
        private List<WorldView> m_Views;
        private List<Entity> m_Entities;
        private ApplicationSession m_Session;
    }
}
