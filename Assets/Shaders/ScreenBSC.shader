﻿Shader "CookbookShaders/ScreenGrayscale"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Brightness("Brightness", Range(0.0, 1)) = 1.0
        _Saturation("Saturation", Range(0.0, 1)) = 1.0
        _Contrast("Contrast", Range(0.0, 1)) = 1.0
    }
    SubShader
    {
        Pass 
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            fixed _Brightness;
            fixed _Saturation;
            fixed _Contrast;
            
            float3 ContrastSaturationBrightness(float3 color, float brt, float sat, float con)
            {
                //increase or decrease these values to adjust RGB color channels separately
                float AvgLumR = 0.5;
                float AvgLumG = 0.5;
                float AvgLumB = 0.5;
                
                //luminance coefficients for getting luminance from the image
                float3 LuminanceCoeff = float3(0.2125, 0.7154, 0.0721);
                
                //operation for brightness
                float3 AvgLumin = float3(AvgLumR, AvgLumG, AvgLumB);
                float3 brtColor = color * brt;
                float intensityf = dot(brtColor, LuminanceCoeff);
                float3 intensity = float3(intensityf, intensityf, intensityf);
                
                //operation for saturation
                float3 satColor = lerp(intensity, brtColor, sat);
                
                //operation for contrast
                float3 conColor = lerp(AvgLumin, satColor, con);
                return conColor;
            }
            
            fixed4 frag(v2f_img i) : COLOR
            {
                //get the colors from the RenderTexture and the uv's from the v2f_img struct
                fixed4 renderTex = tex2D(_MainTex, i.uv);
                
                //apply the brightness, saturation, contrast operations
                renderTex.rgb = ContrastSaturationBrightness(renderTex.rgb, 
                                                            _Brightness,
                                                            _Saturation,
                                                            _Contrast);
                
                return renderTex;
            }
            ENDCG
        }
    }
    FallBack off
}
