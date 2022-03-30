using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PathNode : MonoBehaviour
{
    static Color oragne = new Color(1f, 0.647f, 0f, 0.5f);
    static Color red = new Color(1f, 0f, 0f, 1f);

    [SerializeField] Vector3 topLeft = new Vector3(-0.5f, 0, 0.5f);
    [SerializeField] Vector3 topRight = new Vector3(0.5f, 0, 0.5f);
    [SerializeField] Vector3 bottomLeft = new Vector3(-0.5f, 0, -0.5f);
    [SerializeField] Vector3 bottomRight = new Vector3(0.5f, 0, -0.5f);

    // should probably be public fields
    // or just setter methods
    // we need to be able to assign neighbours after one isss assigned in another neghbour
    // should work during editor runtime
    [SerializeField] PathNode topNeighbour;
    [SerializeField] PathNode bottomNeighbour;
    [SerializeField] PathNode leftNeighbour;
    [SerializeField] PathNode rightNeighbour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = red;

        // Vector3 topLeftPos = new Vector3(topLeft.x + transform.position.x, topLeft.y + transform.position.y, topLeft.z + transform.position.z);
        // Vector3 topRightPos = new Vector3(topRight.x + transform.position.x, topRight.y + transform.position.y, topRight.z + transform.position.z);
        // Vector3 bottomLeftPos = new Vector3(bottomLeft.x + transform.position.x, bottomLeft.y + transform.position.y, bottomLeft.z + transform.position.z);
        // Vector3 bottomRightPos = new Vector3(bottomRight.x + transform.position.x, bottomRight.y + transform.position.y, bottomRight.z + transform.position.z);

        // Gizmos.DrawLine(topLeftPos, topRightPos);
        // Gizmos.DrawLine(topLeftPos, bottomLeftPos);
        // Gizmos.DrawLine(bottomLeftPos, topRightPos);
        // Gizmos.DrawLine(topRightPos, bottomRightPos);
        // Gizmos.DrawLine(bottomLeftPos, bottomRightPos);

        // Gizmos.color = oragne;

        // Mesh someMesh = new Mesh();
        // GetComponent<MeshFilter>().mesh = someMesh;

        // someMesh.vertices = new Vector3[] {topLeftPos, topRightPos, bottomLeftPos, bottomRightPos};
        // someMesh.triangles = new int[] {0, 1, 2, 2, 1, 3};
        // someMesh.RecalculateNormals();

        // Gizmos.DrawMesh(someMesh);
    }

    public Vector3 GetTopLeft()
    {
        return topLeft;
    }

    public Vector3 GetTopRight()
    {
        return topRight;
    }

    public Vector3 GetBottomLeft()
    {
        return bottomLeft;
    }

    public Vector3 GetBottomRight()
    {
        return bottomRight;
    }
}
