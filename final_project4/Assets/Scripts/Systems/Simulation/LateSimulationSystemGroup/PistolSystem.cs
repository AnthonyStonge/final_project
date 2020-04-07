using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(UpdateGunTransformSystem))]
public class PistolSystem : SystemBase
{
    protected override void OnUpdate()
    {
    }
}
