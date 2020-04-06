using Unity.Entities;
using Unity.Physics;

public class RayComponent : IComponentData
{
    public RaycastInput RayInfo;
    public RaycastHit Hit;
}
