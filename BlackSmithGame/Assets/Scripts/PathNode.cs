using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class PathNode : MonoBehaviour
{
    static Color oragne = new Color(1f, 0.647f, 0f, 0.5f);
    static Color red = new Color(1f, 0f, 0f, 1f);

    [SerializeField] Vector2 topLeft = new Vector2(-0.5f, 0.5f);
    [SerializeField] Vector2 topRight = new Vector2(0.5f, 0.5f);
    [SerializeField] Vector2 bottomLeft = new Vector2(-0.5f, -0.5f);
    [SerializeField] Vector2 bottomRight = new Vector2(0.5f, -0.5f);

    Vector3 up = new Vector3(0, 0, 90);

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
        Gizmos.color = red;

        Vector3 topLeftPos = new Vector3(topLeft.x + transform.position.x, transform.position.y, topLeft.y + transform.position.z);
        Vector3 topRightPos = new Vector3(topRight.x + transform.position.x, transform.position.y, topRight.y + transform.position.z);
        Vector3 bottomLeftPos = new Vector3(bottomLeft.x + transform.position.x, transform.position.y, bottomLeft.y + transform.position.z);
        Vector3 bottomRightPos = new Vector3(bottomRight.x + transform.position.x, transform.position.y, bottomRight.y + transform.position.z);

        Gizmos.DrawLine(topLeftPos, topRightPos);
        Gizmos.DrawLine(topLeftPos, bottomLeftPos);
        Gizmos.DrawLine(topRightPos, bottomRightPos);
        Gizmos.DrawLine(bottomLeftPos, bottomRightPos);

        Gizmos.color = oragne;

        Mesh someMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = someMesh;

        someMesh.vertices = new Vector3[] {topLeftPos, topRightPos, bottomLeftPos, bottomRightPos};
        someMesh.triangles = new int[] {0, 1, 2, 2, 1, 3};
        someMesh.RecalculateNormals();

        Gizmos.DrawMesh(someMesh);
    }
}
