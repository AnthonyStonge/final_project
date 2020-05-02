
using System;

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

    public enum Type
    {
        Player,
        Pig,
        Rat,
        Chicken,
        Gorilla
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
    public enum DropType
    {
        //Life,
        Amunation
    }

    //Order is IMPORTANT (The bigger the value, the more important it is for the StateMachine)
    public enum State
    {
        Idle,
        Running,
        Attacking,
        Reloading,
        Dashing,
        Dying,
        Respawning
    }

    public enum InteractableType
    {
        UseOnInput,
        UseOnEnterZone
    }

    public enum InteractableObjectType
    {
        Switch,
        Portal,
        Ammo
    }
    public enum EnnemyState
    {
        Wondering,
        Attack,
        Chase
    }
}
