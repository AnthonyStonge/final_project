using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct LifeComponent : IComponentData
{
    public Range Life;

    // [SerializeField] private float resetValue;

    public void Reset()
    {
        Life.Value = Life.Max;
    }
}
