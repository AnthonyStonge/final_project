using Unity.Entities;

public struct GunComponent : IComponentData
{
    public TimeTrackerComponent BetweenShotsTime;
    public TimeTrackerComponent ReloadTime;
    public int BulletAmountPerShot;
    public int MagasineSize;
}
