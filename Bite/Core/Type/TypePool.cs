using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bite.Core
{
    public sealed class TypePool
    {
        public TypePool()
        {
            /*
             * Initialize list
             */
            m_Types = new List<Type>();

            /*
             * Gather types
             */
        }


        public Type[] Types
        {
            get
            {
                return m_Types.ToArray();
            }
        }
        public Type TargetType
        {
            get
            {
                return m_TargetType;
            }
        }

        private List<Type> GatherTypes(Type targetType)
        {
            List<Type> typesFound = new List<Type>();


            return typesFound;
        }

        private List<Type> m_Types;
        private Type m_TargetType;
    }
}
