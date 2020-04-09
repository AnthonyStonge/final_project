using Unity.Entities;

public struct PistolComponent : IComponentData
{
    public int CurrentBulletInMagazine;
    public int MagasineSize;

    public TimeTrackerComponent BetweenShotTime;
    public TimeTrackerComponent ReloadTime;

    public bool CanShoot => CurrentBulletInMagazine > 0 && BetweenShotTime.Available;
}
