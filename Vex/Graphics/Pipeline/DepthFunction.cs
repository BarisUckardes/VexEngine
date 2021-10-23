using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
	/// <summary>
	/// Represents all the types of depth queries
	/// </summary>
    public enum DepthFunction
    {
		Never = 0,
		Less = 1,
		Equal = 2,
		Lequal = 3,
		Greater = 4,
		Notequal = 5,
		Gequal = 6,
		Always = 7
	}
}