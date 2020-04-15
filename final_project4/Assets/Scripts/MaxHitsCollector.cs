

using Unity.Collections;
using Unity.Physics;
using UnityEngine.Assertions;

public struct MaxHitsCollector<T> : ICollector<T> where T : struct, IQueryResult
{
    private int m_NumHits;
    

    public bool EarlyOutOnFirstHit => false;
    public float MaxFraction { get; }
    public int NumHits => m_NumHits;

    public NativeArray<T> AllHits;

    public MaxHitsCollector(float maxFraction, ref NativeArray<T> allHits)
    {
        MaxFraction = maxFraction;
        AllHits = allHits;
        m_NumHits = 0;
    }
    public bool AddHit(T hit)
    {
        Assert.IsTrue(hit.Fraction < MaxFraction);
        Assert.IsTrue(m_NumHits < AllHits.Length);
        AllHits[m_NumHits] = hit;
        m_NumHits++;
        return true;
    }
    
    
}
