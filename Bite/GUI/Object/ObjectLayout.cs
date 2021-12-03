using Vex.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Bite.Core;

namespace Bite.GUI
{
    /// <summary>
    /// GUI class for object layout implementation
    /// </summary>
    public abstract class ObjectLayout
    {
        /// <summary>
        /// Called when object first appeared in object observer window
        /// </summary>
        public abstract void OnAttach();

        /// <summary>
        /// Called when object disappeared from the object observer window
        /// </summary>
        public abstract void OnDetach();

        /// <summary>
        /// Called each frame
        /// </summary>
        public abstract void OnLayoutRender();


        /// <summary>
        /// The object which this layout targets
        /// </summary>
        protected VexObject Object
        {
            get
            {
                return m_Object;
            }
         
        }


        /// <summary>
        /// The editor session which this gui layout belongs
        /// </summary>
        protected EditorSession Session
        {
            get
            {
                return m_Session;
            }
            
        }

        /// <summary>
        /// An internal setter for target object
        /// </summary>
        /// <param name="obj"></param>
        internal void SetObject(VexObject obj)
        {
            m_Object = obj;
        }

        /// <summary>
        /// An internal setter for target session
        /// </summary>
        /// <param name="session"></param>
        internal void SetSession(EditorSession session)
        {
            m_Session = session;
        }

        private VexObject m_Object;
        private EditorSession m_Session;
    }
}
