using Unity.Entities;
using UnityEngine;

public struct PlayerStateComponent : IComponentData
{
    //Movement key
    public KeyCode Forward_Key;
    public KeyCode Backward_Key;
    public KeyCode Left_Key;
    public KeyCode Right_Key;

    //Attack key
    public KeyCode Attack_Key;
    public KeyCode Reload_Key;

    //Abilities key
    public KeyCode Dash_Key;
}