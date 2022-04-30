using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;
using Utils;

public class GridBuildingSystem : MonoBehaviour
{

    //public static GridBuildingSystem Instance;

    [SerializeField]
    private List<PlacedObjects> placedObjectsList;
    private PlacedObjects placedObject;
    private GameObject visual;

    [SerializeField]
    private GridXZ<GridObject> grid;

    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float cellSize = 10f;
    [SerializeField]
    private Vector3 originPos;
    public bool showDebug;
    public bool showGridNum;
    public bool showGridBound;
    [SerializeField] private LayerMask layer;

    private PlacedObjects.Dir currentDirection = PlacedObjects.Dir.Down;

    bool buildReady = true;

    private void Awake()
    {
        //Instance = this;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, originPos, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        grid.SetDebug(showDebug, showGridNum, showGridBound);
        grid.ShowDebug();
        placedObject = placedObjectsList[0];
        visual = placedObject.visual.gameObject;
        visual = Instantiate(placedObject.visual.gameObject);
        visual.SetActive(false);

    }

    public class GridObject
    {
        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        private Transform transform;


        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void SetTransform(Transform transf)
        {
            this.transform = transf;
            grid.TriggerGridObjectChanged(x, z);
        }

        public void ClearTransform()
        {
            transform = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild()
        {
            return transform == null;
        }

        public bool IsWithinGrid()
        {
            //grid.GetWidth() * grid.GetCellSize() + 
            return true;
        }
        public override string ToString()
        {
            return x + ", " + z + "\n" + transform;
        }

    }

    private void PlaceObjectPlacement()
    {
        if (Input.GetMouseButtonDown(1) && placedObject != null && !UtilsClass.IsPointerOverUI())
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition3D(layer);
            grid.GetXZ(mousePos, out int x, out int z);

            //Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            //if (
            //    )
        }
    }
    private void FixedUpdate()
    {
        if (buildReady)
        {
            Vector3 mousepos = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousepos, out int x, out int z);
            //Debug.Log("X, Z:" + x + ", " + z);
            //List<Vector2Int> gridPosList = placedObject.GetGridPositionList(new Vector2Int(x, z), currentDirection);
            if (grid.isValidGrid(x, z))
            {
                Debug.Log($"{gameObject.name} : {grid.GetGridObject(x, z).CanBuild()}");
                if (grid.GetGridObject(x, z).CanBuild())
                {
                    Debug.Log("Can build");
                    visual.gameObject.SetActive(true);
                    Vector2Int rotOffset = placedObject.GetRotationOffset(currentDirection);
                    //Vector3 placedObjectWorldPos = grid.GetPlacementPosition(placedObject.visual, x, z);
                    //+ new Vector3(rotOffset.x, 0, rotOffset.y) * grid.GetCellSize();
                    visual.transform.position = mousepos + Vector3.up;
                    visual.transform.rotation = Quaternion.Euler(0, placedObject.GetRotationAngle(currentDirection), 0);
                    //Transform builtTransform = Instantiate(placedObject.prefab,
                    //                                        placedObjectWorldPos,
                    //                                        Quaternion.Euler(0, placedObject.GetRotationAngle(currentDirection),
                    //                                        0));

                    //foreach (Vector2Int gridPos in gridPosList)
                    //{
                    //    grid.GetGridObject(gridPos.x, gridPos.y).SetTransform(builtTransform);
                    //}
                }

            }
            else
            {
                visual.SetActive(false);
            }
            //else
            //{
            //    visual.SetActive(false);

            //}
        }
        else
        {
            visual.SetActive(false);
        }
    }

    public void PlaceBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            buildReady = !buildReady;
            UtilsClass.CreateWorldTextPopup($"Build Mode: {buildReady}", Mouse3D.GetMouseWorldPosition());
        }


        if (Input.GetKeyDown(KeyCode.Mouse1) && buildReady)
        {
            buildReady = false;



            Vector3 mousePos = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePos, out int x, out int z);
            //Debug.Log("x and z: " + x + ", " + z);

            List<Vector2Int> gridPosList = placedObject.GetGridPositionList(new Vector2Int(x, z), currentDirection);

            // Test can build
            bool canBuild = true;
            foreach (Vector2Int gridPos in gridPosList)
            {
                if (!grid.GetGridObject(gridPos.x, gridPos.y).CanBuild())
                {
                    // if can't build here
                    canBuild = false;
                    break;
                }
            }


            //GridObject gridObject =  grid.GetGridObject(x, z);
            if (canBuild)
            {
                // Builds the Turret here

                //Debug.Log("Clicked: " + x + ", " + z);
                Vector2Int rotOffset = placedObject.GetRotationOffset(currentDirection);
                Vector3 placedObjectWorldPos = grid.GetPlacementPosition(placedObject.prefab, x, z);
                placedObjectWorldPos.y = 1.67f;
                Transform builtTransform = Instantiate(placedObject.prefab,
                                                        placedObjectWorldPos,
                                                        Quaternion.Euler(0, placedObject.GetRotationAngle(currentDirection),
                                                        0));

                foreach (Vector2Int gridPos in gridPosList)
                {
                    grid.GetGridObject(gridPos.x, gridPos.y).SetTransform(builtTransform);
                }
            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            currentDirection = PlacedObjects.GetNextDir(currentDirection);
            UtilsClass.CreateWorldTextPopup("" + currentDirection, Mouse3D.GetMouseWorldPosition());
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            buildReady = !buildReady;
            UtilsClass.CreateWorldTextPopup($"Build Mode: {buildReady}", Mouse3D.GetMouseWorldPosition());
        }


        if (Input.GetKeyDown(KeyCode.Mouse1) && buildReady)
        {
            buildReady = false;



            Vector3 mousePos = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePos, out int x, out int z);
            //Debug.Log("x and z: " + x + ", " + z);

            List<Vector2Int> gridPosList = placedObject.GetGridPositionList(new Vector2Int(x, z), currentDirection);

            // Test can build
            bool canBuild = true;
            foreach (Vector2Int gridPos in gridPosList)
            {
                if (!grid.GetGridObject(gridPos.x, gridPos.y).CanBuild())
                {
                    // if can't build here
                    canBuild = false;
                    break;
                }
            }


            //GridObject gridObject =  grid.GetGridObject(x, z);
            if (canBuild && MoneyManager.Instance.UseMoney(placedObject.cost))
            {
                // Builds the Turret here

                //Debug.Log("Clicked: " + x + ", " + z);
                Vector2Int rotOffset = placedObject.GetRotationOffset(currentDirection);
                Vector3 placedObjectWorldPos = grid.GetPlacementPosition(placedObject.prefab, x, z);
                placedObjectWorldPos.y = 1.67f;
                Transform builtTransform = Instantiate(placedObject.prefab,
                                                        placedObjectWorldPos,
                                                        Quaternion.Euler(0, placedObject.GetRotationAngle(currentDirection),
                                                        0));

                foreach (Vector2Int gridPos in gridPosList)
                {
                    grid.GetGridObject(gridPos.x, gridPos.y).SetTransform(builtTransform);
                }
            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Cannot build here!", Mouse3D.GetMouseWorldPosition());
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            currentDirection = PlacedObjects.GetNextDir(currentDirection);
            UtilsClass.CreateWorldTextPopup("" + currentDirection, Mouse3D.GetMouseWorldPosition());
        }
    }
}
