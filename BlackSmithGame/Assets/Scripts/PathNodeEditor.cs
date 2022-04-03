using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PathNodeEditor : MonoBehaviour
{
    static Color oragne = new Color(1f, 0.647f, 0f, 0.5f); // half transparent orange
    static Color red = new Color(1f, 0f, 0f, 1f);
    bool drawGizmos = true; // this should be global
    PathNode myNode;
    Mesh myMesh;

    /* ---- Verticies ----- */

    Vector3 topLeftVertex;
    Vector3 topRightVertex;
    Vector3 bottomLeftVertex;
    Vector3 bottomRightVertex;

    /* -------------------- */

    Vector3 lastPosition;

    Vector3 lastTopLeft;
    Vector3 lastTopRight;
    Vector3 lastBottomLeft;
    Vector3 lastBottomRight;

    PathNode topNeighbour = null;
    PathNode bottomNeighbour = null;
    PathNode leftNeighbour = null;
    PathNode rightNeighbour = null;

    private void Awake()
    {
        //TODO: destroy this if its not a debug version
        lastPosition = transform.position;
        myNode = GetComponent<PathNode>();
        myMesh = new Mesh();
        if(!GetComponent<MeshFilter>())
        {
            gameObject.AddComponent<MeshFilter>();
        }
        GetComponent<MeshFilter>().mesh = myMesh;

        AssignVertexOffsets();

        CalculateVerticies();

        UpdateMesh();
        myMesh.triangles = new int[] {0, 1, 2, 2, 1, 3};
        myMesh.RecalculateNormals();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(EditorApplication.isPlaying)
        {
            return;
        }

        if(!Selection.Contains(gameObject)) return; // use this later to optimise path node editor

        bool updateNeighbours = false;

        if(lastTopLeft != myNode.GetTopLeft()
        || lastTopRight != myNode.GetTopRight()
        || lastBottomLeft != myNode.GetBottomLeft()
        || lastBottomRight != myNode.GetBottomLeft()) // I guess just move stuff to on gizmo draw and update mesh in OnGizmos draw, seems more efficient
        {
            MeshUpdateWrapper();

            updateNeighbours = true;
        }

        if(lastPosition != transform.position)
        {
            updateNeighbours = true;
            lastPosition = transform.position;
        }

        UpdateNeighbourAssing(ref updateNeighbours);

        if(updateNeighbours)
        {
            UpdateNeighbours();
        }
    }

    private void OnDrawGizmos()
    {
        if(drawGizmos)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(topLeftVertex, topRightVertex);
            Gizmos.DrawLine(topLeftVertex, bottomLeftVertex);
            Gizmos.DrawLine(bottomLeftVertex, topRightVertex);
            Gizmos.DrawLine(topRightVertex, bottomRightVertex);
            Gizmos.DrawLine(bottomLeftVertex, bottomRightVertex);

            Gizmos.color = oragne;

            Gizmos.DrawMesh(myMesh);
        }
    }

    void AssignVertexOffsets()
    {
        lastTopLeft = myNode.GetTopLeft();
        lastTopRight = myNode.GetTopRight();
        lastBottomLeft = myNode.GetBottomLeft();
        lastBottomRight = myNode.GetBottomRight();
    }

    void CalculateVerticies()
    {
        topLeftVertex = lastTopLeft + transform.position;
        topRightVertex = lastTopRight + transform.position;
        bottomLeftVertex = lastBottomLeft + transform.position;
        bottomRightVertex = lastBottomRight + transform.position;
    }

    void UpdateMesh()
    {
        myMesh.vertices = new Vector3[] {topLeftVertex, topRightVertex, bottomLeftVertex, bottomRightVertex};
    }

    void UpdateNeighbours()
    {
        if(topNeighbour)
        {
            topNeighbour.SetBottomLeft(topLeftVertex - topNeighbour.transform.position);
            topNeighbour.SetBottomRight(topRightVertex - topNeighbour.transform.position);
            topNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();

            PathNode rNeighbour = topNeighbour.GetRightNeighbour();
            if(rNeighbour)
            {
                rNeighbour.SetBottomLeft(topLeftVertex - topNeighbour.transform.position);
                rNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }

            PathNode lNeighbour = topNeighbour.GetLeftNeighbour();
            if(lNeighbour)
            {
                lNeighbour.SetBottomRight(topRightVertex - topNeighbour.transform.position);
                lNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }
        }

        if(bottomNeighbour)
        {
            bottomNeighbour.SetTopLeft(bottomLeftVertex - bottomNeighbour.transform.position);
            bottomNeighbour.SetTopRight(bottomRightVertex - bottomNeighbour.transform.position);
            bottomNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();

            PathNode rNeighbour = bottomNeighbour.GetRightNeighbour();
            if(rNeighbour)
            {
                rNeighbour.SetTopLeft(bottomLeftVertex - bottomNeighbour.transform.position);
                rNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }

            PathNode lNeighbour = topNeighbour.GetLeftNeighbour();
            if(lNeighbour)
            {
                lNeighbour.SetTopRight(bottomRightVertex - bottomNeighbour.transform.position);
                lNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }

        }

        if(leftNeighbour)
        {
            leftNeighbour.SetTopRight(topLeftVertex - leftNeighbour.transform.position);
            leftNeighbour.SetBottomRight(bottomLeftVertex - leftNeighbour.transform.position);
            leftNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();

            PathNode tNeighbour = leftNeighbour.GetTopNeighbour();
            if(tNeighbour)
            {
                tNeighbour.SetBottomRight(bottomLeftVertex - leftNeighbour.transform.position);
                tNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }

            PathNode bNeighbour = leftNeighbour.GetBottomNeighbour();
            if(bNeighbour)
            {
                bNeighbour.SetTopRight(topLeftVertex - leftNeighbour.transform.position);
                bNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }
        }

        if(rightNeighbour)
        {
            rightNeighbour.SetTopLeft(topRightVertex - rightNeighbour.transform.position);
            rightNeighbour.SetBottomLeft(bottomRightVertex - rightNeighbour.transform.position);
            rightNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();

            PathNode tNeighbour = leftNeighbour.GetTopNeighbour();
            if(tNeighbour)
            {
                tNeighbour.SetBottomLeft(bottomRightVertex - rightNeighbour.transform.position);
                tNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }

            PathNode bNeighbour = leftNeighbour.GetBottomNeighbour();
            if(bNeighbour)
            {
                bNeighbour.SetTopLeft(topRightVertex - rightNeighbour.transform.position);
                bNeighbour.GetComponent<PathNodeEditor>().MeshUpdateWrapper();
            }
        }
    }

    void UpdateNeighbourAssing(ref bool updateNeighbours)
    {
        if(topNeighbour != myNode.GetTopNeighbour())
        {
            //REMOVE LAST TOP BOTTOM NEIGHBOUR
            if(topNeighbour)
            {
                topNeighbour.RemoveBottomNeighbour();
            }

            topNeighbour = myNode.GetTopNeighbour();
            if(topNeighbour)
            {
                if(topNeighbour.GetBottomNeighbour() != myNode)
                {
                    topNeighbour.SetBottomNeighbour(ref myNode);
                }

                Vector3 middlePoint;            
                middlePoint = GetMiddlePoint(topNeighbour.GetBottomLeft() + topNeighbour.transform.position, lastTopLeft + transform.position);

                lastTopLeft += middlePoint;
                myNode.SetTopLeft(lastTopLeft);
                topNeighbour.SetBottomLeft(topNeighbour.GetBottomLeft() - middlePoint);

                middlePoint = GetMiddlePoint(topNeighbour.GetBottomRight() + topNeighbour.transform.position, lastTopRight + transform.position);

                lastTopRight += middlePoint;
                myNode.SetTopRight(lastTopRight);
                topNeighbour.SetBottomRight(topNeighbour.GetBottomRight() - middlePoint);

                CalculateVerticies();

                UpdateMesh();

                updateNeighbours = true;
                //UpdateNeighbours();
            }
        }

        if(bottomNeighbour != myNode.GetBottomNeighbour())
        {
            if(bottomNeighbour)
            {
                bottomNeighbour.RemoveTopNeighbour();
            }

            bottomNeighbour = myNode.GetBottomNeighbour();
            if(bottomNeighbour && bottomNeighbour.GetTopNeighbour() != myNode)
            {
                bottomNeighbour.SetTopNeighbour(ref myNode);
            }

            updateNeighbours = true;
            //UpdateNeighbours();
        }

        if(leftNeighbour != myNode.GetLeftNeighbour())
        {
            if(leftNeighbour)
            {
                leftNeighbour.RemoveRightNeighbour();
            }

            leftNeighbour = myNode.GetLeftNeighbour();
            if(leftNeighbour)
            {
                if(leftNeighbour.GetRightNeighbour() != myNode)
                {
                    leftNeighbour.SetRightNeighbour(ref myNode);
                }

                Vector3 middlePoint;            
                middlePoint = GetMiddlePoint(leftNeighbour.GetTopRight() + leftNeighbour.transform.position, lastTopLeft + transform.position);

                lastTopLeft += middlePoint;
                myNode.SetTopLeft(lastTopLeft);
                leftNeighbour.SetTopRight(leftNeighbour.GetTopRight() - middlePoint);

                middlePoint = GetMiddlePoint(leftNeighbour.GetBottomRight() + leftNeighbour.transform.position, lastBottomLeft + transform.position);

                lastBottomLeft += middlePoint;
                myNode.SetBottomLeft(lastBottomLeft);
                leftNeighbour.SetBottomRight(leftNeighbour.GetBottomRight() - middlePoint);

                CalculateVerticies();

                UpdateMesh();

                updateNeighbours = true;
                //UpdateNeighbours();
            }
        }

        if(rightNeighbour != myNode.GetRightNeighbour())
        {
            if(rightNeighbour)
            {
                rightNeighbour.RemoveLeftNeighbour();
            }

            rightNeighbour = myNode.GetRightNeighbour();
            if(rightNeighbour && rightNeighbour.GetLeftNeighbour() != myNode)
            {
                rightNeighbour.SetLeftNeighbour(ref myNode);
            }

            updateNeighbours = true;
            //UpdateNeighbours();
        }
    }

    public void MeshUpdateWrapper()
    {
        AssignVertexOffsets();

        CalculateVerticies();

        UpdateMesh();
    }

    public Vector3 GetMiddlePoint(Vector3 vectorA, Vector3 vectorB)
    {
        return (vectorA - vectorB) / 2;
    }
}
