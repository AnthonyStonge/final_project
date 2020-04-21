using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class CinemachineAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public CinemachineTargetGroup cinemachineTargetGroup;
    public Camera MainCam;
    public string PlayerObjectName = "PlayerTransform";
    public string CursorObjectName = "CursorTransform";

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //TODO Should be initialized elsewhere
        GameObject player = new GameObject(PlayerObjectName);
        GameObject cursor = new GameObject(CursorObjectName);

        GameVariables.Player.Transform = player.transform;
        GameVariables.MouseToTransform = cursor.transform;

        cinemachineTargetGroup.AddMember(GameVariables.Player.Transform, 2, 0);
        cinemachineTargetGroup.AddMember(GameVariables.MouseToTransform, 1, 0);

        GameVariables.MainCamera = MainCam;
    }
}
