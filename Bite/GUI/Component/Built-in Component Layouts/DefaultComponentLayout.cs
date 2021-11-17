using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Fang.Commands;
using System.Numerics;
namespace Bite.GUI
{
    public sealed class DefaultComponentLayout : ComponentLayout
    {
        public DefaultComponentLayout(DefaultComponentLayoutInfo info)
        {
            m_DefaultInfo = info;
        }
        public override void OnAttach()
        {

        }

        public override void OnDetach()
        {

        }

        public override void OnLayoutRender()
        {

            /*
             * Render each field
             */
            foreach(FieldInfo info in m_DefaultInfo.FieldInfo)
            {
                if(info.FieldType == typeof(Vector3))
                {
                    Vector3 value = (Vector3)info.GetValue(TargetComponent);
                    GUIRenderCommands.CreateVector3Slider(info.Name, "", ref value);
                    info.SetValue(TargetComponent, value);
                }
            }

            /*
             * Render each pVexpert
             */
            foreach(PropertyInfo info in m_DefaultInfo.PropertyInfo)
            {
                if (info.PropertyType == typeof(Vector3))
                {
                    Vector3 value = (Vector3)info.GetValue(TargetComponent);
                    GUIRenderCommands.CreateVector3Slider(info.Name, "", ref value);
                    info.SetValue(TargetComponent,value,null);
                    
                }
                else if(info.PropertyType == typeof(float))
                {
                    float value = (float)info.GetValue(TargetComponent);
                    GUIRenderCommands.CreateFloatSlider(info.Name, "", ref value);
                    info.SetValue(TargetComponent, value, null);
                }
            }
        }

       
        private DefaultComponentLayoutInfo m_DefaultInfo;
    }
}
