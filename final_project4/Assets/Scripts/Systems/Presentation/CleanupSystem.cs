using Unity.Entities;

[DisableAutoCreation]
[UpdateAfter(typeof(VisualEventSystem))]
public class CleanupSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EventsHolder.BulletsEvents.Clear();
    }
}
