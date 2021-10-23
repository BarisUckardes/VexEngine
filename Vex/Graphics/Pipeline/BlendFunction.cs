using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
	/// <summary>
	/// Reprensents the blending query types
	/// </summary>
    public enum BlendFunction
    {
		Zero = 1,
		One = 2,
		SourceColor = 3,
		OneMinusSourceColor = 4,
		DestColor = 5,
		OneMinusDestColor = 6,
		SourceAlpha = 7,
		OneMinusSourceAlpha = 8,
		DestAlpha = 9,
		OneMinusDestAlpha = 10,
		ConstantColor = 11,
		OneMinusConstantColor = 12,
		ConstantAlpha = 13,
		OneMinusConstantAlpha = 14

	}
}
