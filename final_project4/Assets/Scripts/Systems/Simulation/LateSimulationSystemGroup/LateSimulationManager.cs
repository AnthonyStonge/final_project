using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : SystemBase
{
    private MachineGunSystem machineGunSystem;
    private PistolSystem pistolSystem;
    private ShotgunSystem shotgunSystem;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        machineGunSystem = world.GetOrCreateSystem<MachineGunSystem>();
        pistolSystem = world.GetOrCreateSystem<PistolSystem>();
        shotgunSystem = world.GetOrCreateSystem<ShotgunSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationSystemGroup>();
        
        lateSimulation.AddSystemToUpdateList(machineGunSystem);
        lateSimulation.AddSystemToUpdateList(pistolSystem);
        lateSimulation.AddSystemToUpdateList(shotgunSystem);
    }

    protected override void OnUpdate()
    {
        //Dependency : None
        machineGunSystem.Update();
        //Dependency : None
        pistolSystem.Update();
        //Dependency : None
        shotgunSystem.Update();
    }

    protected override void OnDestroy()
    {
        
    }
}
