using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{
    private MachineGunSystem machineGunSystem;
    private PistolSystem pistolSystem;
    private ShotgunSystem shotgunSystem;

    // private CollisionTest collisionTest;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        machineGunSystem = world.GetOrCreateSystem<MachineGunSystem>();
        pistolSystem = world.GetOrCreateSystem<PistolSystem>();
        shotgunSystem = world.GetOrCreateSystem<ShotgunSystem>();

        // collisionTest = world.GetOrCreateSystem<CollisionTest>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();

        lateSimulation.AddSystemToUpdateList(machineGunSystem);
        lateSimulation.AddSystemToUpdateList(pistolSystem);
        lateSimulation.AddSystemToUpdateList(shotgunSystem);
        // lateSimulation.AddSystemToUpdateList(collisionTest);
    }

    protected override void OnUpdate()
    {
        //Dependency : None
        machineGunSystem.Update();
        //Dependency : None
        pistolSystem.Update();
        //Dependency : None
        shotgunSystem.Update();

        //collisionTest.Update();
    }

    protected override void OnDestroy()
    {
    }
}