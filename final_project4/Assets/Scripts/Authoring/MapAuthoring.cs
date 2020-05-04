using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class MapAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public MapType CurrentLevel;
    public Transform SpawnPos;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    { 
        if (!MapHolder.MapsInfo.ContainsKey(CurrentLevel))
            MapHolder.MapsInfo.Add(CurrentLevel, new MapInfo());

        MapHolder.MapsInfo[CurrentLevel].SpawnPosition = SpawnPos.position;
    }
}
