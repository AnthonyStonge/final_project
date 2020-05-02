using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]
[AlwaysUpdateSystem]
[UpdateAfter(typeof(StateMovingSystem))]
public class StateAttackingSystem : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        //Player state
        Entity player = GameVariables.Player.Entity;
        InputComponent playerInputs = GetComponent<InputComponent>(player);
        GunComponent gun = GetComponent<GunComponent>(
            GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]);

        //Make sure player can attack (Weapon not on cooldown)
        if (gun.SwapTimer <= 0 && playerInputs.Shoot)
        {
            //Add StateEvent
            EventsHolder.StateEvents.Add(new StateInfo
            {
                Entity = player,
                DesiredState = State.Attacking,
                Action = StateInfo.ActionType.TryChange
            });
        }

        //TODO DONT RUN QUERY IF NO ENEMIES EXISTS
        var playerPos = GetComponent<Translation>(player);
        //Act on all entities with AttackStateData and EnemyTag
        Entities.WithAll<EnemyTag>().ForEach(
            (ref StateComponent state, in Translation currentPosition, in AttackStateData range) =>
            {
                //TODO IMPLEMENT STATE MACHINE WITH ENEMIES
                //Compare distance between current position and target position. If distance <= range -> set state to attack
                if (math.distancesq(currentPosition.Value, playerPos.Value) <= range.Value * range.Value)
                {
                    state.CurrentState = State.Attacking;
                }
            }).ScheduleParallel();
    }
}