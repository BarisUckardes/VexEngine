using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Fang.Commands;
using System.Numerics;
using Vex.Framework;
using Vex.Types;

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
                if(info.FieldType == typeof(System.Numerics.Vector3))
                {
                    System.Numerics.Vector3 value = (System.Numerics.Vector3)info.GetValue(TargetComponent);
                    GUIRenderCommands.CreateVector3Slider(info.Name, "", ref value);
                    info.SetValue(TargetComponent, value);
                }
                else if(info.FieldType.IsSubclassOf(typeof(VexObject)))
                {
                    GUIRenderCommands.CreateText(info.FieldType.Name, "");
                    VexObject value = (VexObject)info.GetValue(TargetComponent);
                    info.SetValue(TargetComponent, GUIRenderCommands.CreateObjectField(value, value == null ? "nll_obj_" + info.GetHashCode().ToString() : value.ID.ToString()));
                }
            }

            /*
             * Render each pVexpert
             */
            foreach(PropertyInfo info in m_DefaultInfo.PropertyInfo)
            {
                if (info.PropertyType == typeof(Vector3)) // set as vector3
                {
                    Vector3 value = (Vector3)info.GetValue(TargetComponent);
                    GUIRenderCommands.CreateVector3Slider(info.Name, "", ref value,0,360);
                    info.SetValue(TargetComponent,value,null);
                    
                }
                else if(info.PropertyType == typeof(float)) // set as float
                {
                    /*
                     * Get float range if has ones
                     */
                    FloatSlider sliderAttribute = info.GetCustomAttribute<FloatSlider>();

                    /*
                     * Get current value
                     */
                    float value = (float)info.GetValue(TargetComponent);

                    /*
                     * Render slider
                     */
                    if(sliderAttribute != null)
                        GUIRenderCommands.CreateFloatSlider(info.Name, "", ref value, sliderAttribute.Min, sliderAttribute.Max);
                    else
                        GUIRenderCommands.CreateFloatSlider(info.Name, "", ref value, 0, 360);

                    /*
                     * Set value
                     */
                    info.SetValue(TargetComponent, value, null);
                }
                else if(info.PropertyType.IsSubclassOf(typeof(VexObject))) // set as vexobject variant
                {
                    GUIRenderCommands.CreateText(info.PropertyType.Name, "");
                    VexObject value = (VexObject)info.GetValue(TargetComponent);
                    info.SetValue(TargetComponent, GUIRenderCommands.CreateObjectField(value, value == null ? "nll_obj_" + info.GetHashCode().ToString() : value.ID.ToString()));
                }
            }
        }

       
        private DefaultComponentLayoutInfo m_DefaultInfo;
    }
}
