using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{
    private MachineGunSystem machineGunSystem;
    private PistolSystem pistolSystem;
    private ShotgunSystem shotgunSystem;

    private ProjectileHitDetectionSystem projectileHitDetectionSystem;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        machineGunSystem = world.GetOrCreateSystem<MachineGunSystem>();
        pistolSystem = world.GetOrCreateSystem<PistolSystem>();
        shotgunSystem = world.GetOrCreateSystem<ShotgunSystem>();

        projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();

        lateSimulation.AddSystemToUpdateList(machineGunSystem);
        lateSimulation.AddSystemToUpdateList(pistolSystem);
        lateSimulation.AddSystemToUpdateList(shotgunSystem);
        lateSimulation.AddSystemToUpdateList(projectileHitDetectionSystem);
    }

    protected override void OnUpdate()
    {
        //Dependency : None
        machineGunSystem.Update();
        //Dependency : None
        pistolSystem.Update();
        //Dependency : None
        shotgunSystem.Update();

        projectileHitDetectionSystem.Update();
    }

    protected override void OnDestroy()
    {
    }
}