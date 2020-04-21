using System;
using Enums;
using EventStruct;
using Unity.Entities;

[DisableAutoCreation]
public class SwapWeaponSystem : SystemBase
{
    private EntityManager entityManager;
    private int gunEnumLength;
    
    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        gunEnumLength = Enum.GetNames(typeof(WeaponType)).Length;
    }

    protected override void OnUpdate()
    {
        //Get player inputs
        InputComponent inputs = entityManager.GetComponentData<InputComponent>(GameVariables.PlayerVars.Entity);

        if (!inputs.Enabled)
            return;
        
        //Number > mouse wheel (override)
        if (inputs.WeaponTypeDesired != WeaponType.Pistol)
        {
            SwapWeapon(inputs.WeaponTypeDesired);
        }
        else if (inputs.MouseWheel.y > 0)
        {
            //Get next weapon
            WeaponType typeDesired = (WeaponType)(((int)GameVariables.PlayerVars.CurrentWeaponHeld + 1) % gunEnumLength);
            
            if(typeDesired == WeaponType.Pistol)
               typeDesired = (WeaponType)(((int)typeDesired + 1) % gunEnumLength);
            
            SwapWeapon(typeDesired);
        }
        else if (inputs.MouseWheel.y < 0)
        {
            //Get previous weapon
            WeaponType typeDesired = (WeaponType)(((int)GameVariables.PlayerVars.CurrentWeaponHeld - 1) % gunEnumLength);

            if (typeDesired == WeaponType.Pistol)
                typeDesired = (WeaponType) gunEnumLength - 1;
            
            SwapWeapon(typeDesired);
        }

    }

    private void SwapWeapon(WeaponType type)
    {
        //Add event to NativeList
        EventsHolder.WeaponEvents.Add(new WeaponInfo
        {
            WeaponType = type,
            EventType = WeaponInfo.WeaponEventType.ON_SWAP
        });
        
        //Activate/Deactivate weapons
        Entity currentWeaponEntity = GameVariables.PlayerVars.PlayerWeaponEntities[GameVariables.PlayerVars.CurrentWeaponHeld];
        Entity desiredWeaponEntity = GameVariables.PlayerVars.PlayerWeaponEntities[type];
        
        entityManager.SetEnabled(currentWeaponEntity, false);
        entityManager.SetEnabled(desiredWeaponEntity, true);
        
        //Set CurrentGunType
        GameVariables.PlayerVars.CurrentWeaponHeld = type;
    }
}
