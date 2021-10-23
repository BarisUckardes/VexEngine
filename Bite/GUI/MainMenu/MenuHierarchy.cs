using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public sealed class MenuHierarchy
    {
        public MenuHierarchy()
        {
            /*
             * Initialize node list
             */
            m_Nodes = new List<MenuItemNode>();
        }

        public void ValidateHiearchy()
        {
            foreach (MenuItemNode node in m_Nodes)
                node.Validate();
        }
        public void RenderHierarchy()
        {
            foreach (MenuItemNode node in m_Nodes)
                node.RenderNode();

        }
        public void RegisterNode(string fullPath,MethodInfo methodInfo)
        {
            /*
             * Split path
             */
            string[] splatPath = fullPath.Split("/");

            /*
             * Add node
             */
            InjectSplatPath(m_Nodes,splatPath, methodInfo,0);

        }

        private void InjectSplatPath(List<MenuItemNode> targetNodes,string[] splatPath,MethodInfo methodInfo,int currentIndex)
        {
            /*
             * Get current path
             */
            string currentPath = splatPath[currentIndex];

            /*
             * Validate if target nodes contains path
             */
            for (int i=0;i<targetNodes.Count;i++)
            {
                if(targetNodes[i].Name == currentPath)
                {
                    InjectSplatPath(targetNodes[i].SubNodes, splatPath,methodInfo, ++currentIndex);
                    return;
                }
            }

            /*
             * Create new node
             */
            MenuItemNode node = new MenuItemNode(currentPath, methodInfo);
            targetNodes.Add(node);

            /*
             * If paths is not in the end
             */
            if(currentIndex < splatPath.Length-1)
            {
                InjectSplatPath(node.SubNodes, splatPath,methodInfo, ++currentIndex);
            }

          
        }

        private List<MenuItemNode> m_Nodes;
    }
}
