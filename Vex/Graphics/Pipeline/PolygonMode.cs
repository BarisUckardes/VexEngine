using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Represents the which face willbe filled and with which method
    /// </summary>
    public readonly struct PolygonMode
    {
        public PolygonMode(PolygonFillFace fillFace = PolygonFillFace.FrontAndBack, PolygonFillMethod fillMethod = PolygonFillMethod.Fill)
        {
            FillFace = fillFace;
            FillMethod = fillMethod;
        }

        /// <summary>
        /// The face which will be filled
        /// </summary>
        public readonly PolygonFillFace FillFace;

        /// <summary>
        /// The method which will be used for filling
        /// </summary>
        public readonly PolygonFillMethod FillMethod;
    }
}
