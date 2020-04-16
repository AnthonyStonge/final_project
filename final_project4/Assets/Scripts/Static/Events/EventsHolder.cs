using Unity.Collections;
using EventStruct;

public static class EventsHolder
{
    //TODO DISPOSE WHEN CLOSING PROGRAM
    public static NativeList<BulletInfo> PistolBulletToShoot =
        new NativeList<BulletInfo>(Allocator.Persistent);
}