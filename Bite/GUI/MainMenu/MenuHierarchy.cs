using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// A hiearchy which represents a number of nodes stacked as a tree
    /// </summary>
    public sealed class MenuHierarchy
    {
        public MenuHierarchy()
        {
            /*
             * Initialize node list
             */
            m_Nodes = new List<MenuItemNode>();
        }

        /// <summary>
        /// Validates all the hierarchy recursively
        /// </summary>
        public void ValidateHiearchy()
        {
            /*
             * Iterates and validates all the nodes
             */
            foreach (MenuItemNode node in m_Nodes)
                node.Validate();
        }

        /// <summary>
        /// Renders the whole menu hierarchy
        /// </summary>
        public void RenderHierarchy()
        {
            /*
             * Iterates and renders all the nodes
             */
            foreach (MenuItemNode node in m_Nodes)
                node.RenderNode();

        }

        /// <summary>
        /// Registers anew node into the hierarchy
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="methodInfo"></param>
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

        /// <summary>
        /// Injects a splatted path to hiearchy to fit to best place
        /// </summary>
        /// <param name="targetNodes"></param>
        /// <param name="splatPath"></param>
        /// <param name="methodInfo"></param>
        /// <param name="currentIndex"></param>
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
