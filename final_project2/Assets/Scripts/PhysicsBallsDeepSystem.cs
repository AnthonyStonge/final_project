using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using UnityEngine;
using static Unity.Mathematics.math;

public class PhysicsBallsDeepSystem : SystemBase
{
    protected override void OnCreate()
    {
    }
    
    protected override void OnStartRunning()
    {
    }

    protected override void OnDestroy()
    {
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("WHATATSTSTSTSST");
        }
    }
}