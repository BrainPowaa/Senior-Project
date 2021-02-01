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
    float3 color;
};

// Vertex input attributes
struct Attributes
{
    uint vertexID : SV_VertexID;
    uint instanceID : SV_InstanceID;
};

StructuredBuffer<Vertex> MeshBuffer;

// Custom vertex shader
PackedVaryingsType CustomVert(Attributes input)
{
    const uint i = input.instanceID;
    const float3 instancePos = float3(i % 10, (i / 10) % 10, i / 10 / 10);
    const float3 pos = MeshBuffer[input.vertexID].position + instancePos * 8.f;
        
    AttributesMesh am;
    
    am.positionOS = pos;
#ifdef ATTRIBUTES_NEED_NORMAL
    am.normalOS = MeshBuffer[input.vertexID].normal;
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
    am.color.xyz = MeshBuffer[input.vertexID].color;
#endif
    UNITY_TRANSFER_INSTANCE_ID(input, am);

    // Throw it into the default vertex pipeline.
    VaryingsType varyingsType;
    varyingsType.vmesh = VertMesh(am);
    return PackVaryingsType(varyingsType);
}
