using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Grid
{

    [System.Serializable]
    public class Grid<TGridObject>
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private float cellSize;
        [SerializeField] private Vector3 originPosition;
        [SerializeField] private bool showDebug = true;

        float cellDiameter;

        int gridSizeX;
        int gridSizeY;

        private Vector3 offset;

        private TGridObject[,] gridArray;

        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        public int MaxSize
        {
            get
            {
                return width * height;
            }
        }
        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridSizeX = Mathf.RoundToInt(width / cellSize);
            gridSizeY = Mathf.RoundToInt(height / cellSize);

            offset = new Vector3(cellSize, cellSize, 0) * .5f;

            gridArray = new TGridObject[width, height];

            // creates the grid object 
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        debugTextArray[x, y] = Utils.UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

                OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
                };
            }
        }

        public Vector3 GetOffset()
        {
            return offset;
        }
        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public float GetCellSize()
        {
            return cellSize;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }


        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                //debugTextArray[x, y].text = gridArray[x, y].ToString();
                if (OnGridValueChanged != null)
                    OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            }
        }


        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return default(TGridObject);
            }
        }
        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

        // function to be called on value change
        public void TriggerGridObjectChanged(int x, int y)
        {
            if (OnGridValueChanged != null)
                OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    [System.Serializable]
    public class GridXZ<TGridObject>
    {

        private int width;
        private int height;
        private float cellSize;
        //[SerializeField] private LayerMask walkableMask;

        int gridSizeX;
        int gridSizeY;


        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }

        private Vector3 offset;

        private Vector3 originPosition;
        //public TerrainType[] walkableRegions; // for movementPenalty

        [SerializeField] private bool showDebug;
        [SerializeField] private bool showNumber;
        [SerializeField] private bool showGridBound;
        private TGridObject[,] gridArray;

        public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public int x;
            public int z;
        }

        // Constructor without any movement penalty
        public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            float gridworldsizeX = width * cellSize;
            float gridworldsizeY = height * cellSize;
            float nodediameter = cellSize / 2;
            // gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodediameter);
            gridSizeX = Mathf.RoundToInt(gridworldsizeX / nodediameter);
            gridSizeY = Mathf.RoundToInt(gridworldsizeY / nodediameter);

            offset = new Vector3(cellSize, 0, cellSize) * .5f;
            gridArray = new TGridObject[width, height];
            SetDebug(true, false, true);
            

            // creates the grid object 
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    gridArray[x, z] = createGridObject(this, x, z);
                }
            }

            //if (showDebug)
            //{
            //    TextMesh[,] debugTextArray = new TextMesh[width, height];

            //    for (int x = 0; x < gridArray.GetLength(0); x++)
            //    {
            //        for (int z = 0; z < gridArray.GetLength(1); z++)
            //        {
            //            if (showNumber)
            //            {
            //                debugTextArray[x, z] = Utils.UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
            //            }
            //            if (showGridBound)
            //            {
            //                Debug.DrawLine(GetWorldPosition(x, z) + Vector3.up, GetWorldPosition(x, z + 1) + Vector3.up, Color.blue, float.MaxValue);
            //                Debug.DrawLine(GetWorldPosition(x, z) + Vector3.up, GetWorldPosition(x + 1, z) + Vector3.up, Color.blue, float.MaxValue);
            //            }
            //        }
            //    }
            //    if (showGridBound)
            //    {
            //        Debug.DrawLine(GetWorldPosition(0, height) + Vector3.up, GetWorldPosition(width, height) + Vector3.up, Color.blue, float.MaxValue);
            //        Debug.DrawLine(GetWorldPosition(width, 0) + Vector3.up, GetWorldPosition(width, height) + Vector3.up, Color.blue, float.MaxValue);
            //    }

            //    OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            //    {
            //        debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            //    };
            //}
        }


        // constructor with movement penalty for different types of terrain
        public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, int, TGridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            float gridworldsizeX = width * cellSize;
            float gridworldsizeY = height * cellSize;
            float nodediameter = cellSize / 2;
            // gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodediameter);
            gridSizeX = Mathf.RoundToInt(gridworldsizeX / nodediameter);
            gridSizeY = Mathf.RoundToInt(gridworldsizeY / nodediameter);

            offset = new Vector3(cellSize, 0, cellSize) * .5f;
            gridArray = new TGridObject[width, height];

            //foreach (TerrainType region in walkableRegions)
            //{
            //    walkableMask.value |= region.terrainMask.value;
            //}

            // creates the grid object 
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    Vector3 worldPoint = GetWorldPosition(x, z) + offset;
                    //bool walkable = !(Physics.CheckSphere(worldPoint, nodediameter));
                    int movementPenalty = 0;

                    gridArray[x, z] = createGridObject(this, x, z, movementPenalty);
                }
            }

            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int z = 0; z < gridArray.GetLength(1); z++)
                    {
                        debugTextArray[x, z] = Utils.UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

                OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
                };
            }
        }


        public void SetDebug(bool showDebug, bool showNumber, bool showGrid)
        {
            this.showDebug = showDebug;
            this.showNumber = showNumber;
            this.showGridBound = showGrid;
        }
        public Vector3 GetOffset()
        {
            return offset;
        }
        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public float GetCellSize()
        {
            return cellSize;
        }

        // returns the position of the placement of grid
        public Vector3 GetPlacementPosition(Transform objectPos, int x, int z)
        {
            return GetWorldPosition(x, z) + GetOffset() + (Vector3.up * objectPos.localScale.y / 2);
        }

        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * cellSize + originPosition;
        }

        public Vector3 GetWorldPosition(int x, int y, int z)
        {
            return new Vector3(x, y, z) * cellSize + originPosition;
        }


        public void GetXZ(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
        }

        public void SetGridObject(int x, int z, TGridObject value)
        {
            if (isValidGrid(x,z))
            {
                gridArray[x, z] = value;
                //debugTextArray[x, y].text = gridArray[x, y].ToString();
                if (OnGridValueChanged != null)
                    OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
            }
        }


        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            SetGridObject(x, z, value);
        }

        public TGridObject GetGridObject(int x, int z)
        {
            if (isValidGrid(x,z))
            {
                return gridArray[x, z];
            }
            else
            {
                return default(TGridObject);
            }
        }

        public bool isValidGrid(int x, int z)
        {
            return x >= 0 && z >= 0 && x < width && z < height;
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetGridObject(x, z);
        }

        // function to be called on value change
        public void TriggerGridObjectChanged(int x, int z)
        {
            if (OnGridValueChanged != null)
                OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }

        public void ShowDebug()
        {
            if (showDebug)
            {
                TextMesh[,] debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int z = 0; z < gridArray.GetLength(1); z++)
                    {
                        if (showNumber)
                        {
                            debugTextArray[x, z] = Utils.UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                        }
                        if (showGridBound)
                        {
                            Debug.DrawLine(GetWorldPosition(x, z) + Vector3.up * .5f, GetWorldPosition(x, z + 1) + Vector3.up * .5f, Color.blue, float.MaxValue);
                            Debug.DrawLine(GetWorldPosition(x, z) + Vector3.up * .5f, GetWorldPosition(x + 1, z) + Vector3.up * .5f, Color.blue, float.MaxValue);
                        }
                    }
                }
                if (showGridBound)
                {
                    Debug.DrawLine(GetWorldPosition(0, height) + Vector3.up * .5f, GetWorldPosition(width, height) + Vector3.up * .5f, Color.blue, float.MaxValue);
                    Debug.DrawLine(GetWorldPosition(width, 0) + Vector3.up * .5f, GetWorldPosition(width, height) + Vector3.up * .5f, Color.blue, float.MaxValue);
                }

                OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
                {
                    debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
                };
            }
        }
    }
    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

}
