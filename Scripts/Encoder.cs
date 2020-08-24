
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Encoder : UdonSharpBehaviour
{
    bool IsU16HighSurrogate(char ch) {
        return 0xD800 <= ch && ch <= 0xDC00;
    }

    bool IsU16LowSurrogate(char ch) {
        return 0xDC00 <= ch && ch <= 0xE000;
    }

    void ConvertU16ToU32(ref char[] u16Array, ref uint[] u32char) {
        int outIter = 0;
        int inIter = 0;
        while (inIter < u16Array.Length) {
            if (IsU16HighSurrogate(u16Array[inIter])) {
                if (inIter + 1 < u16Array.Length && IsU16LowSurrogate(u16Array[inIter + 1]))  {
                    u32char[outIter] = 65536u + ((uint) u16Array[inIter] - 55296u) * 1024u + ((uint) u16Array[inIter + 1] - 56320u); 
                    inIter += 2;
                    outIter++;
                } else if (inIter + 1 >= u16Array.Length || u16Array[inIter + 1] == 0) {
                    u32char[outIter] = (uint) u16Array[inIter];
                    inIter += 2;
                    outIter++;
                }
            } else if (IsU16LowSurrogate(u16Array[inIter])) {
                u32char[outIter] = (uint) u16Array[inIter];
                inIter++;
                outIter++;
            } else {
                u32char[outIter] = (uint) u16Array[inIter];
                inIter++;
                outIter++;
            }
        }
    }

    int ConvertU32ToU8AndAppendArray(ref byte[] u8Char, ref uint u32Char, int iter) {
        if (u32Char < 0u || u32Char > 1114111u) {
            return iter;
        }

        if (u32Char < 128u) {
            u8Char[iter] = (byte) u32Char;
            return iter + 1;
        } else if (u32Char < 2048u) {
            u8Char[iter] = (byte) (0xC0 | (u32Char >> 6));
            u8Char[iter + 1] = (byte) (0x80 | (u32Char & 0x3F));
            return iter + 2;
        } else if (u32Char < 65536u) {
            u8Char[iter] = (byte) (0xE0 | (u32Char >> 12));
            u8Char[iter + 1] = (byte) (0x80 | ((u32Char >> 6) & 0x3F));
            u8Char[iter + 2] = (byte) (0x80 | (u32Char & 0x3F));
            return iter + 3;
        } else {
            u8Char[iter] = (byte) (0xF0 | (u32Char >> 18));
            u8Char[iter + 1] = (byte) (0x80 | ((u32Char >> 12) & 0x3F));
            u8Char[iter + 2] = (byte) (0x80 | ((u32Char >> 6) & 0x3F));
            u8Char[iter + 3] = (byte) (0x80 | (u32Char & 0x3F));
            return iter + 4;
        }
    }

    byte[] ConvertU16ToU8(ref char[] u16Char) {
        uint[] u32Char = new uint[u16Char.Length];
        ConvertU16ToU32(ref u16Char, ref u32Char);
        byte[] tmp = new byte[200];
        int iter = 0;
        for (int i = 0; i < u32Char.Length; i++) {
            iter = ConvertU32ToU8AndAppendArray(ref tmp, ref u32Char[i], iter);
        }

        byte[] u8Char = new byte[iter];
        for (int i = 0; i < u8Char.Length; i++) {
            u8Char[i] = tmp[i];
        }
        
        return u8Char;
    }

    public byte[] StringToU8(ref string str) {
        char[] c = str.ToCharArray();
        return ConvertU16ToU8(ref c);
    }
}
