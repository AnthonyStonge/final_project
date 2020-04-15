using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public class PathFollowSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float time = Time.DeltaTime;
        float3 playerPosition = float3.zero;
        Entities.WithAll<PlayerTag>().ForEach((ref Translation translation) => { playerPosition = translation.Value; });
        Entities.ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation, ref PathFollow pathFollow, ref PathFindingComponent pathFindingComponent) =>
        {
            if (pathFollow.pathIndex > 0)
            {
                int2 pathPosition = pathPos[pathFollow.pathIndex].position;
                float3 targetPos = new float3(pathPosition.x, 0, pathPosition.y);
                float3 moveDir = math.normalizesafe(targetPos - translation.Value);
                float moveSpeed = 3f;
                translation.Value += moveDir * moveSpeed * time;
                if (math.distance(translation.Value, targetPos) < .1f)
                {
                    pathFollow.pathIndex--;
                }
            }
            else
            {
                pathFindingComponent.endPos = new int2((int)playerPosition.x,(int)playerPosition.z);
                pathFindingComponent.findPath = 0;
            }
        });
    }
}
