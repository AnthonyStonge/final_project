using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using Unity.Physics.Authoring;
public class VelocitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref InputComponent ic) =>
        {
         
            Test.TROLOLOL();
        }).Schedule();
    }
}

public static class Test
{
    public static void TROLOLOL()
    {
        Debug.Log("What tha fuck");
    }
}