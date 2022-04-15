using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
public class Testing : MonoBehaviour
{
    public struct LineDrawer
    {
        private LineRenderer lineRenderer;
        private float lineSize;

        public LineDrawer(float lineSize = 0.2f)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }

        private void init(float lineSize = 0.2f)
        {
            if (lineRenderer == null)
            {
                GameObject lineObj = new GameObject("LineObj");
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                //Particles/Additive
                lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

                this.lineSize = lineSize;
            }
        }

        //Draws lines through the provided vertices
        public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
        {
            if (lineRenderer == null)
            {
                init(0.2f);
            }

            //Set color
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;

            //Set width
            lineRenderer.startWidth = lineSize;
            lineRenderer.endWidth = lineSize;

            //Set line count which is 2
            lineRenderer.positionCount = 2;

            //Set the postion of both two lines
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }

        public void Destroy()
        {
            if (lineRenderer != null)
            {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }
    [SerializeField]
    private Pathfinding pathfinding;

    Vector3 start, end;
    [SerializeField]
    bool showDebug = true;
    void Start()
    {
        pathfinding = new Pathfinding(10, 10, 10f, Vector3.zero);

        

    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = UtilsClass.GetMouseWorldPosition3D(LayerMask.GetMask("Ground"));
            //Debug.Log(pathfinding);
            //Debug.Log(pathfinding.GetGrid());
            pathfinding.GetGrid().GetXZ(mouseWorldPos, out int x, out int z);
            //Debug.Log("Clicked on " + x + " " + z);
            List<PathNode> path = pathfinding.FindPath(0, 0, x, z);

            //Debug.Log("CLicked line : " + path.Count);
            if (path !=  null)
            {
                Debug.Log("Clicked " + x + ", " + z);
                for (int i = 0; i < path.Count - 1; i++)
                {
                    //Debug.Log("Drewline");
                    start = new Vector3(path[i].x, mouseWorldPos.y, path[i].y) * 10f + Vector3.one * 5f;
                    end = new Vector3(path[i + 1].x, mouseWorldPos.y, path[i + 1].y) * 10f + Vector3.one * 5f;
                    //Debug.Log($"Start: {start} \nend: {end}");

                    //lineDrawer.DrawLineInGameView(vec, end, Color.blue);
                    if (showDebug)
                        Debug.DrawLine(start, end, Color.blue, 4f);
                }
            }
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition3D(LayerMask.GetMask("Ground"));
            pathfinding.GetGrid().GetXZ(mousePos, out int x, out int z);
            pathfinding.GetNode(x, z).SetIsWalkable(!pathfinding.GetNode(x, z).isWalkable);
        }
    }
}
