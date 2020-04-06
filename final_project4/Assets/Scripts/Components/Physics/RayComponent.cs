using Unity.Entities;
using Unity.Physics;

public class RayComponent : IComponentData
{
    public RaycastInput rayInfo;
    public RaycastHit hit;
}
