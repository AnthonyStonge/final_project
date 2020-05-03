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

            direction.Value = GetVelocity(dash, dt);
            EntityManager.SetComponentData(playerEntity, direction);
        }
        //Is dash finished? -> Unlock inputs...
        else if (dash.WasDashingPreviousFrame)
        {
            OnDashEnd(ref dash);
        }

        //
        if (!dash.IsAvailable)
            dash.CurrentCooldownTime -= dt;

        if (TryDash(dash, inputs))
        {
            //Dash
            Dash(ref dash, inputs, rotation);
        }

        EntityManager.SetComponentData(playerEntity, dash);
    }

    private static float2 GetVelocity(DashComponent dash, float delta)
    {
        return math.normalizesafe(dash.TargetEndDash) * dash.Speed * 100 * delta;
    }

    private static void OnDashEnd(ref DashComponent dash)
    {
        GlobalEvents.PlayerEvents.UnlockUserInputs();
        dash.WasDashingPreviousFrame = false;
    }

    private static bool TryDash(DashComponent dash, InputComponent inputs)
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

    private static void Dash(ref DashComponent dash, InputComponent inputs, Rotation rotation)
    {
        //Dash
        dash.OnDash();
        GlobalEvents.PlayerEvents.LockUserInputs();

        //Set target
        if (inputs.Move.Equals(float2.zero))
            //Go toward mouse
            dash.TargetEndDash = math.forward(rotation.Value).xz * 1000;
        else
            //Go toward moving direction
            dash.TargetEndDash = inputs.Move;
    }
}