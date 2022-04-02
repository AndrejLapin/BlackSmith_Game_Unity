using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PathNodeEditor : MonoBehaviour
{
    // half transparent orange
    static Color oragne = new Color(1f, 0.647f, 0f, 0.5f);
    static Color red = new Color(1f, 0f, 0f, 1f);

    // this should be global
    bool drawGizmos = true;
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

    private void Awake()
    {
        // destroy this if its not a debug version
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
}
