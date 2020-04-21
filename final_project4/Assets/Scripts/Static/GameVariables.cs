using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public static class GameVariables
{
    public static bool InputEnabled = true;
    public static EntityManager EntityManager;
    public static Camera MainCamera;
    public static Transform MouseToTransform;

    public static GameState StartingState = GameState.INTRO;
    
    public static class Player
    {
        //Player general infos (Can change during gameplay)
        public static Entity Entity;

        public static Dictionary<WeaponType, Entity> PlayerWeaponEntities = new Dictionary<WeaponType, Entity>();
        public static WeaponType CurrentWeaponHeld;

        //Unity linker
        public static Transform Transform;
    }
}
