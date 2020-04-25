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
        public string KeyName;
        public Type type;
        public GameObject Prefab;
    }
}