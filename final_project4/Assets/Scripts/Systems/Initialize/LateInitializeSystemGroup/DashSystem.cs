using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


[DisableAutoCreation]
[UpdateBefore(typeof(MoveVelocitySystem))]
public class DashSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;

        //Get Entities
        Entity playerEntity = GameVariables.Player.Entity;

        //Get Components
        DirectionData direction = EntityManager.GetComponentData<DirectionData>(playerEntity);
        DashComponent dash = EntityManager.GetComponentData<DashComponent>(playerEntity);
        InputComponent inputs = EntityManager.GetComponentData<InputComponent>(playerEntity);
        Rotation rotation = EntityManager.GetComponentData<Rotation>(playerEntity);

        //Is Player currently dashing? -> Keep moving
        if (dash.IsDashing)
        {
            dash.CurrentDashTime -= dt;

            direction.Value = GetVelocity(dash);
            EntityManager.SetComponentData(playerEntity, direction);
        }
        //Is dash finished? -> Unlock inputs...
        else if (dash.WasDashingPreviousFrame)
        {
            OnDashEnd(ref dash);
        }

        if (!dash.IsAvailable)
            dash.CurrentCooldownTime -= dt;

        if (TryDash(dash, inputs))
        {
            //Dash
            Dash(ref dash, inputs, rotation);
        }

        EntityManager.SetComponentData(playerEntity, dash);
    }

    private static float2 GetVelocity(in DashComponent dash)
    {
        return math.normalizesafe(dash.Direction) * dash.Speed;
    }

    private static void OnDashEnd(ref DashComponent dash)
    {
        GlobalEvents.PlayerEvents.UnlockUserInputs();
        dash.WasDashingPreviousFrame = false;
    }

    private static bool TryDash(in DashComponent dash, in InputComponent inputs)
    {
        //Is Dash Requested
        if (!dash.IsAvailable)
            return false;
        if (dash.IsDashing)
            return false;
        if (!inputs.Dash)
            return false;
        return true;
    }

    private static void Dash(ref DashComponent dash, in InputComponent inputs, in Rotation rotation)
    {
        //Dash
        dash.OnDash();
        GlobalEvents.PlayerEvents.LockUserInputs();

        //Look if Player is moving
        dash.Direction = inputs.Move.Equals(float2.zero) ? math.forward(rotation.Value).xz : inputs.Move;
    }
}