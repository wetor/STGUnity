// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/AA" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _DH("Hue", Range(0, 360)) = 0
        _DS("Saturation", Range(-1, 1)) = 0
        _DL("Lightness", Range(-1, 1)) = 0
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType" = "Plane"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:setup
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DH;
            float _DS;
            float _DL;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f ip) : SV_Target
            {
                /*fixed4 col = tex2D(_MainTex, i.texcoord);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;*/
				fixed4 col = tex2D(_MainTex, ip.texcoord);
				float r = col.r;
				float g = col.g;
				float b = col.b;
				float a = col.a;
				float h;
				float s;
				float l;
				float maxv = max(max(r, g), b);
				float minv = min(min(r, g), b);
				if (maxv == minv) {
					h = 0.0;
				}
				else if (maxv == r && g >= b) {
					h = 60.0 * (g - b) / (maxv - minv) + 0.0;
				}
				else if (maxv == r && g < b) {
					h = 60.0 * (g - b) / (maxv - minv) + 360.0;
				}
				else if (maxv == g) {
					h = 60.0 * (b - r) / (maxv - minv) + 120.0;
				}
				else if (maxv == b) {
					h = 60.0 * (r - g) / (maxv - minv) + 240.0;
				}
				l = 0.5 * (maxv + minv);
				if (l == 0.0 || maxv == minv) {
					s = 0.0;
				}
				else if (0.0 <= l && l <= 0.5) {
					s = (maxv - minv) / (2.0 * l);
				}
				else if (l > 0.5) {
					s = (maxv - minv) / (2.0 - 2.0 * l);
				}
				h = h + _DH;
				s = min(1.0, max(0.0, s + _DS));
				l = l + _DL;
				// final color
				float q;
				if (l < 0.5) {
					q = l * (1.0 + s);
				}
				else if (l >= 0.5) {
					q = l + s - l * s;
				}
				float p = 2.0 * l - q;
				float hk = h / 360.0;
				float t[3];
				t[0] = hk + 1.0 / 3.0;
				t[1] = hk;
				t[2] = hk - 1.0 / 3.0;
				for (int i = 0; i < 3; i++) {
					if (t[i] < 0.0) {
						t[i] += 1.0;
					}
					else if (t[i] > 1.0) {
						t[i] -= 1.0;
					}
				}
				float c[3];
				for (int i = 0; i < 3; i++) {
					if (t[i] < 1.0 / 6.0) {
						c[i] = p + ((q - p) * 6.0 * t[i]);
					}
					else if (1.0 / 6.0 <= t[i] && t[i] < 0.5) {
						c[i] = q;
					}
					else if (0.5 <= t[i] && t[i] < 2.0 / 3.0) {
						c[i] = p + ((q - p) * 6.0 * (2.0 / 3.0 - t[i]));
					}
					else {
						c[i] = p;
					}
				}
				fixed4 finalColor = fixed4(c[0], c[1], c[2], a);
				finalColor += fixed4(_DL, _DL, _DL, 0.0);
				return finalColor;
            }
            struct Input {
                float2 uv_MainTex;
            };

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            StructuredBuffer<float4> positionBuffer;
            //StructuredBuffer<float4x4> rotationBuffer;
#endif

            void setup()
            {
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

                float4 position = positionBuffer[unity_InstanceID];
                float scale = 1;

                unity_ObjectToWorld._11_21_31_41 = float4(scale, 0, 0, 0);
                unity_ObjectToWorld._12_22_32_42 = float4(0, scale, 0, 0);
                unity_ObjectToWorld._13_23_33_43 = float4(0, 0, scale, 0);
                unity_ObjectToWorld._14_24_34_44 = float4(position.xyz, 1);

                unity_WorldToObject = unity_ObjectToWorld;
                unity_WorldToObject._14_24_34 *= -1;
                unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;
#endif
            }


        ENDCG
    }
}

}
