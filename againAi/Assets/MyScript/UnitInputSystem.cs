using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
public class UnitInputSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        EntityQuery query = GetEntityQuery(typeof(UsePathFindingComp));
        EntityQuery queryPlayer = GetEntityQuery(typeof(PlayerTag));
        /*if (Input.GetMouseButtonDown(0))
        {
            Entities.With(query).ForEach((Entity entity) =>
            {
                EntityManager.AddComponentData(entity, new PathFindingComponent
                {
                    //startPos = new int2(0, 0),
                    endPos = new int2(Random.Range(0,20), Random.Range(0,20))
                });
            });
        }*/
        
            Entities.With(queryPlayer).ForEach((Entity e, ref Translation translation) =>
            {
                translation.Value += new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            });
        
    }
}
