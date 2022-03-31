using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class MeshEditor : MonoBehaviour
{
    Vector3[] originalVerticies;
    [SerializeField] Vector3[] verticies;
    [SerializeField] int[] tris;
    [SerializeField] Vector2[] UV;
    [SerializeField] Vector3[] normals;
    [SerializeField] Vector3 meshScale = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] bool enableVertexModification = false;
    MeshFilter meshFilter = null;

    private void Awake() 
    {
        meshFilter = GetComponent<MeshFilter>();
        if(!meshFilter)
        {
            gameObject.AddComponent<MeshFilter>();
            meshFilter = GetComponent<MeshFilter>();
        }
        if(!GetComponent<MeshRenderer>())
        {
            gameObject.AddComponent<MeshRenderer>();
        }

        originalVerticies = meshFilter.mesh.vertices;
        verticies = meshFilter.mesh.vertices;
        tris = meshFilter.mesh.triangles;
        UV = meshFilter.mesh.uv;
        normals = meshFilter.mesh.normals;
        //myMesh = meshFilter.mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Application.isPlaying)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objectOrigin = gameObject.transform.position;

        if(verticies.Length != originalVerticies.Length)
        {
            //Array.Clear(originalVerticies, 0, originalVerticies.Length); // do we actually need to clear it?, doubt
            originalVerticies = new Vector3[verticies.Length];
            for(int index = 0; index < verticies.Length; index++)
            {
                originalVerticies[index] = new Vector3(verticies[index].x / meshScale.x, verticies[index].y / meshScale.y, verticies[index].z / meshScale.z);
            }
        }

        for(int index = 0; index < verticies.Length; index++)
        {
            
        }

        for(int index = 0; index < verticies.Length; index++)
        {
            if(enableVertexModification)
            {
                originalVerticies[index] = new Vector3(verticies[index].x / meshScale.x, verticies[index].y / meshScale.y, verticies[index].z / meshScale.z);
            }
            else
            {
                // multiply by scale
                Vector3 scaledDistanceVector = Vector3.Scale(originalVerticies[index], meshScale);
                // apply to vertex position
                verticies[index] = scaledDistanceVector;// + objectOrigin;
            }
        }

        meshFilter.mesh.vertices = verticies;
        meshFilter.mesh.triangles = tris;
        meshFilter.mesh.uv = UV;
        meshFilter.mesh.normals = normals;
    }
}
