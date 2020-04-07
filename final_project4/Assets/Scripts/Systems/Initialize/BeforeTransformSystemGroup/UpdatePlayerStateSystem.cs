using Unity.Entities;

[DisableAutoCreation]
[UpdateAfter(typeof(PlayerTargetSystem))]
[UpdateAfter(typeof(DecrementTimeSystem))]
public class UpdatePlayerStateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        
    }
}
