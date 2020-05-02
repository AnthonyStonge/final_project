using System.Collections.Generic;
using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct BatchFilter : ISharedComponentData
{
    public ushort Value;
}
[DisableAutoCreation]
[UpdateBefore(typeof(PathFollowSystem))]
public class PathFinding : SystemBase
{
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
    public ushort batchCall;
    //Cost of movement
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private int2 gridSize;
    private float3 nodeSize;
    private NativeArray<Node> nodeArray;
    private List<int> wall;
    private NativeArray<int2> neightBourOffsetArray;
    private EntityQueryDesc queryDesc;
    protected override void OnCreate()
    {
        
        batchCall = 0;
        queryDesc = new EntityQueryDesc
        {
            All = new ComponentType[] {typeof(BatchFilter),typeof(Translation), typeof(PathFindingComponent), typeof(PathFollowComponent)}
        };
        neightBourOffsetArray = new NativeArray<int2>(8, Allocator.Persistent);
        neightBourOffsetArray[0] = new int2(-1, 0);
        neightBourOffsetArray[1] = new int2(1, 0);
        neightBourOffsetArray[2] = new int2(0, 1);
        neightBourOffsetArray[3] = new int2(0, -1);
        neightBourOffsetArray[4] = new int2(-1, -1);
        neightBourOffsetArray[5] = new int2(-1, 1);
        neightBourOffsetArray[6] = new int2(1, -1);
        neightBourOffsetArray[7] = new int2(1, 1);
        wall = GameVariables.grid.indexNoWalkable;
        gridSize = GameVariables.grid.gridSize;
        nodeSize = GameVariables.grid.nodeSize;
        nodeArray = new NativeArray<Node>(gridSize.x * gridSize.y, Allocator.Persistent);
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                bool iswalkable = !wall.Contains(CalculateIndex( i, j, gridSize.x));
                
                Node node = new Node
                {
                    x = i,
                    y = j,
                    isWalkable = iswalkable,
                    gCost = int.MaxValue,
                    index = CalculateIndex(i, j, gridSize.x),
                    cameFromNodeIndex = -1
                };
                nodeArray[node.index] = node;
            }
        }
    }

    protected override void OnUpdate()
    {
        int2 _gridSize = gridSize;
        NativeArray<Node> nodeArray = this.nodeArray;
        NativeArray<int2> neightBourOffsetArrayJob = neightBourOffsetArray;
        Entities.WithSharedComponentFilter(new BatchFilter{Value = batchCall++}).ForEach((DynamicBuffer<PathPosition> pathBuffer, ref Translation translation, ref PathFindingComponent pathFindingComp, ref PathFollowComponent pathFollow) =>
        {
            if(pathFollow.ennemyState == EnnemyState.Chase)
            {
                if (nodeArray[CalculateIndex(pathFindingComp.endPos.x, pathFindingComp.endPos.y, 100)].isWalkable && IsPositionInsideGrid(pathFindingComp.endPos, _gridSize))
                {
                    int2 tmp = new int2((int) translation.Value.x, (int) translation.Value.z);
                    if (nodeArray[CalculateIndex(tmp.x, tmp.y, _gridSize.x)].isWalkable && IsPositionInsideGrid(tmp, _gridSize))
                    {
                        pathFindingComp.startPos = tmp;
                    }
                    FindPath(pathFindingComp.startPos, pathFindingComp.endPos, ref pathFindingComp.findPath, pathBuffer, ref pathFollow, _gridSize, nodeArray, neightBourOffsetArrayJob);
                }
            }
        }).ScheduleParallel();
        
        batchCall %= 8;
        
    }

    private static void FindPath(in int2 startPos, in int2 endPos, ref int pathFind, DynamicBuffer<PathPosition> pathBufferPos,ref PathFollowComponent pathFollowComponent, in int2 gridSize, in NativeArray<Node> nodeArray, in NativeArray<int2> neightBourOffsetArrayJob)
    {
        
        NativeArray<Node> pathNode = new NativeArray<Node>(gridSize.x * gridSize.y, Allocator.Temp);
        
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Node basicNode = nodeArray[CalculateIndex(i, j, gridSize.x)];
                Node node = new Node
                {
                    x = basicNode.x,
                    y = basicNode.y,
                    isWalkable = basicNode.isWalkable,
                    index = basicNode.index,
                    gCost = basicNode.gCost,
                    hCost = CalculateDistanceCost(new int2(i, j), endPos),
                    cameFromNodeIndex = -1
                };
                //node.CalculFCost();
                pathNode[node.index] = node;
            }
        }
        
        int endNodeIndex = CalculateIndex(endPos.x, endPos.y, gridSize.x);
        Node startNode = pathNode[CalculateIndex(startPos.x, startPos.y, gridSize.x)];
        startNode.gCost = 0;
        //startNode.CalculFCost();
        pathNode[startNode.index] = startNode;
        NativeList<int> openList = new NativeList<int>(Allocator.Temp);
        NativeList<int> closedList = new NativeList<int>(Allocator.Temp);
        openList.Add(startNode.index);

        while (openList.Length > 0)
        {
            int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNode, gridSize.x);
            Node currentNode = pathNode[currentNodeIndex];
            if (currentNodeIndex == endNodeIndex)
                break;
            for (int i = 0; i < openList.Length; i++)
            {
                if (openList[i] == currentNodeIndex)
                {
                    openList.RemoveAtSwapBack(i);
                    break;
                }
            }
            closedList.Add(currentNodeIndex);

            for (int i = 0; i < neightBourOffsetArrayJob.Length; i++)
            {
                int2 neightbourOffSet = neightBourOffsetArrayJob[i];
                int2 neightbourPos = new int2(currentNode.x + neightbourOffSet.x,
                    currentNode.y + neightbourOffSet.y);
                if (!IsPositionInsideGrid(neightbourPos, gridSize))
                    continue;
                int tmp = CalculateIndex(neightbourPos.x, neightbourPos.y, gridSize.x);
                if (closedList.Contains(tmp))
                    continue;
                Node neightbourNode = pathNode[tmp];
                if (!pathNode[tmp].isWalkable)
                    continue;
                int2 currentNodePos = new int2(currentNode.x, currentNode.y);
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePos, neightbourPos);
                if (tentativeGCost < neightbourNode.gCost)
                {
                    //Debug.Log(neightbourNode.index);
                    neightbourNode.cameFromNodeIndex = currentNodeIndex;
                    neightbourNode.gCost = tentativeGCost;
                    //neightbourNode.CalculFCost();
                    pathNode[tmp] = neightbourNode;
                    if (!openList.Contains(neightbourNode.index))
                    {
                        openList.Add(neightbourNode.index);
                    }
                }
            }
        }
        pathBufferPos.Clear();
        Node endNode = pathNode[endNodeIndex];
        if (endNode.cameFromNodeIndex == -1)
        {
            pathFollowComponent= new PathFollowComponent
            {
                pathIndex = -1
            };
        }
        else
        {
            pathFind = 1;
            CalculatePath(pathNode, endNode, pathBufferPos);
            pathFollowComponent.pathIndex = pathBufferPos.Length - 1;
        }
        pathNode.Dispose();
        openList.Dispose();
        closedList.Dispose();
    }
    private static void CalculatePath(in NativeArray<Node> pathNodes, in Node endNode, DynamicBuffer<PathPosition> pathPos)
    {
        if(endNode.cameFromNodeIndex != -1)
        {
            pathPos.Add( new PathPosition{position = new int2(endNode.x, endNode.y)});
            Node currentNode = endNode;
            while (currentNode.cameFromNodeIndex != -1)
            {
                Node cameFromNode = pathNodes[currentNode.cameFromNodeIndex];
                pathPos.Add(new PathPosition{ position = new int2(cameFromNode.x, cameFromNode.y)});
                currentNode = cameFromNode;
            }
        }
    }
    private static bool IsPositionInsideGrid(in int2 gridPos, in int2 gridSize)
    {
        return gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < gridSize.x && gridPos.y < gridSize.y;
    }
    public static int CalculateIndex(in int x, in int y, in int gridWith)
    {

        return (x + y * gridWith);
    }
    private static int CalculateDistanceCost(in int2 aPos, in int2 bPos)
    {
        int xDistance = math.abs(aPos.x - bPos.x);
        int yDistance = math.abs(aPos.y - bPos.y);
        int remaining = math.abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    private static int GetLowestCostFNodeIndex(in NativeList<int> openList, in NativeArray<Node> pathNode, in int2 gridSize)
    {
        Node lowestCost = pathNode[openList[0]];
        for (int i = 0; i < openList.Length; i++)
        {
            Node testNode = pathNode[openList[i]];
            if (testNode.FCost < lowestCost.FCost)
            {
                lowestCost = pathNode[openList[i]];
            }
        }
        return CalculateIndex(lowestCost.x, lowestCost.y, gridSize.x);
    }
    protected override void OnDestroy()
    {
        nodeArray.Dispose();
        neightBourOffsetArray.Dispose();
    }
}