using Unity.Entities;

[DisableAutoCreation]
[UpdateInGroup(typeof(LateSimulationSystemGroup))]
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
    }

    protected override void OnUpdate()
    {
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
