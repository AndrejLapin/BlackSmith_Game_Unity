using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    const string HORIZONTAL_AXIS_NAME = "Horizontal";
    const string VERTICAL_AXIS_NAME = "Vertical";
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float acceleration = 0.8f;
    [SerializeField] GameObject body;
    float moveSpeed = 0f;
    Rigidbody2D myRigidBody;

    bool lookingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // framerate independant update
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float horizontalThrow = Input.GetAxisRaw(HORIZONTAL_AXIS_NAME);
        float verticalThrow = Input.GetAxisRaw(VERTICAL_AXIS_NAME);
        float hypothenuse = Mathf.Sqrt(horizontalThrow * horizontalThrow + verticalThrow * verticalThrow);

        float horizontalVelocity;
        float verticalVelocity;

        if(hypothenuse != 0)
        {
            moveSpeed += acceleration;
            if(moveSpeed > maxMoveSpeed) moveSpeed = maxMoveSpeed;
        }
        else
        {
            moveSpeed -= acceleration;
            if(moveSpeed < 0) moveSpeed = 0;
        }
        
        if(horizontalThrow != 0)
        {
            horizontalVelocity = horizontalThrow / hypothenuse * moveSpeed;
        }
        else
        {
            horizontalVelocity = myRigidBody.velocity.x * acceleration * acceleration;
        }

        if(verticalThrow != 0)
        {
            verticalVelocity = verticalThrow / hypothenuse * moveSpeed;
        }
        else
        {
            verticalVelocity = myRigidBody.velocity.y * acceleration * acceleration;
        }

        Vector2 playerVelocity = new Vector2(horizontalVelocity, verticalVelocity);
        myRigidBody.velocity = playerVelocity;

        if(horizontalVelocity > 0 && lookingLeft)
        {
            lookingLeft = false;
            body.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else if(horizontalVelocity < 0 && !lookingLeft)
        {
            lookingLeft = true;
            body.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
