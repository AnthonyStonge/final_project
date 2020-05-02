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
        Debug.Log("Adding info to GameVariables");
    }

    private static void OnExitInputInteractable()
    {
        //Remove Object in GameVariables
        GameVariables.Interactables.Info = null;
        Debug.Log("Removing info from GameVariables");
    }

    private static void OnEnterPortal(InteractableInfo info)
    {
        Debug.Log("Entered Portal");

        //Get PortalComponent
        PortalData data =
            Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager
                .GetComponentData<PortalData>(info.Entity);

        //Get PortalInfo
        MapInfo.Portal portal = MapHolder.MapsInfo[MapEvents.CurrentTypeLoaded].Portals[data.Value];

        if (!TryEnterPortal(portal.MapTypeLeadingTo, portal.PortalIdLeadingTo))
        {
            Debug.Log("The Portal ur trying to teleport to doesnt exist yet...");
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
            Debug.Log($"MapsInfo doesnt contain the MapType {mapType}...");
            return false;
        }
        else if (!MapHolder.MapsInfo[mapType].Portals.ContainsKey(portalId))
        {
            Debug.Log($"The MapType in MapsInfo doesnt contain the PortalId {portalId}...");
            return false;
        }
        //return MapHolder.MapsInfo.ContainsKey(mapType) && MapHolder.MapsInfo[mapType].Portals.ContainsKey(portalId);
        return true;
    }

    private static void OnWalkOverAmmo(InteractableInfo info)
    {
        Debug.Log("Walked Over Ammo");
    }
}