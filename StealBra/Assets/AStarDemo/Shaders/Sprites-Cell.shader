Shader "Sprites/Cell"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_LineTex("Sprite Line Texture", 2D) = "black" {}
		_Mask1Tex("Sprite Mask Texture", 2D) = "black" {}
		_Mask1("Sprite Mask 1", Vector) = (1, 1, 1, 1)
		_Mask2Tex("Sprite Mask Texture", 2D) = "black" {}
		_Mask2("Sprite Mask 2", Vector) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _LineTex;
			sampler2D _Mask1Tex;
			sampler2D _Mask2Tex;
			float4 _Mask1;
			float4 _Mask2;
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				half4 lineColor = tex2D(_LineTex, uv);
				half4 mask1 = tex2D(_Mask1Tex, uv) * _Mask1;
				half4 mask2 = tex2D(_Mask2Tex, uv) * _Mask2;

				color = lerp(color, lineColor, mask1.r*lineColor.a);
				color = lerp(color, lineColor, mask1.g*lineColor.a);
				color = lerp(color, lineColor, mask1.b*lineColor.a);
				color = lerp(color, lineColor, mask1.a*lineColor.a);
				color = lerp(color, lineColor, mask2.r*lineColor.a);
				color = lerp(color, lineColor, mask2.g*lineColor.a);
				color = lerp(color, lineColor, mask2.b*lineColor.a);
				color = lerp(color, lineColor, mask2.a*lineColor.a);
#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
