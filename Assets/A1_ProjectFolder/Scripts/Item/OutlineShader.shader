// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline" {
    Properties {
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range (0, 0.1)) = 0.001
        _CameraDepthTexture ("Camera Depth Texture", 2D) = "white" {}
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100
 
        Pass {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata {
                float4 vertex : POSITION;
            };
 
            struct v2f {
                float4 vertex : SV_POSITION;
            };
 
            float _OutlineWidth;
            float4 _OutlineColor;
            sampler2D _CameraDepthTexture;
 
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target {
                // Calculate the screen space position of the pixel
                float2 screenPos = i.vertex.xy / i.vertex.w * _ScreenParams.xy + _ScreenParams.zw;
                // Sample the depth texture to get the depth at the pixel
                float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, UNITY_PROJ_COORD(screenPos)));
                // Calculate the depth difference between the pixel and its neighbors
                float depthDiffX = abs(depth - Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, UNITY_PROJ_COORD(screenPos + float2(_OutlineWidth, 0)))));
                float depthDiffY = abs(depth - Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, UNITY_PROJ_COORD(screenPos + float2(0, _OutlineWidth))))); 
                // Calculate the edge factor based on the depth differences
                float edgeFactor = (depthDiffX + depthDiffY) * 500.0;
                // Use the edge factor to blend the outline color with the object color
                return lerp(_OutlineColor, fixed4(0,0,0,0), edgeFactor);
            }
            ENDCG
        }
    }
}
