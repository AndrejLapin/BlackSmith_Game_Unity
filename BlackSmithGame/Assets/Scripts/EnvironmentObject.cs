using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
    public struct TransparentObject
    {
        Material spriteMaterial;
        float transparency;
    }
    //[SerializeField] Collider2D transparencyCollider;
    [SerializeField] SpriteRenderer[] rendererCollection;
    [SerializeField] float transparency = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("Collider entered by " + other.name);

        if(other.tag == "Player")
        {
            foreach (var renderer in rendererCollection)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, transparency);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            foreach (var renderer in rendererCollection)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1.0f); // assuming it had alpha 1, can cause a bug
            }
        }
    }
}
