using Unity.Entities;
using UnityEngine;

public class GameLogic : IStateLogic
{
     public void Enable()
     {
         Debug.Log("Enable GameLogic Systems");
     }
 
     public void Disable()
     {
         Debug.Log("Disable GameLogic Systems");
     }
 
     public void Initialize()
     {
         //Level Creation logic
         //Entities creation logic
         Debug.Log("Initialize GameLogic Systems");
     }
 
     public void Destroy()
     {
         //Delete or reset everything tied to GameLogic
         Debug.Log("Destroy GameLogic Systems");
     }
 }