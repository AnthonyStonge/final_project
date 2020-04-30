using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class GridAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public ScriptableGrid grid;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Debug.Log(grid.indexNoWalkable.Count);
        GameVariables.grid = grid;
    }
}
