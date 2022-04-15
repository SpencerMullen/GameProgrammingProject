using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;
using System;

public class PathNode : IHeapItem<PathNode>
{
    
    private GridXZ<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public int movementPenalty;

    public bool isWalkable;
    public PathNode cameFromNode;
    int heapIndex;
    public PathNode(GridXZ<PathNode> grid, int x, int y, int penalty=0)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.movementPenalty = penalty;
        isWalkable = true;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
