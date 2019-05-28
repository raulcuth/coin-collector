Shader "CookbookShaders/DiffuseShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _BlendTex("Blend Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf SimpleLambert exclude_path:prepass noforwardadd

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _BlendTex;

        struct Input
        {
            half2 uv_MainTex;           
        };
        
        inline float4 LightingSimpleLambert(SurfaceOutput s, float3 lightDir, fixed atten)
        {
            fixed diff = max(0, dot(s.Normal, lightDir));
            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
            c.a = s.Alpha;
            return c;
        }
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 blendTex = tex2D(_BlendTex, IN.uv_MainTex);
            
            c = lerp(c, blendTex, blendTex.r);
            
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
