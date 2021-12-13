using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Vex.Graphics
{
    public enum TextureDataType
    {
        None = 0,
        Byte = 5120,
        //
        // Summary:
        //     Original was GL_UNSIGNED_BYTE = 0x1401
        UnsignedByte = 5121,
        //
        // Summary:
        //     Original was GL_SHORT = 0x1402
        Short = 5122,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT = 0x1403
        UnsignedShort = 5123,
        //
        // Summary:
        //     Original was GL_INT = 0x1404
        Int = 5124,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT = 0x1405
        UnsignedInt = 5125,
        //
        // Summary:
        //     Original was GL_FLOAT = 0x1406
        Float = 5126,
        //
        // Summary:
        //     Original was GL_HALF_FLOAT = 0x140B
        HalfFloat = 5131,
        //
        // Summary:
        //     Original was GL_UNSIGNED_BYTE_3_3_2 = 0x8032
        UnsignedByte332 = 32818,
        //
        // Summary:
        //     Original was GL_UNSIGNED_BYTE_3_3_2_EXT = 0x8032
        UnsignedByte332Ext = 32818,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033
        UnsignedShort4444 = 32819,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_4_4_4_4_EXT = 0x8033
        UnsignedShort4444Ext = 32819,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034
        UnsignedShort5551 = 32820,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_5_5_5_1_EXT = 0x8034
        UnsignedShort5551Ext = 32820,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_8_8_8_8 = 0x8035
        UnsignedInt8888 = 32821,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_8_8_8_8_EXT = 0x8035
        UnsignedInt8888Ext = 32821,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_10_10_10_2 = 0x8036
        UnsignedInt1010102 = 32822,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_10_10_10_2_EXT = 0x8036
        UnsignedInt1010102Ext = 32822,
        //
        // Summary:
        //     Original was GL_UNSIGNED_BYTE_2_3_3_REVERSED = 0x8362
        UnsignedByte233Reversed = 33634,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_5_6_5 = 0x8363
        UnsignedShort565 = 33635,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_5_6_5_REVERSED = 0x8364
        UnsignedShort565Reversed = 33636,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_4_4_4_4_REVERSED = 0x8365
        UnsignedShort4444Reversed = 33637,
        //
        // Summary:
        //     Original was GL_UNSIGNED_SHORT_1_5_5_5_REVERSED = 0x8366
        UnsignedShort1555Reversed = 33638,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_8_8_8_8_REVERSED = 0x8367
        UnsignedInt8888Reversed = 33639,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_2_10_10_10_REVERSED = 0x8368
        UnsignedInt2101010Reversed = 33640,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_24_8 = 0x84FA
        UnsignedInt248 = 34042,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B
        UnsignedInt10F11F11FRev = 35899,
        //
        // Summary:
        //     Original was GL_UNSIGNED_INT_5_9_9_9_REV = 0x8C3E
        UnsignedInt5999Rev = 35902,
        //
        // Summary:
        //     Original was GL_FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD
        Float32UnsignedInt248Rev = 36269
    }
}
