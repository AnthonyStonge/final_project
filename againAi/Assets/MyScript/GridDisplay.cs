using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    private ScriptableGrid grid;
    void Start()
    {
        grid = GameVariable.Instance.scriptableGrid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        bool isHit = false;
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay( e.mousePosition );
        grid = GameVariable.Instance.scriptableGrid;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        RaycastHit hit;
        if (e.button == 0)
        if (Physics.Raycast(ray, out hit))
        {
            isHit = true;
            int bob = (int)hit.point.x + (int)hit.point.z * grid.gridSize.x;
            if (e.shift)
                if (!grid.indexNoWalkable.Contains(bob))
                    grid.indexNoWalkable.Add(bob);
            if (e.control)
                if (grid.indexNoWalkable.Contains(bob))
                    grid.indexNoWalkable.Remove(bob);
                
            
        }
        /*if (e.shift)
        {
            Debug.Log("mousefckDown");
        }*/
        for (int i = 0; i < grid.gridSize.x; i++)
        {
            for (int j = 0; j < grid.gridSize.y; j++)
            {
                if (grid.indexNoWalkable.Contains(i + j * grid.gridSize.x))
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                //Gizmos.DrawCube(new Vector3(((grid.nodeSize.x / 2f) * i) + (grid.nodeSize.x / 2f),0 ,(j * (grid.nodeSize.z / 2f)) + (grid.nodeSize.z / 2f)),new Vector3(grid.nodeSize.x, grid.nodeSize.y, grid.nodeSize.z));
                Gizmos.DrawCube( new Vector3(i ,0,j ), new Vector3(grid.nodeSize.x, grid.nodeSize.y, grid.nodeSize.z));
                //Handles.Label(new Vector3(i,1,j), (i + j * grid.gridSize.x).ToString(), style);
            }
        }
    }
}
