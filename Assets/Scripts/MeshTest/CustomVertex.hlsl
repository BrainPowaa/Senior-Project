struct Triangle
{
    float3 normal;
    float3 color;
    // Packed verts
    int vertices[3];
};

// Vertex input attributes
struct Attributes
{
    uint vertexID : SV_VertexID;
    uint instanceID : SV_InstanceID;
};

StructuredBuffer<Triangle> MeshBuffer;

// Custom vertex shader
PackedVaryingsType CustomVert(Attributes input)
{
    AttributesMesh am;

    const Triangle tri = MeshBuffer[input.vertexID / 3];
    const int vert = tri.vertices[input.vertexID % 3];
    
    am.positionOS = float3(vert & 1023, (vert >> 10)  & 1023, (vert >> 20)  & 1023);
#ifdef ATTRIBUTES_NEED_NORMAL
    am.normalOS = tri.normal;
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
    //am.color.xyz = 1.f;
    am.color.xyz = tri.color;
#endif
    UNITY_TRANSFER_INSTANCE_ID(input, am);

    // Throw it into the default vertex pipeline.
    VaryingsType varyingsType;
    varyingsType.vmesh = VertMesh(am);
    return PackVaryingsType(varyingsType);
}
