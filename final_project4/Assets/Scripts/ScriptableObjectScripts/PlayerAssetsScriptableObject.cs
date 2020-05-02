using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player", fileName = "new Player")]
public class PlayerAssetsScriptableObject : ScriptableObject
{
    [Header("Variables")]
    public Translation DefaultSpawnPosition;
    public Rotation DefaultSpawnRotation;
    
    [Space(5)]
    public float DefaultSpeed = 20;
    public short DefaultHealth = 3;
    
    [Space(3)]
    public RenderMesh RenderMesh;    //TODO CHANGE FOR ANIMATION

    [Header("Internal Variables")] 
    public AudioSource PlayerAudioSource;

    [Header("Debug Variables")] 
    public bool UseDebugVariables = false;

    [Space(5)]
    public Translation StartingPosition;
    public Rotation StartingRotation;

    [Space(5)]
    public short StartingHealth = 1;

    //[Header("Inputs")] 
    //public InputComponent inputs;
}