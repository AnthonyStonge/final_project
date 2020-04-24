using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[ExecuteInEditMode]
public class CreateGridSystem : MonoBehaviour
{
    public static bool[] isWalkable;
    private ScriptableGrid scriptableGrid;
    protected void Start()
    {
        scriptableGrid = GameVariable.Instance.scriptableGrid;
        isWalkable = new bool[scriptableGrid.gridSize.x * scriptableGrid.gridSize.y];
        for (int i = 0; i < scriptableGrid.gridSize.x; i++)
        {
            for (int j = 0; j < scriptableGrid.gridSize.y; j++)
            {
                isWalkable[i+j*scriptableGrid.gridSize.x] = true;
            }
        }
    }
   
}
