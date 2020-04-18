using Unity.Collections;
using Unity.Entities;

public struct ComponentDataContainer<T> where T : struct, IComponentData
{
    [ReadOnly] public ComponentDataFromEntity<T> Components;
}
