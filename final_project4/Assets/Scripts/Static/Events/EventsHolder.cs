using Unity.Collections;
using EventStruct;

public static class EventsHolder
{
    public static NativeList<WeaponInfo> WeaponEvents =
        new NativeList<WeaponInfo>(Allocator.Persistent);
    
    public static NativeList<BulletInfo> BulletsEvents =
        new NativeList<BulletInfo>(Allocator.Persistent);
    

    public static void OnDestroy()
    {
        WeaponEvents.Dispose();
        BulletsEvents.Dispose();
    }
}