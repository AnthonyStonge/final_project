using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Map/MapContainer")]
public class MapContainerScriptable : ScriptableObject
{
    public List<MapObjectLinks> Links;

    [Serializable]
    public struct MapObjectLinks
    {
        public MapType Type;
        public GameObject Prefab;
    }
}

public enum MapType
{
    LevelMenu,
    Level_01,
    Level_02,
    Level_03,
    Level_04,
    Level_05,
    Level_Hell
}