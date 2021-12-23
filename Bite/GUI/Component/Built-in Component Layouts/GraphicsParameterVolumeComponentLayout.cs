using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Graphics;
using Fang.Commands;
namespace Bite.GUI
{
    [ComponentLayout(typeof(GraphicsParameterVolumeComponent))]
    public sealed class GraphicsParameterVolumeComponentLayout : ComponentLayout
    {
        public override void OnAttach()
        {
            m_ParameterVolumeComponent = (TargetComponent as GraphicsParameterVolumeComponent);
        }

        public override void OnDetach()
        {
            m_ParameterVolumeComponent = null;
        }

        public override void OnLayoutRender()
        {
            
            /*
             * Render resolver information blocks
             */
            List<GraphicsResolverInformationBlock> resolverInformationBlocks = m_ParameterVolumeComponent.ObtainedInformationBlock.ResolverInformationBlocks;
            foreach (GraphicsResolverInformationBlock resolverInformationBlock in resolverInformationBlocks)
            {
                /*
                 * Render resolver header
                 */
                if (GUIRenderCommands.CreateTreeNode(resolverInformationBlock.ResolverName, ""))
                {
                    /*
                     * Get parameter groups
                     */
                    List<GraphicsResolverParameterGroup> parameterGroups = resolverInformationBlock.ParameterGroups;

                    /*
                     * Render parameter groups
                     */
                    foreach (GraphicsResolverParameterGroup parameterGroup in parameterGroups)
                    {
                        /*
                         * Render group category
                         */
                        if (GUIRenderCommands.CreateTreeNode(parameterGroup.CategoryName, ""))
                        {
                            /*
                             * Get parameters
                             */
                            List<GraphicsResolverParameter> parameters = parameterGroup.Parameters;
                            foreach (GraphicsResolverParameter parameter in parameters)
                            {
                                /*
                                 * Render resolver parameter
                                 */
                                Type expectedType = parameter.ExpectedDataType;

                                /*
                                 * Render parameter header
                                 */
                                GUIRenderCommands.CreateText(parameter.VisibleParameterName, "");

                                /*
                                 * Catch type
                                 */
                                if (expectedType == typeof(float))
                                {
                                    parameter.SetParameter<float>(GUIRenderCommands.CreateFloatSlider(" ##" + parameter.GetHashCode().ToString(), "nocode", parameter.GetParameter<float>(),0.0f,10.0f));
                                }
                                else if(expectedType.IsAssignableTo(typeof(VexObject)))
                                {
                                    parameter.SetParameter<VexObject>(GUIRenderCommands.CreateObjectField(parameter.GetParameter<VexObject>(), "m_kode"));
                                }
                                else
                                {
                                    GUIRenderCommands.CreateText($"Parameter [{expectedType.ToString()}] data type is not recognized", "");
                                }

                            }
                            GUIRenderCommands.FinalizeTreeNode();
                        }
                    }
                    GUIRenderCommands.FinalizeTreeNode();
                }
            }
        }

        private GraphicsParameterVolumeComponent m_ParameterVolumeComponent;
    }
}
