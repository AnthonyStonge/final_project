using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(RetrieveInteractableCollisionsSystem))]
public class InteractableEventSystem : SystemBase
{
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
                OnExitInputInteractable();
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
        }
    }

    private static void OnEnterInputInteractable(InteractableInfo info)
    {
        //Set Object in GameVariables
        GameVariables.Interactables.Info = info;
#if UNITY_EDITOR
        Debug.Log("Adding info to GameVariables");
#endif
    }

    private static void OnExitInputInteractable()
    {
        //Remove Object in GameVariables
        GameVariables.Interactables.Info = null;
#if UNITY_EDITOR
        Debug.Log("Removing info from GameVariables");
#endif
    }

    private static void OnEnterPortal(InteractableInfo info)
    {
#if UNITY_EDITOR
        Debug.Log("Entered Portal");
#endif
        //Get PortalComponent
        PortalData data =
            Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager
                .GetComponentData<PortalData>(info.Entity);

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
}