
using System;

namespace Enums
{
    public enum WeaponType : byte
    {
        Pistol,
        Shotgun
    }

    public enum ProjectileType: byte
    {
        PistolBullet,
        ShotgunBullet
    }

    public enum Type : byte
    {
        Player,
        Pig,
        Rat,
        Chicken,
        Gorilla
    }

    public enum EnemyType : byte
    {
        Enemy
    }

    public enum PlayerType : byte
    {
        Player
    }

    public enum AudioSourceType : byte
    {
        BackgroundMusic,
        PlayerWeaponActionsPlay, //Shoot, Reload, Swap
        PlayerWeaponActionPOSO,
        PlayerActions,    //Take damage, Die
        EnemyWeaponActions, //Shoot
        EnemyActions,    //Take damage, Die
        BulletActionsPlay    //Hit
    }
    public enum DropType
    {
        //Life,
        Amunation
    }
}
