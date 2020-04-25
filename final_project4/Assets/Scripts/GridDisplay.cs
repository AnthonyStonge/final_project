using UnityEditor;
using UnityEngine;

public class GridDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public ScriptableGrid grid;

    public bool ShowGrid = false;
    // Update is called once per frame
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!ShowGrid)
                return;
        bool isHit = false;
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay( e.mousePosition );
        //grid = GameVariables.grid;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            int bob = (((int) hit.point.x <= 0) ? (int) hit.point.x : (int) hit.point.x + 1) + (((int) hit.point.z <= 0)?(int) hit.point.z:(int) hit.point.z + 1) * grid.gridSize.x;
            if (e.shift)
                if (!grid.indexNoWalkable.Contains(bob))
                    grid.indexNoWalkable.Add(bob);
            if (e.control)
                if (grid.indexNoWalkable.Contains(bob))
                    grid.indexNoWalkable.Remove(bob);
        }
        
        for (int i = -grid.gridSize.x / 2; i < grid.gridSize.x / 2; i++)
        {
            for (int j = -grid.gridSize.y / 2; j < grid.gridSize.y / 2; j++)
            {
                if (grid.indexNoWalkable.Contains(i + j * grid.gridSize.x))
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawCube( new Vector3(i - (grid.nodeSize.x / 2),0,j  - (grid.nodeSize.z / 2)), new Vector3(grid.nodeSize.x, grid.nodeSize.y, grid.nodeSize.z));
            }
        }
#endif
    }
}
