using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Framework;
using Vex.Types;

namespace Vex.Graphics
{
    public sealed class DeferredInstancedMesh : Component
    {
        public DeferredInstancedMesh()
        {
            m_PerInstanceModelMatrix = new List<Matrix4>();
            SetInstanceData(new List<Vector3> { new Vector3(0, 0, 0) }, new List<Vector3>() { new Vector3(0, 0, 0) }, new List<Vector3>() { new Vector3(1, 1, 1) });
        }
        public List<Matrix4> PerInstanceModelMatrices
        {
            get
            {
                return m_PerInstanceModelMatrix;
            }
        }
        public StaticMesh Mesh
        {
            get
            {
                return m_Mesh;
            }
            set
            {
                m_Mesh = value;
            }
        }

        public Texture2D Texture
        {
            get
            {
                return m_Texture;
            }
            set
            {
                m_Texture = value;
            }
        }

        public void SetInstanceData(List<Vector3> positions,List<Vector3> rotations, List<Vector3> scales)
        {
            /*
             * Cahce
             */
            m_PerInstanceModelMatrix = new System.Collections.Generic.List<Matrix4>(positions.Count);

            /*
             * Create model matrixes
             */
            for(int instanceIndex = 0;instanceIndex < positions.Count;instanceIndex++)
            {        
                m_PerInstanceModelMatrix.Add(Matrix4.CreateScale(scales[instanceIndex]) *
                        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotations[instanceIndex].X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotations[instanceIndex].Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotations[instanceIndex].Z)) *
                        Matrix4.CreateTranslation(positions[instanceIndex]));
            }
        }
        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }


        protected override void OnAttach()
        {
            base.OnAttach();
            OwnerEntity.World.GetView<WorldGraphicsView>().RegisterGraphicsObject(this);
        }
        protected override void OnDetach()
        {
            base.OnDetach();
            OwnerEntity.World.GetView<WorldGraphicsView>().RemoveGraphicsObject(this);
        }

        [ExposeThis]
        private StaticMesh m_Mesh;
        [ExposeThis]
        private Texture2D m_Texture;
        private List<Matrix4> m_PerInstanceModelMatrix;
    }
}
