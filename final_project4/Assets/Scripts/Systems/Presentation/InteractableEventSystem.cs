using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using UnityEngine;

[DisableAutoCreation]
public class InteractableEventSystem : SystemBase
{
    private static EntityManager manager;

    protected override void OnCreate()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        foreach (InteractableInfo info in EventsHolder.InteractableEvents)
        {
            switch (info.InteractableType)
            {
                case InteractableType.UseOnInput:
                    UseOnInputDispatch(info);
                    break;
                case InteractableType.UseOnEnterZone:
                    UseOnEnterZoneDispatch(info);
                    break;
            }
        }
    }

    private static void UseOnInputDispatch(InteractableInfo info)
    {
        switch (info.CollisionType)
        {
            case InteractableInfo.InteractableCollisionType.OnTriggerEnter:
                OnEnterInputInteractable(info);
                break;
            case InteractableInfo.InteractableCollisionType.OnTriggerExit:
                OnExitInputInteractable(info);
                break;
        }
    }

    private static void UseOnEnterZoneDispatch(InteractableInfo info)
    {
        //Make sure its an OnEnterTrigger
        if (info.CollisionType != InteractableInfo.InteractableCollisionType.OnTriggerEnter)
            return;

        //Use depending on object type
        switch (info.ObjectType)
        {
            case InteractableObjectType.Portal:
                OnEnterPortal(info);
                break;
            case InteractableObjectType.Ammo:
                OnWalkOverAmmo(info);
                break;
            case InteractableObjectType.Door:
                OnEnterDoorTrigger(info);
                break;
            case InteractableObjectType.DoorClosing:
                OnEnterDoorClosingTrigger(info);
                break;
            case InteractableObjectType.Weapon:
                OnWalkOverWeapon(info);
                break;
            case InteractableObjectType.DeepHole:
                OnWalkOverDeepHole(info);
                break;
            case InteractableObjectType.WinningPortal:
                OnWinningPortal(info);
                break;
        }
    }

    private static void OnEnterInputInteractable(InteractableInfo info)
    {
#if UNITY_EDITOR
        Debug.Log($"Entered InputInteractable of type {info.ObjectType}");
#endif
        //Set Object in GameVariables
        GameVariables.Interactables.CurrentInteractableSelected = new GameVariables.Interactables.Interactable
        {
            Entity = info.TriggerEntity,
            ObjectType = info.ObjectType
        };

        //Toggle desired system on
        switch (info.ObjectType)
        {
            case InteractableObjectType.Switch:
                break;
            case InteractableObjectType.Door:
                ToggleSystem<InteractableDoorSystem>(true);
                break;
        }
    }

    private static void OnExitInputInteractable(InteractableInfo info)
    {
#if UNITY_EDITOR
        Debug.Log($"Exited InputInteractable of type {info.ObjectType}");
#endif
        //Remove Object in GameVariables
        GameVariables.Interactables.PreviousInteractableSelected =
            GameVariables.Interactables.CurrentInteractableSelected;
        GameVariables.Interactables.CurrentInteractableSelected = null;

        //Toggle desired system off
        switch (info.ObjectType)
        {
            case InteractableObjectType.Switch:
                break;
            case InteractableObjectType.Door:
                ToggleSystem<InteractableDoorSystem>(false);
                break;
        }
    }

    private static void ToggleSystem<T>(bool enable) where T : SystemBase
    {
        World.DefaultGameObjectInjectionWorld.GetExistingSystem<T>().Enabled = enable;
    }

    private static void OnEnterPortal(InteractableInfo info)
    {
#if UNITY_EDITOR
        Debug.Log("Entered Portal");
#endif
        //Get PortalComponent
        PortalData data =
            World.DefaultGameObjectInjectionWorld.EntityManager
                .GetComponentData<PortalData>(info.TriggerEntity);

        //Get PortalInfo
        MapInfo.Portal portal = MapHolder.MapsInfo[MapEvents.CurrentTypeLoaded].Portals[data.Value];

        if (!TryEnterPortal(portal.MapTypeLeadingTo, portal.PortalIdLeadingTo))
        {
#if UNITY_EDITOR
            Debug.Log("The Portal ur trying to teleport to doesnt exist yet...");
#endif
            return;
        }

        //Get PortalInfo of other map
        MapInfo.Portal objectivePortal = MapHolder.MapsInfo[portal.MapTypeLeadingTo].Portals[portal.PortalIdLeadingTo];

        //Change Map
        MapEvents.LoadMap(portal.MapTypeLeadingTo);

        //Set player position/rotation
        GlobalEvents.PlayerEvents.SetPlayerPosition(objectivePortal.Position);
        GlobalEvents.PlayerEvents.SetPlayerRotation(objectivePortal.Rotation);
    }

    private static bool TryEnterPortal(MapType mapType, ushort portalId)
    {
        //Make sure PortalId exist in MapType
        if (!MapHolder.MapsInfo.ContainsKey(mapType))
        {
#if UNITY_EDITOR
            Debug.Log($"MapsInfo doesnt contain the MapType {mapType}...");
#endif
            return false;
        }
        else if (!MapHolder.MapsInfo[mapType].Portals.ContainsKey(portalId))
        {
#if UNITY_EDITOR
            Debug.Log($"The MapType in MapsInfo doesnt contain the PortalId {portalId}...");
#endif
            return false;
        }

        //return MapHolder.MapsInfo.ContainsKey(mapType) && MapHolder.MapsInfo[mapType].Portals.ContainsKey(portalId);
        return true;
    }

    private static void OnWalkOverAmmo(InteractableInfo info)
    {
#if UNITY_EDITOR
        Debug.Log("Walked Over Ammo");
#endif
    }

    private static void OnEnterDoorTrigger(InteractableInfo info)
    {
        //Get Door Entity
        Entity door = manager.GetComponentData<InteractableComponent>(info.TriggerEntity).DoorToOpen;

        //Remove Door collider
        manager.RemoveComponent<PhysicsCollider>(door);

        //TODO Start animation

        //TODO REMOVE UNDER
        manager.RemoveComponent<RenderMesh>(door);

        //Remove Trigger (because its been use so...)
        manager.DestroyEntity(info.TriggerEntity);
    }
    
    private static void OnEnterDoorClosingTrigger(InteractableInfo info)
    {
        //Get Door Entity
        Entity door = manager.GetComponentData<InteractableComponent>(info.TriggerEntity).DoorToOpen;

        //Activate door
        manager.SetEnabled(door, true);

        //Remove Trigger (because its been use so...)
        manager.DestroyEntity(info.TriggerEntity);
    }

    private static void OnWalkOverWeapon(InteractableInfo info)
    {
        //Get WeaponType
        WeaponType type = manager.GetComponentData<InteractableComponent>(info.TriggerEntity).WeaponType;

        //Make sure Player doesnt already have this weapon
        if (GameVariables.Player.PlayerCurrentWeapons.Contains(type))
        {
#if UNITY_EDITOR
            Debug.Log($"Player already has weapon {type}, not adding new one...");
#endif
        }
        else
            //Add to player inventory
            GameVariables.Player.PlayerCurrentWeapons.Add(type);

        //Destroy Interactable
        manager.DestroyEntity(info.TriggerEntity);
    }

    private static void OnWalkOverDeepHole(InteractableInfo info)
    {
        //Make sure entity has a LifeComponent
        if (!manager.HasComponent<LifeComponent>(info.CollidedEntity))
            return;

        //Get LifeComponent of Entity
        LifeComponent life = manager.GetComponentData<LifeComponent>(info.CollidedEntity);

        //Kill and set back
        life.Life.Value = 0;
        manager.SetComponentData(info.CollidedEntity, life);
    }

    private static void OnWinningPortal(InteractableInfo info)
    {
        //Start winning system
        Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<WinningSystem>().Enabled = true;
    }
}