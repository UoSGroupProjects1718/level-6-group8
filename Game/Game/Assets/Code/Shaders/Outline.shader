Shader "Outlined/Uniform"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Texture", 2D) = "white" {}
	    _OutlineColor("Outline color", Color) = (1,1,1,1)
	    _Smoothness("Smoothness", Range(0,1)) = 0.5
	    _Metallic("Metallic", Range(0,1)) = 0
		_OutlineWidth("Outlines width", Range(0.0, 2.0)) = 1.1
	}

		CGINCLUDE
#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	uniform sampler2D _MainTex;
	uniform float4 _Color;

	ENDCG

    SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }

		Pass //Outline
	    {
		ZWrite Off
		Cull Back
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

		v2f vert(appdata v)
	    {
            appdata original = v;
            v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);
    
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            return o;

        }

        half4 frag(v2f i) : COLOR
        {
            return _OutlineColor;
        }

            ENDCG
        }

		Tags{ "Queue" = "Geometry" }

		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

        half _Smoothness;
        half _Metallic;

        struct Input {
            float2 uv_MainTex;
        };
    
        void surf(Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Smoothness = _Smoothness;
            o.Metallic = _Metallic;
        }
	ENDCG
	}
    Fallback "Diffuse"
}