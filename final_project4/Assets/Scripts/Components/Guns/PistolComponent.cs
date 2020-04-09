using System;
using Unity.Entities;

[Serializable]
public struct PistolComponent : IComponentData
{
    public int CurrentBulletInMagazine;

    public float BetweenShotTime;
    public float ReloadTime;

    public Entity bullet;

    public bool IsBetweenShot => BetweenShotTime > 0;
    public bool IsReloading => ReloadTime > 0 ;
}
