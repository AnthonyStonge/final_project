using Enums;
using Unity.Entities;

public struct GunComponent : IComponentData
{
    public GunType GunType;

    public int CurrentAmountBulletInMagazine;
    public int CurrentAmountBulletOnPlayer;
    public int MaxBulletInMagazine;

    public float BetweenShotTime;
    public float ReloadTime;
    public float ResetReloadTime;
    public bool IsBetweenShot => BetweenShotTime > 0;
    public bool IsReloading => ReloadTime > 0;
}
