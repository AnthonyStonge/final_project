using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
[ExecuteInEditMode]
public class GameVariable : MonoBehaviour
{
    private static GameVariable instance = null;
 
 
    // A static property that finds or creates an instance of the manager object and returns it.
    public static GameVariable Instance
    {
        get
        {
            if (instance == null)
            {
                // FindObjectOfType() returns the first AManager object in the scene.
                instance = FindObjectOfType(typeof(GameVariable)) as GameVariable;
            }
 
            // If it is still null, create a new instance
            if (instance == null)
            {
                var obj = new GameObject("AManager");
                instance = obj.AddComponent<GameVariable>();
            }
 
            return instance;
        }
    }
    public ScriptableGrid scriptableGrid;
    public Object PrefabEnnemy;
    // Start is called before the first frame update
}
