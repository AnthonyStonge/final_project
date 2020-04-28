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
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay( e.mousePosition );
        //grid = GameVariables.grid;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            int bob = (((int) hit.point.x < 0) ? (int) hit.point.x: (int) hit.point.x) + (((int) hit.point.z < 0)?(int) hit.point.z:(int) hit.point.z) * grid.gridSize.x;
            bob += 5050;
            //int bob = (int)hit.point.x + (int)hit.point.z * grid.gridSize.x;
            if (e.shift)
                if (!grid.indexNoWalkable.Contains(bob))
                    grid.indexNoWalkable.Add(bob);
            if (e.control)
                if (grid.indexNoWalkable.Contains(bob))
                    grid.indexNoWalkable.Remove(bob);
        }

        bool cycle = false;
        for (int i = -grid.gridSize.x / 2; i < grid.gridSize.x / 2; i++)
        {
            for (int j = -grid.gridSize.y / 2; j < grid.gridSize.y / 2; j++)
            {
                if (grid.indexNoWalkable.Contains((i + 50) + (j + 50) * grid.gridSize.x))
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                    cycle = false;
                }
                Gizmos.DrawCube( new Vector3(i,0,j), new Vector3(grid.nodeSize.x,0f, grid.nodeSize.z));
            }

            if (cycle)
            {
                cycle = false;
            }
            else
            {
                cycle = true;
            }
        }
#endif
    }
}
