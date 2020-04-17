using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(LateInitializeManager))]
public class LateSimulationManager : ComponentSystemGroup
{
    private MachineGunSystem machineGunSystem;
    private ShotgunSystem shotgunSystem;
    private RetrieveGunEventSystem retrieveGunEventSystem;
    // private ProjectileHitDetectionSystem projectileHitDetectionSystem;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        machineGunSystem = world.GetOrCreateSystem<MachineGunSystem>();
        shotgunSystem = world.GetOrCreateSystem<ShotgunSystem>();
        retrieveGunEventSystem = world.GetOrCreateSystem<RetrieveGunEventSystem>();

        // projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();

        lateSimulation.AddSystemToUpdateList(machineGunSystem);
        lateSimulation.AddSystemToUpdateList(shotgunSystem);
        lateSimulation.AddSystemToUpdateList(retrieveGunEventSystem);
        // lateSimulation.AddSystemToUpdateList(projectileHitDetectionSystem);
    }

    protected override void OnUpdate()
    {
        //Dependency : None
        machineGunSystem.Update();
        //Dependency : None
        shotgunSystem.Update();
        
        retrieveGunEventSystem.Update();
        

        // projectileHitDetectionSystem.Update();
    }

    protected override void OnDestroy()
    {
    }
}