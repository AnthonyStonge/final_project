using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(ProjectileHitDetectionSystem))]
public class RetrieveVisualEventSystem : SystemBase
{
    
    
    protected override void OnUpdate()
    {
        //Take event and play it at desired position
        //Bullets
    }
}
