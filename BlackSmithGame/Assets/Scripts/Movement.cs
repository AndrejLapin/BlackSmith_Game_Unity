using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    const string HORIZONTAL_AXIS_NAME = "Horizontal"; // probably should be taking this name from settings
    const string VERTICAL_AXIS_NAME = "Vertical";
    const string ANIMATOR_RUNNING = "running";

    [SerializeField] float maxMoveSpeed = 2.5f;
    [SerializeField] float acceleration = 0.8f;
    [SerializeField] Animator characterAnimator;
    [SerializeField] GameObject characterBody;
    [SerializeField] float bodyRotationSpeed = 0.5f;
    [SerializeField] Camera myCamera;

    [SerializeField] float targetAngleY;
    [SerializeField] float currentAngleY;
    [SerializeField] float interpolationAngle;
    float moveSpeed = 0f;
    Rigidbody myRigidBody;

    bool lookingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // framerate independant update
    void FixedUpdate()
    {
        Move();

        //Debug.Log(myCamera.transform.rotation.eulerAngles.y);
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
            characterAnimator.SetBool(ANIMATOR_RUNNING, true);
            moveSpeed += acceleration * Time.deltaTime;
            if(moveSpeed > maxMoveSpeed) moveSpeed = maxMoveSpeed;
        }
        else
        {
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

        // rotation interpolation implementation
        // targetAngleY = rotationTargetAngle;
        // currentAngleY = characterBody.transform.rotation.eulerAngles.y;
        // if(characterBody.transform.rotation.eulerAngles.y != rotationTargetAngle)
        // {
        //     float interpolatedAngle =  Mathf.Lerp(characterBody.transform.rotation.eulerAngles.y, rotationTargetAngle, bodyRotationSpeed * Time.deltaTime);
        //     interpolationAngle = interpolatedAngle;
        //     characterBody.transform.rotation = Quaternion.Euler(0, interpolatedAngle * Mathf.Sign(playerVelocity.x), 0);
        // }
    }
}
