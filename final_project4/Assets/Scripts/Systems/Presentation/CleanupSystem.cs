using Unity.Entities;

[DisableAutoCreation]
[UpdateAfter(typeof(VisualEventSystem))]
public class CleanupSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EventsHolder.WeaponEvents.Clear();
        EventsHolder.BulletsEvents.Clear();
    }
}
