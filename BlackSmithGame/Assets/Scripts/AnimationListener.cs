using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    AnimationEventHandler myAnimationEventHandler;
    private void Awake() 
    {
        myAnimationEventHandler = gameObject.GetComponentInParent(typeof(AnimationEventHandler)) as AnimationEventHandler;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationEnded(string parameter)
    {
        myAnimationEventHandler.AnimationEnded(parameter);
    }
}
