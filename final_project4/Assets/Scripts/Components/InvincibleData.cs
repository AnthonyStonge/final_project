using System;
using Unity.Entities;

[Serializable]
public struct InvincibleData : IComponentData
{
    public float Timer;
}