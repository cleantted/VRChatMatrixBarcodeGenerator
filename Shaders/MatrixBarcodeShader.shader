Shader "MatrixBarcodeGenerator/MatrixBarcodeShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _mergin ("mergin", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        float _mergin;
        int _matrix_barcode_size;
        int _value1;
        int _value2;
        int _value3;
        int _value4;
        int _value5;
        int _value6;
        int _value7;
        int _value8;
        int _value9;
        int _value10;
        int _value11;
        int _value12;
        int _value13;
        int _value14;
        int _value15;
        int _value16;
        int _value17;
        int _value18;
        int _value19;
        int _value20;
        int _value21;
        int _value22;
        int _value23;
        int _value24;
        int _value25;
        int _value26;
        int _value27;
        int _value28;
        int _value29;
        int _value30;
        int _value31;
        int _value32;
        int _value33;
        int _value34;
        int _value35;
        int _value36;
        int _value37;
        int _value38;
        int _value39;
        int _value40;
        int _value41;
        int _value42;
        int _value43;
        int _value44;
        int _value45;
        int _value46;
        int _value47;
        int _value48;
        int _value49;
        int _value50;
        int _value51;
        int _value52;
        int _value53;
        int _value54;
        int _value55;
        int _value56;
        int _value57;
        int _value58;
        int _value59;
        int _value60;
        int _value61;
        int _value62;
        int _value63;
        int _value64;
        int _value65;
        int _value66;
        int _value67;
        int _value68;
        int _value69;
        int _value70;
        int _value71;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex;
            if (uv.x < _mergin || uv.x > 1 - _mergin || uv.y < _mergin || uv.y > 1 - _mergin) {
                o.Albedo = (float3) 1;
                return;
            }
            int w = (int) ((uv.x - _mergin) / (1 - 2 * _mergin) * _matrix_barcode_size);
            int h = (int) ((1 - _mergin - uv.y) / (1 - 2 * _mergin) * _matrix_barcode_size);
            int p = (h * _matrix_barcode_size + w);
            int use_bite_per_int = 3 * 8;

            int pp = p / use_bite_per_int;
            int d = 0;
            switch (pp) {
                case 0:
                    d = _value1;
                    break;
                case 1:
                    d = _value2;
                    break;
                case 2:
                    d = _value3;
                    break;
                case 3:
                    d = _value4;
                    break;
                case 4:
                    d = _value5;
                    break;
                case 5:
                    d = _value6;
                    break;
                case 6:
                    d = _value7;
                    break;
                case 7:
                    d = _value8;
                    break;
                case 8:
                    d = _value9;
                    break;
                case 9:
                    d = _value10;
                    break;
                case 10:
                    d = _value11;
                    break;
                case 11:
                    d = _value12;
                    break;
                case 12:
                    d = _value13;
                    break;
                case 13:
                    d = _value14;
                    break;
                case 14:
                    d = _value15;
                    break;
                case 15:
                    d = _value16;
                    break;
                case 16:
                    d = _value17;
                    break;
                case 17:
                    d = _value18;
                    break;
                case 18:
                    d = _value19;
                    break;
                case 19:
                    d = _value20;
                    break;
                case 20:
                    d = _value21;
                    break;
                case 21:
                    d = _value22;
                    break;
                case 22:
                    d = _value23;
                    break;
                case 23:
                    d = _value24;
                    break;
                case 24:
                    d = _value25;
                    break;
                case 25:
                    d = _value26;
                    break;
                case 26:
                    d = _value27;
                    break;
                case 27:
                    d = _value28;
                    break;
                case 28:
                    d = _value29;
                    break;
                case 29:
                    d = _value30;
                    break;
                case 30:
                    d = _value31;
                    break;
                case 31:
                    d = _value32;
                    break;
                case 32:
                    d = _value33;
                    break;
                case 33:
                    d = _value34;
                    break;
                case 34:
                    d = _value35;
                    break;
                case 35:
                    d = _value36;
                    break;
                case 36:
                    d = _value37;
                    break;
                case 37:
                    d = _value38;
                    break;
                case 38:
                    d = _value39;
                    break;
                case 39:
                    d = _value40;
                    break;
                case 40:
                    d = _value41;
                    break;
                case 41:
                    d = _value42;
                    break;
                case 42:
                    d = _value43;
                    break;
                case 43:
                    d = _value44;
                    break;
                case 44:
                    d = _value45;
                    break;
                case 45:
                    d = _value46;
                    break;
                case 46:
                    d = _value47;
                    break;
                case 47:
                    d = _value48;
                    break;
                case 48:
                    d = _value49;
                    break;
                case 49:
                    d = _value50;
                    break;
                case 50:
                    d = _value51;
                    break;
                case 51:
                    d = _value52;
                    break;
                case 52:
                    d = _value53;
                    break;
                case 53:
                    d = _value54;
                    break;
                case 54:
                    d = _value55;
                    break;
                case 55:
                    d = _value56;
                    break;
                case 56:
                    d = _value57;
                    break;
                case 57:
                    d = _value58;
                    break;
                case 58:
                    d = _value59;
                    break;
                case 59:
                    d = _value60;
                    break;
                case 60:
                    d = _value61;
                    break;
                case 61:
                    d = _value62;
                    break;
                case 62:
                    d = _value63;
                    break;
                case 63:
                    d = _value64;
                    break;
                case 64:
                    d = _value65;
                    break;
                case 65:
                    d = _value66;
                    break;
                case 66:
                    d = _value67;
                    break;
                case 67:
                    d = _value68;
                    break;
                case 68:
                    d = _value69;
                    break;
                case 69:
                    d = _value70;
                    break;
                case 70:
                    d = _value71;
                    break;
                default:
                    d = 0;
                    break;
            } 

            d = (d >> (use_bite_per_int - 1 - p % use_bite_per_int)) & 0x1;

            o.Albedo = (float3) (1 - d);
        }

        ENDCG
    }
}
