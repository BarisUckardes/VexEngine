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
        public World(ApplicationSession session,in WorldSettings worldSettings)
        {
            m_Views = new List<WorldView>();
            m_Session = session;
            m_Settings = worldSettings;
        }

        /// <summary>
        /// Registers this world to the current session
        /// </summary>
        public void Register()
        {
            m_Session.RegisterWorld(this);
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
        /// Adds a world view with the specified type
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        public void AddView<TView>() where TView:WorldView,new()
        {
            /*
             * Create the view
             */
            TView view = new TView();

            /*
             * Initialize with world settings
             */
            view.InitializeWithWorldSettings(m_Settings);

            /*
             * Register the view
             */
            m_Views.Add(view);
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        private List<WorldView> m_Views;
        private ApplicationSession m_Session;
        private WorldSettings m_Settings;
    }
}
