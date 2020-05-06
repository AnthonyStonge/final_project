using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
public enum GridChange
{
    Wall,
    PreSpawn,
    MobSpawn
}
public class GridDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public GridChange gridChange; 
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
            int indexGrid = (int)(hit.point.x + 0.5f) + (int)(hit.point.z + 0.5f) * grid.gridSize.x;
            //int bob = (int)hit.point.x + (int)hit.point.z * grid.gridSize.x;
            if(gridChange == GridChange.Wall)
            {
                if (e.shift)
                    if (!grid.indexNoWalkable.Contains(indexGrid))
                        grid.indexNoWalkable.Add(indexGrid);
                if (e.control)
                    if (grid.indexNoWalkable.Contains(indexGrid))
                        grid.indexNoWalkable.Remove(indexGrid);
            }
            else if (gridChange == GridChange.PreSpawn)
            {
                if (e.shift)
                    if (!grid.enemyPreSpawnerIndex.Contains(indexGrid))
                    {
                        grid.enemyPreSpawner.Add(new EnemyPreSpawnerInfo
                        {
                            spawnerPos = new int2((int) (hit.point.x + 0.5f), (int) (hit.point.z + 0.5f)),
                        });
                        grid.enemyPreSpawnerIndex.Add(indexGrid);
                    }
                if (e.control)
                    if (grid.enemyPreSpawnerIndex.Contains(indexGrid))
                    {
                        grid.enemyPreSpawner.RemoveAt(grid.enemyPreSpawnerIndex.FindIndex(x => x == indexGrid));
                        grid.enemyPreSpawnerIndex.Remove(indexGrid);

                    }
            }
            else
            {
                if (e.shift)
                    if (!grid.enemySpawnerIndex.Contains(indexGrid))
                    {
                        grid.enemySpawner.Add(new EnemySpawnerInfo
                        {
                            spawnerPos = new int2((int) (hit.point.x + 0.5f), (int) (hit.point.z + 0.5f))
                        });
                        grid.enemySpawnerIndex.Add(indexGrid);
                    }
                if (e.control)
                    if (grid.enemySpawnerIndex.Contains(indexGrid))
                    {
                        grid.enemySpawner.RemoveAt(grid.enemySpawnerIndex.FindIndex(x => x == indexGrid));
                        grid.enemySpawnerIndex.Remove(indexGrid);

                    }
            }
        }

        bool cycle = false;
        for (int i = 0; i < grid.gridSize.x; i++)
        {
            for (int j = 0; j < grid.gridSize.y; j++)
            {
                if (grid.indexNoWalkable.Contains(i + j * grid.gridSize.x))
                {
                    Gizmos.color = Color.red;
                }
                else if (grid.enemyPreSpawnerIndex.Contains(i + j * grid.gridSize.x))
                {
                    Gizmos.color = Color.blue;
                }
                else if (grid.enemySpawnerIndex.Contains(i + j * grid.gridSize.x))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    if((i + j) % 2 == 0)
                        Gizmos.color = Color.white;
                    else
                        Gizmos.color = Color.grey;
                    cycle = false;
                }
                Gizmos.DrawCube( new Vector3(i,0.1f,j), new Vector3(grid.nodeSize.x,0.1f, grid.nodeSize.z));
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
