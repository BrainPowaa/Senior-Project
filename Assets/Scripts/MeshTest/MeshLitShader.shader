Shader "Custom/MeshLitShader"
{
<<<<<<< HEAD
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
=======
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="MarchingCubes" }
		
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 5.0

		half4 _Color;
		half _Glossiness;
		half _Metallic;

		struct v2f 
		{
			float4 vertex : SV_POSITION;
			float3 col : Color;
		};

		// Vertex input attributes
	struct Attributes
	{
	    uint vertexID : SV_VertexID;
	    UNITY_VERTEX_INPUT_INSTANCE_ID
	};

#ifdef SHADER_API_D3D11
		StructuredBuffer<float3> MeshBuffer;
#endif

		PackedVaryingsType vert(Attributes IN)
		{
#ifdef SHADER_API_D3D11
			float3 vert = MeshBuffer[IN.vertexID];
			
			AttributesMesh OUT;
			OUT.vertex.xyz = vert;

			return OUT;
#endif
		}

		struct Input
		{
			float not_in_use;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
>>>>>>> 573c01e (Voxel Engine: Successful mesh on GPU test, will work on implementing it in the voxel engine)
