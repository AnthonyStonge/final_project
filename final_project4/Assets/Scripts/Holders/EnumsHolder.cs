﻿
using System;

namespace Enums
{
    public enum WeaponType
    {
        Pistol = 0,
        Shotgun,
        RatWeapon,
        ChickenWeapon,
        PigWeapon,
        GorillaWeapon,
        Machinegun,
        HellPistol,
        HellShotgun,
        HellMachinegun
    }
    public enum DropType
    {
        AmmunitionShotgun = WeaponType.Shotgun,
        AmmunitionMachinegun = WeaponType.Machinegun
    }
    public enum ProjectileType
    {
        PistolBullet,
        ShotgunBullet,
        MachinegunBullet,
        RatBullet,
        ChickenBullet,
        PigBullet,
        GorillaBullet
    }

    public enum Type
    {
        Player,
        Pig,
        Rat,
        Chicken,
        Gorilla,
        Door
    }

    public enum PlayerType
    {
        Player
    }

    //These sounds are link to global event and generic event (NOT interactables/player/enemies)
    public enum SoundType
    {
        Backgrounds,
        Soundtracks,
        SFX
    }
    
    public enum AudioSourceType
    {
        Soundtrack,
        AmbienceMusic,
        AmbienceSFX,
        PlayerWeaponActionsPlay, //Shoot, Reload, Swap
        PlayerWeaponActionPOSO,
        PlayerActions,    //Take damage, Die
        EnemyWeaponActions, //Shoot
        EnemyActions,    //Take damage, Die
        BulletActionsPlay //Hit
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
        Respawning,
        DoorOpening,
        DoorClosing,
        Hit
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
        Ammo,
        Door,
        Weapon,
        DeepHole,
        WinningPortal,
        DoorClosing
    }
    public enum EnemyState
    {
        Wondering,
        Attack,
        Chase
    }
    public enum InvincibilityType{
        Hit,
        Spawn,
        Death,
        Dash,
        None
    }
}
