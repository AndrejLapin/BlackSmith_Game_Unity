using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class MeshEditor : MonoBehaviour
{
    [SerializeField] Vector3[] verticies;
    [SerializeField] int[] tris;
    [SerializeField] Vector2[] UV;
    [SerializeField] Vector3[] normals;
    [SerializeField] Vector3 meshScale = new Vector3(1.0f, 1.0f, 1.0f);
    Vector3 lasMeshScale;
    [SerializeField] Vector2 uvOffset = new Vector2(0.0f, 0.0f);
    Vector2 lastUVOffset;
    [SerializeField] Vector2 uvScale = new Vector2(1.0f, 1.0f);
    Vector2 lasUVScale;
    MeshFilter meshFilter = null;

    const float minScaleValue = 0.0000000001f;

    private void Awake() 
    {
        lasMeshScale = meshScale;
        lasUVScale = uvScale;
        lastUVOffset = uvOffset;

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

        verticies = meshFilter.mesh.vertices;
        tris = meshFilter.mesh.triangles;
        UV = meshFilter.mesh.uv;
        normals = meshFilter.mesh.normals;
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
        if(meshScale != lasMeshScale)
        {
            meshScale.x = meshScale.x == 0 ? minScaleValue : meshScale.x;
            meshScale.y = meshScale.y == 0 ? minScaleValue : meshScale.y;
            meshScale.z = meshScale.z == 0 ? minScaleValue : meshScale.z;

            for(int index = 0; index < verticies.Length; index++)
            {
                Vector3 notScaledVertex = new Vector3(verticies[index].x / lasMeshScale.x, verticies[index].y / lasMeshScale.y, verticies[index].z / lasMeshScale.z);
                verticies[index] = Vector3.Scale(notScaledVertex, meshScale);
            }
            lasMeshScale = meshScale;
        }

        if(uvScale != lasUVScale)
        {
            uvScale.x = uvScale.x == 0 ? minScaleValue : uvScale.x;
            uvScale.y = uvScale.y == 0 ? minScaleValue : uvScale.y;
            for(int index = 0; index < UV.Length; index++)
            {
                UV[index].x *= uvScale.x / lasUVScale.x;
                UV[index].y *= uvScale.y / lasUVScale.y;
            }
        }

        if(uvOffset != lastUVOffset)
        {
            for(int index = 0; index < UV.Length; index++)
            {
                UV[index].x += uvOffset.x - lastUVOffset.x;
                UV[index].y += uvOffset.y - lastUVOffset.y;
            }
        }

        meshFilter.mesh.vertices = verticies;
        meshFilter.mesh.triangles = tris;
        meshFilter.mesh.uv = UV;
        meshFilter.mesh.normals = normals;
    }
}
