using Fang.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bite.GUI
{
    public class MenuItemNode
    {

        public MenuItemNode(string name, MethodInfo methodInfo)
        {
            m_Name = name;
            m_MethodInfo = methodInfo;
            m_SubNodes = new List<MenuItemNode>();
        }
        public List<MenuItemNode> SubNodes
        {
            get
            {
                return m_SubNodes;
            }
        }
        public string Name
        {
            get
            {
                return m_Name;
            }
        }
        public MenuItemNodeType Type
        {
            get
            {
                return m_Type;
            }
        }
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
        public void Execute()
        {
            m_MethodInfo.Invoke(null, null);
        }

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

        private List<MenuItemNode> m_SubNodes;
        private MenuItemNodeType m_Type;
        private readonly string m_Name;
        private readonly MethodInfo m_MethodInfo;
        

     
    }
}
