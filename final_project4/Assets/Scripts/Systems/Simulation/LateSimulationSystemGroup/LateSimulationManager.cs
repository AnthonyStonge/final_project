using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : SystemBase
{
    private UpdateGunTransformSystem updateGunTransformSystem;
    private MachineGunSystem machineGunSystem;
    private PistolSystem pistolSystem;
    private ShotgunSystem shotgunSystem;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        updateGunTransformSystem = world.GetOrCreateSystem<UpdateGunTransformSystem>();
        machineGunSystem = world.GetOrCreateSystem<MachineGunSystem>();
        pistolSystem = world.GetOrCreateSystem<PistolSystem>();
        shotgunSystem = world.GetOrCreateSystem<ShotgunSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationSystemGroup>();
        
        lateSimulation.AddSystemToUpdateList(updateGunTransformSystem);
        lateSimulation.AddSystemToUpdateList(machineGunSystem);
        lateSimulation.AddSystemToUpdateList(pistolSystem);
        lateSimulation.AddSystemToUpdateList(shotgunSystem);
    }

    protected override void OnUpdate()
    {
        Debug.Log("LateSimulation Manager Update");
        //Dependency : None
        updateGunTransformSystem.Update();
        //Dependency : UpdateGunTransformSystem
        machineGunSystem.Update();
        //Dependency : UpdateGunTransformSystem
        pistolSystem.Update();
        //Dependency : UpdateGunTransformSystem
        shotgunSystem.Update();
    }

    protected override void OnDestroy()
    {
        
    }
}
