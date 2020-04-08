﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class GameVariables
{

    public static bool InputEnabled = true;

    public static Camera MainCamera;
    
    public static class PlayerVars
    {
        public static float3 Position;
        public static float3 MousePos;
        public static bool IsDead;
    }

}
