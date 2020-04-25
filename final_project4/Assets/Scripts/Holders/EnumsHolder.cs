
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

    public enum PlayerType
    {
        Player
    }

    public enum AudioSourceType
    {
        BackgroundMusic,
        PlayerWeaponActionsPlay, //Shoot, Reload, Swap
        PlayerWeaponActionPOSO,
        PlayerActions,    //Take damage, Die
        EnemyWeaponActions, //Shoot
        EnemyActions,    //Take damage, Die
        BulletActionsPlay    //Hit
    }
}
