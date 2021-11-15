using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Graphics
{
    /// <summary>
    /// Supported texture formats
    /// </summary>
    public enum TextureFormat
    {
        None = 0,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT = 0x1403
        UnsignedShort = 5123,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT = 0x1405
        UnsignedInt = 5125,
        //
        // Summary:
        //     Original was GL_COLOR_INDEX = 0x1900
        ColorIndex = 6400,
        //
        // Summary:
        //     Original was GL_STENCIL_INDEX = 0x1901
        StencilIndex = 6401,
        //
        // Summary:
        //     Original was GL_DEPTH_COMPONENT = 0x1902
        DepthComponent = 6402,
        //
        // Summary:
        //     Original was GL_RED = 0x1903
        Red = 6403,
        //
        // Summary:
        //     Original was GL_RED_EXT = 0x1903
        RedExt = 6403,
        //
        // Summary:
        //     Original was GL_GREEN = 0x1904
        Green = 6404,
        //
        // Summary:
        //     Original was GL_BLUE = 0x1905
        Blue = 6405,
        //
        // Summary:
        //     Original was GL_ALPHA = 0x1906
        Alpha = 6406,
        //
        // Summary:
        //     Original was GL_RGB = 0x1907
        Rgb = 6407,
        //
        // Summary:
        //     Original was GL_RGBA = 0x1908
        Rgba = 6408,
        //
        // Summary:
        //     Original was GL_LUMINANCE = 0x1909
        Luminance = 6409,
        //
        // Summary:
        //     Original was GL_LUMINANCE_ALPHA = 0x190A
        LuminanceAlpha = 6410,
        //
        // Summary:
        //     Original was GL_ABGR_EXT = 0x8000
        AbgrExt = 32768,
        //
        // Summary:
        //     Original was GL_CMYK_EXT = 0x800C
        CmykExt = 32780,
        //
        // Summary:
        //     Original was GL_CMYKA_EXT = 0x800D
        CmykaExt = 32781,
        //
        // Summary:
        //     Original was GL_BGR = 0x80E0
        Bgr = 32992,
        //
        // Summary:
        //     Original was GL_BGRA = 0x80E1
        Bgra = 32993,
        //
        // Summary:
        //     Original was GL_YCRCB_422_SGIX = 0x81BB
        Ycrcb422Sgix = 33211,
        //
        // Summary:
        //     Original was GL_YCRCB_444_SGIX = 0x81BC
        Ycrcb444Sgix = 33212,
        //
        // Summary:
        //     Original was GL_RG = 0x8227
        Rg = 33319,
        //
        // Summary:
        //     Original was GL_RG_INTEGER = 0x8228
        RgInteger = 33320,
        //
        // Summary:
        //     Original was GL_R5_G6_B5_ICC_SGIX = 0x8466
        R5G6B5IccSgix = 33894,
        //
        // Summary:
        //     Original was GL_R5_G6_B5_A8_ICC_SGIX = 0x8467
        R5G6B5A8IccSgix = 33895,
        //
        // Summary:
        //     Original was GL_ALPHA16_ICC_SGIX = 0x8468
        Alpha16IccSgix = 33896,
        //
        // Summary:
        //     Original was GL_LUMINANCE16_ICC_SGIX = 0x8469
        Luminance16IccSgix = 33897,
        //
        // Summary:
        //     Original was GL_LUMINANCE16_ALPHA8_ICC_SGIX = 0x846B
        Luminance16Alpha8IccSgix = 33899,
        //
        // Summary:
        //     Original was GL_DEPTH_STENCIL = 0x84F9
        DepthStencil = 34041,
        //
        // Summary:
        //     Original was GL_RED_INTEGER = 0x8D94
        RedInteger = 36244,
        //
        // Summary:
        //     Original was GL_GREEN_INTEGER = 0x8D95
        GreenInteger = 36245,
        //
        // Summary:
        //     Original was GL_BLUE_INTEGER = 0x8D96
        BlueInteger = 36246,
        //
        // Summary:
        //     Original was GL_ALPHA_INTEGER = 0x8D97
        AlphaInteger = 36247,
        //
        // Summary:
        //     Original was GL_RGB_INTEGER = 0x8D98
        RgbInteger = 36248,
        //
        // Summary:
        //     Original was GL_RGBA_INTEGER = 0x8D99
        RgbaInteger = 36249,
        //
        // Summary:
        //     Original was GL_BGR_INTEGER = 0x8D9A
        BgrInteger = 36250,
        //
        // Summary:
        //     Original was GL_BGRA_INTEGER = 0x8D9B
        BgraInteger = 36251
    }
}
