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
        //nodeArray = new NativeArray<Node>(scriptableGrid.gridSize.x * scriptableGrid.gridSize.y, Allocator.Persistent);// [scriptableGrid.gridSize.x * scriptableGrid.gridSize.y];
        //NativeArray<Node> pathNode = new NativeArray<Node>(gridSize.x * gridSize.y, Allocator.Temp);
        for (int i = 0; i < scriptableGrid.gridSize.x; i++)
        {
            for (int j = 0; j < scriptableGrid.gridSize.y; j++)
            {
                isWalkable[i+j*scriptableGrid.gridSize.x] = true;
            }
        }
    }
    protected void Update()
    {
        Ray v = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit r;
        if (Physics.Raycast(v,out r,100))
        {
            //Debug.Log((int)r.point.x + (int)r.point.z * scriptableGrid.gridSize.x);
           /* if (Input.GetMouseButton(0))
            {
                isWalkable[(int) r.point.x + (int) r.point.z * scriptableGrid.gridSize.x] = false;
            }*/
        }
    }
    [Serializable]
    public struct Node
    {
        public int x;
        public int y;
        public int index;
        public int gCost;
        public int hCost;
        public int FCost
        {
            get
            {
                return hCost + gCost;
            }
        }
        public bool isWalkable;
        public int cameFromNodeIndex;
    }
}
