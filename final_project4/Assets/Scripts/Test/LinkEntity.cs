using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct LinkPtr<T> : IComponentData where T : struct
{
    public BlobPtr<T> link;
}

public struct MachineGun : IComponentData
{
    public int bullets;
    public bool fire;
    public Entity parent;
}

public struct StateMachine : IComponentData
{
    public bool state;
}
public struct BlobTest
{
    public int num;
}







//Who is KevinM????? 