using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using static GameVariables;

public static class MapInitializer
{
    private static EntityManager entityManager;

    private static RenderMesh pistolRenderMesh;

    public static void Initialize()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        if (entityManager == null)
            return;

        InitializePlayer();
        InitializePlayerWeapon();
    }

    private static void InitializePlayer()
    {
        //Create player entity
        Entity player = entityManager.CreateEntity(StaticArchetypes.PlayerArchetype);
        entityManager.SetName(player, "Player");

        //Set Values
        entityManager.SetComponentData(player, new Translation
        {
            Value = PlayerVars.SpawnPosition
        });
        entityManager.SetComponentData(player, new Rotation
        {
            Value = PlayerVars.SpawnRotation
        });
        entityManager.SetComponentData(player, new HealthData
        {
            Value = PlayerVars.Health
        });
        entityManager.SetComponentData(player, new SpeedData
        {
            Value = PlayerVars.Speed
        });
        entityManager.SetComponentData(player, new StateData
        {
            Value = StateActions.IDLE
        });
        entityManager.SetComponentData(player, new DashComponent
        {
            Distance = PlayerVars.DefaultDashDistance,
            Timer = new TimeTrackerComponent
            {
                ResetValue = PlayerVars.DashResetTime
            }
        });
        //TODO CHANGE MESH COMPONENT FOR AN "ANIMATOR"
        entityManager.SetSharedComponentData(player, new RenderMesh
        {
            mesh = MonoGameVariables.instance.PlayerMesh,
            material = MonoGameVariables.instance.playerMaterial
        });

        entityManager.AddComponentData(player, new TimeTrackerComponent(5));

        //Set info in GameVariables
        PlayerVars.Entity = player;
        PlayerVars.CurrentPosition = PlayerVars.SpawnPosition;
        PlayerVars.CurrentState = StateActions.IDLE;
        PlayerVars.IsAlive = PlayerVars.Health > 0;
    }

    private static void InitializePlayerWeapon()
    {
        //TODO INITIALIZE ANY WEAPON NOT ONLY PISTOL
        //Create player entity
        Entity weapon = entityManager.CreateEntity(StaticArchetypes.GunArchetype);
        entityManager.SetName(weapon, "Player Weapon");

        entityManager.AddComponent<PistolComponent>(weapon);

        //Set Values
        entityManager.SetComponentData(weapon, new Translation
        {
            //TODO PROBLEM IF PLAYER SPAWNS WITH A ROTATION
            Value = PlayerVars.SpawnPosition + PistolVars.PlayerOffset
        });
        entityManager.SetComponentData(weapon, new Rotation
        {
            Value = PlayerVars.SpawnRotation
        });
        entityManager.SetComponentData(weapon, new Parent
        {
            Value = PlayerVars.Entity
        });
        
        entityManager.SetSharedComponentData(weapon, new RenderMesh
        {
            mesh = MonoGameVariables.instance.PistolMesh,
            material = MonoGameVariables.instance.PistolMaterial
        });

        Entity e = entityManager.CreateEntity(StaticArchetypes.BulletArchetype);
        
        entityManager.SetName(e, "Pistol Bullet");
        entityManager.SetEnabled(e, false);
        
        entityManager.SetSharedComponentData(e, new RenderMesh
        {
            mesh = MonoGameVariables.instance.BulletMesh,
            material = MonoGameVariables.instance.BulletMaterial
        });
        entityManager.SetComponentData(e, new Scale
       {
           Value = 1
       });
        
        entityManager.SetComponentData(e, new DamageProjectile
        {
            Speed = PistolVars.Bullet.Speed
        });
        
        //entityManager.SetEnabled(e, false);
        
        entityManager.SetComponentData(weapon, new PistolComponent
        {
            CurrentBulletInMagazine = PlayerVars.StartingBulletAmount,
            ReloadTime = PistolVars.ReloadTime,
            BetweenShotTime = PistolVars.BetweenShotTime,
            bullet = e
        });
    }
}