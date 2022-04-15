using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;
//using System.Diagnostics;
using System;

[System.Serializable]
public class Pathfinding
{
    private const int MOVE_STRAIGHT_COOST = 10;
    private const int MOVE_DIAGNAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    [SerializeField]
    private GridXZ<PathNode> grid;

    private Heap<PathNode> openList; // openlists are the ones to be searched
    //private List<PathNode> closedList; // the ones already searched
    private HashSet<PathNode> closedList;
    bool is2D;
    public Pathfinding(int width, int height, float cellSize, Vector3 originPos)
    {
        Instance = this;
        
        grid = new GridXZ<PathNode>(width, height, cellSize, originPos, (GridXZ<PathNode> g, int x, int y) => new PathNode(g, x, y));

    }

    public GridXZ<PathNode> GetGrid()
    {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 endWorldPos)
    {
        grid.GetXZ(startWorldPos, out int startX, out int startY);
        grid.GetXZ(endWorldPos, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        } else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path)
            { // GetWorldPosition(x, z) + GetOffset() 
                vectorPath.Add(grid.GetOffset() + grid.GetWorldPosition(pathNode.x, pathNode.y));
            }
            return vectorPath;

        }
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        Debug.Log("MaxSize: " + grid.MaxSize);
        openList = new Heap<PathNode>(grid.MaxSize);
        closedList = new HashSet<PathNode>();

        openList.Add(startNode);
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;

            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            //PathNode currentNode = GetLowestFCostNode(openList);
            PathNode currentNode = openList.RemoveFirst();
            if (currentNode == endNode)
            {
                // reached final node
                return CalculatePath(endNode);
            }

            closedList.Add(currentNode);
            

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) 
                { 
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode) + neighbourNode.movementPenalty;

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    } else
                    {
                        openList.UpdateItem(neighbourNode);
                    }
                }
            }

        }
        // could not find the path or out of nodes on the openlist
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGNAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COOST * remaining;
    }

    //private PathNode GetLowestFCostNode(Heap<PathNode> pathNodeList)
    //{
    //    PathNode lowestFCostNode = pathNodeList.RemoveFirst();

    //    for (int i = 1; i < pathNodeList.Count; i++)
    //    {
    //        if (pathNodeList[i].fCost < lowestFCostNode.fCost)
    //        {
    //            lowestFCostNode = pathNodeList[i];
    //        }
    //    }
    //    return lowestFCostNode;
    //}
}
