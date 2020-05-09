using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class InteractableAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public InteractableType Type;
    public InteractableObjectType ObjectType;

    public MapType CurrentMapType;

    [Header("Portal")]
    public ushort PortalId;
    public Transform PlayerTeleportPosition;

    [Space(5)] public MapType MapTypeLeadingTo;
    public ushort PortalIdLeadingTo;

    [Header("Door")] public GameObject DoorToOpen;

    [Header("Weapon")] public WeaponType WeaponType;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
#if UNITY_EDITOR
        dstManager.SetName(entity, "Interactable Object");
#endif

        dstManager.AddComponentData(entity, new InteractableComponent
        {
            Type = Type,
            ObjectType = ObjectType
        });

        //Add to Map portals
        if (ObjectType == InteractableObjectType.Portal)
        {
            //Create MapInfo for this MapType
            if (!MapHolder.MapsInfo.ContainsKey(CurrentMapType))
                MapHolder.MapsInfo.Add(CurrentMapType, new MapInfo());

            if (ReferenceEquals(PlayerTeleportPosition, null))
            {
                PlayerTeleportPosition = transform;
            }

            //Add info
            MapHolder.MapsInfo[CurrentMapType].Portals.Add(PortalId, new MapInfo.Portal
            {
                Id = PortalId,
                Position = PlayerTeleportPosition.position,
                Rotation = PlayerTeleportPosition.rotation,
                MapTypeLeadingTo = MapTypeLeadingTo,
                PortalIdLeadingTo = PortalIdLeadingTo
            });

            //Add Component to Entity
            dstManager.AddComponentData(entity, new PortalData
            {
                Value = PortalId
            });
        }

        if (ObjectType == InteractableObjectType.Door)
        {
            //If Type Door -> Make sure theres a door linked to it lol
            Assert.IsNotNull(DoorToOpen);

            dstManager.AddComponentData(entity, new InteractableComponent
            {
                Type = Type,
                ObjectType = ObjectType,
                DoorToOpen = conversionSystem.GetPrimaryEntity(DoorToOpen)
            });
            dstManager.AddSharedComponentData(entity, new AnimationBatch
            {
                BatchId = AnimationHolder.AddAnimatedObject()
            });
            dstManager.AddComponent<AnimationData>(entity);
        }

        if (ObjectType == InteractableObjectType.Weapon)
        {
            dstManager.AddComponentData(entity, new InteractableComponent
            {
                Type = Type,
                ObjectType = ObjectType,
                WeaponType = WeaponType
            });
        }

        if (ObjectType == InteractableObjectType.DoorClosing)
        {
            Entity door = conversionSystem.GetPrimaryEntity(DoorToOpen);
            
            dstManager.SetEnabled(door, false);
            
            dstManager.AddComponentData(entity, new InteractableComponent
            {
                Type = Type,
                ObjectType = ObjectType,
                DoorToOpen = door
            });
        }
    }
}