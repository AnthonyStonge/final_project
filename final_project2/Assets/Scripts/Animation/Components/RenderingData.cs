using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RenderingDataProxy : SharedComponentDataProxy<RenderingData>
{
    
}

[Serializable]
public struct RenderingData : ISharedComponentData, IEquatable<RenderingData>
{
    public UnitType unitType;
    public GameObject bakingPrefab;
    public Material material;

    public LodData lodData;

    public bool Equals(RenderingData other)
    {
        return unitType == other.unitType && Equals(bakingPrefab, other.bakingPrefab) && Equals(material, other.material) && lodData.Equals(other.lodData);
    }

    public override bool Equals(object obj)
    {
        return obj is RenderingData other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int) unitType;
            hashCode = (hashCode * 397) ^ (bakingPrefab != null ? bakingPrefab.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (material != null ? material.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ lodData.GetHashCode();
            return hashCode;
        }
    }
}
