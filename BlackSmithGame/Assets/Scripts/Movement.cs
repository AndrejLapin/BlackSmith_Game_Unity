using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, AnimationEventHandler
{
    // put in a seperate file
    enum CharacterState
    {
        Idle = 0,
        Moving = 1,
        Dodging = 2
    }

    // put in a seperate file
    const string HORIZONTAL_AXIS_NAME = "Horizontal";
    const string VERTICAL_AXIS_NAME = "Vertical";
    const string DODGE_INPUT_NAME = "Dodge";

    const string ANIMATOR_RUNNING = "running";
    const string ANIMATOR_DODGE = "dodge";

    [SerializeField] float maxMoveSpeed = 2.5f;
    [SerializeField] float acceleration = 0.8f;
    [SerializeField] Animator characterAnimator;
    [SerializeField] GameObject characterBody;
    [SerializeField] float bodyRotationSpeed = 0.5f;
    [SerializeField] Camera myCamera;
    [SerializeField] bool dodgeToMouse = false;
    [SerializeField] float dodgeVelocity = 1.5f;

    CharacterState myState;
    float moveSpeed = 0f;
    Rigidbody myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myState = CharacterState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(myState == CharacterState.Idle || myState == CharacterState.Moving)
        {
            Move();
            Dodge();
        }
        else if(myState == CharacterState.Dodging)
        {
            //need to turn off movement
        }
    }

    // framerate independant update
    void FixedUpdate()
    {
        
    }

    void Move()
    {
        float horizontalThrow = Input.GetAxisRaw(HORIZONTAL_AXIS_NAME);
        float verticalThrow = Input.GetAxisRaw(VERTICAL_AXIS_NAME);
        float hypothenuse = Mathf.Sqrt(horizontalThrow * horizontalThrow + verticalThrow * verticalThrow);

        float horizontalVelocity = 0;
        float verticalVelocity = 0;

        if(hypothenuse != 0)
        {
            myState = CharacterState.Moving;
            characterAnimator.SetBool(ANIMATOR_RUNNING, true);
            moveSpeed += acceleration * Time.deltaTime;
            if(moveSpeed > maxMoveSpeed) moveSpeed = maxMoveSpeed;
        }
        else
        {
            myState = CharacterState.Idle;
            characterAnimator.SetBool(ANIMATOR_RUNNING, false);
            moveSpeed -= acceleration * Time.deltaTime;
            if(moveSpeed < 0) moveSpeed = 0;
        }
        
        if(horizontalThrow != 0)
        {
            horizontalVelocity = horizontalThrow / hypothenuse * moveSpeed ;
        }
        // else // to perform a little drift
        // {
        //     horizontalVelocity = myRigidBody.velocity.x * acceleration * acceleration;
        // }

        if(verticalThrow != 0)
        {
            verticalVelocity = verticalThrow / hypothenuse * moveSpeed;
        }
        // else // to perform a little drift
        // {
        //     verticalVelocity = myRigidBody.velocity.z * acceleration * acceleration;
        // }

        // calculating player velocity vector
        Vector3 playerVelocity = new Vector3(horizontalVelocity, 0, verticalVelocity);
        // rotating velocity vector by camera rotation
        // needed so if the camera faces other way controls behave naturally
        playerVelocity = Quaternion.Euler(0, myCamera.transform.rotation.eulerAngles.y, 0 ) * playerVelocity;

        if (hypothenuse != 0)
        {
            RotateBody(playerVelocity);
        }
        myRigidBody.velocity = playerVelocity;
    }

    void RotateBody(Vector3 playerVelocity)
    {
        float rotationTargetAngle = Vector3.Angle(Vector3.forward, playerVelocity) * Mathf.Sign(playerVelocity.x);
        characterBody.transform.rotation = Quaternion.Euler(0, rotationTargetAngle, 0);
    }

    void Dodge()
    {
        if(Input.GetButtonDown(DODGE_INPUT_NAME))
        {
            myState = CharacterState.Dodging;
            characterAnimator.SetTrigger(ANIMATOR_DODGE);
            //characterAnimator.SetBool(ANIMATOR_RUNNING, false);

            StopMovement();

            if(!dodgeToMouse)
            {
                // get character rotation
                Vector3 playerVelocity = new Vector3(0, 0, dodgeVelocity);
                // aplly dodge velocity
                myRigidBody.velocity = Quaternion.Euler(0,  characterBody.transform.rotation.eulerAngles.y, 0 ) * playerVelocity;
            }
        }
    }

    void StopMovement()
    {
        myRigidBody.velocity = new Vector3(0, 0, 0);
    }

    public void AnimationEnded(string parameter)
    {
        Debug.Log(parameter);
        if(parameter == ANIMATOR_DODGE)
        {
            myState = CharacterState.Idle;
        }
    }
    
}
