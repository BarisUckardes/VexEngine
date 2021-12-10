using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vex.Extensions;
using Vex.Framework;
using Vex.Graphics;
namespace Bite.Core
{
    /// <summary>
    /// Special type of observer for free game observering
    /// </summary>
    public sealed class FreeGameObserver : ObserverComponent
    {
        public override bool ShouldTick
        {
            get
            {
                return false;
            }
        }

        public override Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView((float)((Math.PI / 180) * 60), AspectRatio, NearPlane, FarPlane);
        }

        public override Matrix4 GetViewMatrix()
        {
            Vector3 forwardAbsolute = Spatial.Forward.GetAsOpenTK();
            return Matrix4.LookAt(new System.Numerics.Vector3(-Spatial.Position.X, Spatial.Position.Y, Spatial.Position.Z).GetAsOpenTK(), new System.Numerics.Vector3(-Spatial.Position.X, Spatial.Position.Y, Spatial.Position.Z).GetAsOpenTK() + new Vector3(-forwardAbsolute.X, forwardAbsolute.Y, forwardAbsolute.Z), new Vector3(0, 1, 0));
        }


        protected override void OnAttach()
        {
            Framebuffer = new Framebuffer2D(512, 512, TextureFormat.Rgba, TextureInternalFormat.Rgba32f);
            Framebuffer.Name = "Default Color";
        }
        protected override void OnDetach()
        {
            if (Framebuffer != null)
                Framebuffer.Destroy();
            Framebuffer = null;
        }

    }
}
