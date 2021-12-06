﻿using Vex.Application;
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
                 * Adds a world view
                 */
                defaultWorld.AddView(typeof(WorldLogicView));

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
            m_Session = session;
        }

        /// <summary>
        /// Returns the views which this world has
        /// </summary>
        public List<WorldView> Views
        {
            get
            {
                return m_Views;
            }
        }

        /// <summary>
        /// Registers this world to the current session
        /// </summary>
        public void Register()
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
        public void AddView(Type viewType)
        {
            /*
             * Create the view
             */
            WorldView view = Activator.CreateInstance(viewType) as WorldView;

            /*
             * Initialize with world settings
             */

            /*
             * Register the view
             */
            m_Views.Add(view);
        }

        public override void Destroy()
        {
            /*
             * Get all the entities
             */
            Entity[] entities = GetView<WorldLogicView>().Entities;

            /*
             * Iterate and destory all
             */
            foreach (Entity entity in entities)
                entity.Destroy();

        }

        private List<WorldView> m_Views;
        private ApplicationSession m_Session;
    }
}
