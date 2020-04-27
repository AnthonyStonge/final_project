using System;
using Unity.Entities;

public struct AnimationBatch : ISharedComponentData, IEquatable<AnimationBatch>
{
    public int BatchId;
    
    public bool Equals(AnimationBatch other)
    {
        return BatchId == other.BatchId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
