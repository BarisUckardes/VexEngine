using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Framework
{
    public sealed class GraphicsObjectRegisterInfo
    {
        public GraphicsObjectRegisterInfo(Type objectType,OnGraphicsComponentRegister registerDelegate,OnGraphicsComponentRegister removeDelegate)
        {
            m_ObjectType = objectType;
            m_RegisterDelegate = registerDelegate;
            m_RemoveDelegate = removeDelegate;
        }

        public Type ObjectType
        {
            get
            {
                return m_ObjectType;
            }
        }

        public void Register(Component graphicsComponent)
        {
            m_RegisterDelegate.Invoke(graphicsComponent);
        }
        public void Remove(Component graphicsComponent)
        {
            m_RemoveDelegate.Invoke(graphicsComponent);
        }
        private Type m_ObjectType;
        private OnGraphicsComponentRegister m_RegisterDelegate;
        private OnGraphicsComponentRegister m_RemoveDelegate;
    }
}
