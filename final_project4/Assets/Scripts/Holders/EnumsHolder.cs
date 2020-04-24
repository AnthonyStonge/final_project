
namespace Enums
{
    public enum WeaponType
    {
        Pistol,
        Shotgun
    }

    public enum ProjectileType
    {
        PistolBullet,
        ShotgunBullet
    }

    public enum EnemyType
    {
        Enemy
    }

    public enum VFXType
    {
        ON_PISTOL_SHOT
    }

    public enum PlayerType
    {
        Player
    }

    public enum AudioSourceType
    {
        BackgroundMusic,
        PlayerWeaponActions, //Shoot, Reload, Swap
        PlayerActions,    //Take damage, Die
        EnemyWeaponActions, //Shoot
        EnemyActions,    //Take damage, Die
        BulletActions    //Hit
    }
}
