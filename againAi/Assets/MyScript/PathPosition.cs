using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public struct PathPosition : IBufferElementData
{
    public int2 position;
}
