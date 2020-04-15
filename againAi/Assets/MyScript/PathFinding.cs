using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
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
    //Cost of movement
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private int2 gridSize;
    private float3 nodeSize;
    private NativeArray<Node> nodeArray;
    private List<int> wall;
    protected override void OnCreate()
    {
        wall = GameVariable.Instance.scriptableGrid.indexNoWalkable;
        gridSize = GameVariable.Instance.scriptableGrid.gridSize;
        nodeSize = GameVariable.Instance.scriptableGrid.nodeSize;
        //nodeArray = new NativeArray<Node>();
        nodeArray = new NativeArray<Node>(gridSize.x * gridSize.y, Allocator.Persistent);// [scriptableGrid.gridSize.x * scriptableGrid.gridSize.y];
        //NativeArray<Node> pathNode = new NativeArray<Node>(gridSize.x * gridSize.y, Allocator.Temp);
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                bool iswalkable = !wall.Contains(i + j * gridSize.x);
                Node node = new Node
                {
                    x = i,
                    y = j,
                    isWalkable = iswalkable,
                    gCost = int.MaxValue,
                    index = CalculateIndex(i, j, gridSize.x),
                    cameFromNodeIndex = -1
                };
                //node.CalculFCost();
                nodeArray[node.index] = node;
            }
        }
    }
    protected override void OnUpdate()
    {
        //NativeList<JobHandle> jobHandlesList = new NativeList<JobHandle>(Allocator.Temp);
        int2 bob = gridSize;
        NativeArray<Node> nodeArray = this.nodeArray;
        Entities.ForEach((Entity e, DynamicBuffer<PathPosition> pathBuffer, ref Translation translation, ref PathFindingComponent pathFindingComp, ref PathFollow pathFollow) =>
            {
                if(pathFindingComp.findPath == 0)
                {
                    pathFindingComp.startPos = new int2((int) translation.Value.x,(int) translation.Value.z);
                    pathFindingComp.findPath = 1;
                    FindPath(pathFindingComp.startPos, pathFindingComp.endPos, e, pathBuffer,ref pathFollow, bob, nodeArray);
                    
                }
            }).ScheduleParallel();
            
    }
    private static void FindPath(int2 startPos, int2 endPos, Entity entity, DynamicBuffer<PathPosition> pathBufferPos,ref PathFollow pathFollow, in int2 gridSize, in NativeArray<Node> nodeArray)
    {
        NativeArray<Node> pathNode = new NativeArray<Node>(gridSize.x * gridSize.y, Allocator.Temp);
        //NativeArray<CreateGridSystem.Node> pathNode;// = nodeArray.;
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
        NativeArray<int2> neightBourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);

            neightBourOffsetArray[0] = new int2(-1, 0);
            neightBourOffsetArray[1] = new int2(1, 0);
            neightBourOffsetArray[2] = new int2(0, 1);
            neightBourOffsetArray[3] = new int2(0, -1);
            neightBourOffsetArray[4] = new int2(-1, -1);
            neightBourOffsetArray[5] = new int2(-1, 1);
            neightBourOffsetArray[6] = new int2(1, -1);
            neightBourOffsetArray[7] = new int2(1, 1);
            
            int endNodeIndex = CalculateIndex(endPos.x, endPos.y, gridSize.x);
            Node startNode = pathNode[CalculateIndex(startPos.x, startPos.y, gridSize.x)];
            startNode.gCost = 0;
            //startNode.CalculFCost();
            pathNode[startNode.index] = startNode;
            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> ClosedList = new NativeList<int>(Allocator.Temp);
            openList.Add(startNode.index);
            while (openList.Length > 0)
            {
                int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNode);
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
                ClosedList.Add(currentNodeIndex);
                for (int i = 0; i < neightBourOffsetArray.Length; i++)
                {
                    int2 neightbourOffSet = neightBourOffsetArray[i];
                    int2 neightbourPos = new int2(currentNode.x + neightbourOffSet.x,
                        currentNode.y + neightbourOffSet.y);
                    if (!IsPositionInsideGrid(neightbourPos, gridSize))
                        continue;
                    int tmp = CalculateIndex(neightbourPos.x, neightbourPos.y, gridSize.x);
                    if (ClosedList.Contains(tmp))
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
                
                pathFollow= new PathFollow
                {
                    
                    pathIndex = -1
                };
            }
            else
            {
                CalculatePath(pathNode, endNode, pathBufferPos);
                pathFollow = new PathFollow
                {
                    pathIndex = pathBufferPos.Length - 2
                };
            }
            pathNode.Dispose();
            openList.Dispose();
            ClosedList.Dispose();
            neightBourOffsetArray.Dispose();
       // }
    }
    private static void CalculatePath(NativeArray<Node> pathNodes, Node endNode, DynamicBuffer<PathPosition> pathPos)
    {
        if (endNode.cameFromNodeIndex != -1)
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
    private static bool IsPositionInsideGrid(int2 gridPos, int2 gridSize)
    {
        return gridPos.x >= 0 && gridPos.y >= 0 && gridPos.x < gridSize.x && gridPos.y < gridSize.y;
    }
    private static int CalculateIndex(int x, int y, int gridWith)
    {
        return x + y * gridWith;
    }
    private static int CalculateDistanceCost(int2 aPos, int2 bPos)
    {
        int xDistance = math.abs(aPos.x - bPos.x);
        int yDistance = math.abs(aPos.y - bPos.y);
        int remaining = math.abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    private static int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<Node> pathNode)
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
        return lowestCost.index;
    }
    protected override void OnDestroy()
    {
        nodeArray.Dispose();
    }
}