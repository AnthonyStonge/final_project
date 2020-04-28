using Unity.Collections;
using Unity.Entities;

public struct NativeListContainer<T> where T : struct
{ 
    [ReadOnly] public NativeList<T> List;
}
