uint _TriangleCount;
float _LocalTime;
float _Extent;
float _NoiseAmplitude;
float _NoiseFrequency;
float3 _NoiseOffset;
float4x4 _LocalToWorld;

struct Vertex
{
    float3 position;
    float3 normal;
};

// Vertex input attributes
struct Attributes
{
    uint vertexID : SV_VertexID;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

StructuredBuffer<float3> MeshBuffer;

// Custom vertex shader
PackedVaryingsType CustomVert(Attributes input)
{
    const float3 pos = MeshBuffer[input.vertexID];
        
    AttributesMesh am;
    
    am.positionOS = pos;
#ifdef ATTRIBUTES_NEED_NORMAL
    am.normalOS = float3(0.0, 1.0, 0.0);
#endif
#ifdef ATTRIBUTES_NEED_TANGENT
    am.tangentOS = 0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD0
    am.uv0 = 0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD1
    am.uv1 = 0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD2
    am.uv2 = 0;
#endif
#ifdef ATTRIBUTES_NEED_TEXCOORD3
    am.uv3 = 0;
#endif
#ifdef ATTRIBUTES_NEED_COLOR
    am.color = 0;
#endif
    UNITY_TRANSFER_INSTANCE_ID(input, am);

    // Throw it into the default vertex pipeline.
    VaryingsType varyingsType;
    varyingsType.vmesh = VertMesh(am);
    return PackVaryingsType(varyingsType);
}
