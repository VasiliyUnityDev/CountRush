Shader "Flexus/Universal"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0)
        _GlowColor ("Glow Color", Color) = (.4,.4,.4)
        _GlowPower ("Glow Power", Float) = 1
//		_Point ("Point Light", Vector) = (0,0,0,1)
//		_PointColor ("Point Light Color", Color) = (1,1,1,1)
		_SpecularMultiplier ("Specular Multiplier", Float) = 4
		_SpecularCompensation ("Specular Compensation", Float) = -3.5
		[Space(10)][Toggle(USE_FOG)] _UseFog("Fog", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
			Tags {"LightMode"="ForwardBase"}
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#pragma shader_feature USE_FOG
			#pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				half3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
				float3 ws_pos : TEXCOORD0;
				half3 V : COLOR0;
				half3 N : NORMAL;
				fixed NdotL : COLOR1;
				fixed3 ambient : COLOR2;
				SHADOW_COORDS(1)
				#ifdef USE_FOG
				UNITY_FOG_COORDS(2)
				#endif
            };

			fixed3 _Color;
			fixed3 _GlowColor;
			//float4 _Point;
			//fixed3 _PointColor;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
				o.ws_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.N = UnityObjectToWorldNormal(v.normal);
				o.V = -normalize(UnityWorldSpaceViewDir(o.ws_pos));
				//half3 L = _Point.xyz-o.ws_pos;
				//o.NdotV = max(dot(N,normalize(V)),0);
				//fixed NdotL = max(dot(N,normalize(L)),0);
				
				fixed3 light = unity_AmbientSky.rgb; //Ambient
				
				o.ambient = ShadeSH9(half4(o.N, 1));

				//float len = length(L);
				//float atten = max(1 - len/_Point.w,0);
				//fixed3 flash = NdotL * _PointColor * atten; //Point

				half3 L = _WorldSpaceLightPos0.xyz;
				o.NdotL = max(dot(o.N,L), 0);//*.5+.5;
				light += o.NdotL*_LightColor0.rgb; //Directional
				
				//fixedcolor = saturate(_GlowColor*(1-NdotV)+_Color*light/*+flash*/);
				TRANSFER_SHADOW(o)
				#ifdef USE_FOG
				UNITY_TRANSFER_FOG(o,o.pos);
				#endif
				return o;
            }
			
			fixed _SpecularMultiplier;
			fixed _SpecularCompensation;
			fixed _GlowPower;

            fixed4 frag (v2f i) : SV_Target
            {
				//i.color *= .5 + .5 * i.NdotL * SHADOW_ATTENUATION(i);
				fixed shadow = SHADOW_ATTENUATION(i);
				half3 NdotV = saturate(dot(i.N, -i.V));
				fixed3 glow = pow(1 - NdotV, _GlowPower) * _GlowColor;
				fixed3 lambert = i.NdotL * _LightColor0.rgb * shadow + i.ambient;
				fixed specular = max(dot(reflect(i.V, i.N), _WorldSpaceLightPos0.xyz) * _SpecularMultiplier + _SpecularCompensation, 0) * shadow;
				fixed3 color = _Color * lambert + specular * _LightColor0.rgb + glow;	
				
				//color = i.NdotL * _LightColor0.rgb * shadow;
				//color = lambert;// i.ambient*_Color+specular * _LightColor0.rgb + glow;
				#ifdef USE_FOG
				UNITY_APPLY_FOG(i.fogCoord, color);
				#endif
				return fixed4(saturate(color),1);
            }
            ENDCG
        }
	UsePass "VertexLit/SHADOWCASTER"
    }
}	