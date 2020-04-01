using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

[AlwaysUpdateSystem]
public class InputSystem : SystemBase
{
    protected override void OnUpdate()
     {
         bool interact = false;
             
         if (Input.GetKeyDown(KeyCode.Space))
         {
             Debug.Log("Spacebar");
             interact = true;
         }
 
         if (Input.GetKeyUp(KeyCode.Space))
         {
             interact = false;
         }
         Entities.ForEach((ref InputComponent ic) => { ic.interact = interact; }).Schedule();
     }
 }