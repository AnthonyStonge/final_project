using Unity.Entities;

public class PistolComponent : IComponentData
{
    public int CurrentBulletInMagazine;
    public int MagasineSize;

    public TimeTrackerComponent BetweenShotTime;
    public TimeTrackerComponent ReloadTime;

    public bool CanShoot => this.CurrentBulletInMagazine > 0 && BetweenShotTime.Available;
}
