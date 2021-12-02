using Fang.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    /// <summary>
    /// Represents a single node in a menu view
    /// </summary>
    public class MenuItemNode
    {

        public MenuItemNode(string name, MethodInfo methodInfo)
        {
            m_Name = name;
            m_MethodInfo = methodInfo;
            m_SubNodes = new List<MenuItemNode>();
        }

        /// <summary>
        /// Returns the sub menu nodes
        /// </summary>
        public List<MenuItemNode> SubNodes
        {
            get
            {
                return m_SubNodes;
            }
        }

        /// <summary>
        /// Returns the title/name of this node
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// Returns the menu node type
        /// </summary>
        public MenuItemNodeType Type
        {
            get
            {
                return m_Type;
            }
        }

        /// <summary>
        /// Renders the node and sub node hieracy recursively
        /// </summary>
        public void RenderNode()
        {
            if(m_SubNodes.Count > 0)
            {
                if(GUIRenderCommands.CreateMenu(m_Name, "3"))
                {
                    foreach (MenuItemNode node in m_SubNodes)
                        node.RenderNode();

                    GUIRenderCommands.FinalizeMenu();
                }
                
            }
            else
            {
                if(GUIRenderCommands.CreateMenuItem(m_Name,"3"))
                {
                    Execute();
                }
            }
        }

        /// <summary>
        /// Creates node properties.Called after creating the menu item tree
        /// </summary>
        public void Validate()
        {
            /*
             * Validate self
             */
            if (m_SubNodes.Count > 0)
                m_Type = MenuItemNodeType.Menu;
            else
                m_Type = MenuItemNodeType.Item;

            /*
             * Validate sub nodes
             */
            foreach (MenuItemNode node in m_SubNodes)
                node.Validate();
        }


        /// <summary>
        /// Executes the target function
        /// </summary>
        protected void Execute()
        {
            m_MethodInfo.Invoke(null, null);
        }

     

        private List<MenuItemNode> m_SubNodes;
        private MenuItemNodeType m_Type;
        private readonly string m_Name;
        private readonly MethodInfo m_MethodInfo;
        

     
    }
}
