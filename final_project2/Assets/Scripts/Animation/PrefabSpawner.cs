using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{

   public static SwordsManScriptable skeleton;

   public static RenderingData GetRenderingData(UnitType type)
   {
      return skeleton.renderingData;
   }
   
}
