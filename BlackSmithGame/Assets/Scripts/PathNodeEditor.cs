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

        if(lastTopLeft != myNode.GetTopLeft()
        || lastTopRight != myNode.GetTopRight()
        || lastBottomLeft != myNode.GetBottomLeft()
        || lastBottomRight != myNode.GetBottomLeft()) // I guess just move stuff to on gizmo draw and update mesh in OnGizmos draw, seems more efficient
        {
            AssignVertexOffsets();

            CalculateVerticies();

            UpdateMesh();

            UpdateNeighbourAssing();
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
        topLeftVertex = new Vector3(lastTopLeft.x + transform.position.x, lastTopLeft.y + transform.position.y, lastTopLeft.z + transform.position.z);
        topRightVertex = new Vector3(lastTopRight.x + transform.position.x, lastTopRight.y + transform.position.y, lastTopRight.z + transform.position.z);
        bottomLeftVertex = new Vector3(lastBottomLeft.x + transform.position.x, lastBottomLeft.y + transform.position.y, lastBottomLeft.z + transform.position.z);
        bottomRightVertex = new Vector3(lastBottomRight.x + transform.position.x, lastBottomRight.y + transform.position.y, lastBottomRight.z + transform.position.z);
    }

    void UpdateMesh()
    {
        myMesh.vertices = new Vector3[] {topLeftVertex, topRightVertex, bottomLeftVertex, bottomRightVertex};
    }

    void UpdateNeighbourAssing()
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
        }
    }

    public Vector3 GetMiddlePoint(Vector3 vectorA, Vector3 vectorB)
    {
        return (vectorA - vectorB) / 2;
    }
}
