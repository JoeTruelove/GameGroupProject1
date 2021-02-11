using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool debugMode = false;
    public bool isGrounded;
    public float jumpHeight = 3f;
    public bool isMoving = false;
    private float yRot;
    public float speed = 1;
    public float horizontalSpeed = 1;
    public bool onWall = false;
    public float raycastLineDist = 1;
    public bool frontWall = false;
    public bool death = false;
    public bool startMoving = false;
    public int timesJumped = 0;
    public int currentLevel = 1;
    private bool justPortaled = false;
    
    private Animator anim;
    private Rigidbody rigidBody;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        startMoving = false;

    }
    private void FixedUpdate()
    {

        //Raycast
        Vector3 forward = transform.TransformDirection(Vector3.forward) * raycastLineDist;
        Debug.DrawRay(transform.position, forward, Color.red);

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, forward, raycastLineDist))
        {
            print("hit!");
            frontWall = true;
        }
        else
        {
            frontWall = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            startMoving = true;
        }

        //Camera Code
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, yRot, transform.localEulerAngles.z);

        isMoving = false;

        //Jumping code
        if ((Input.GetKeyDown(KeyCode.Space) && (isGrounded || (timesJumped < 2 && onWall))))
        {
            //transform.Translate(Vector3.up * jumpHeight);
            //rigidBody.AddForce(transform.TransformDirection(Vector3.up) * 5);
            rigidBody.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            isGrounded = false;
            timesJumped++;
        }

        //move character forward
        if (!frontWall && startMoving)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
        
        //Moving left and right
        if ((Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f && !onWall) && startMoving)
        {
            //transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * playerSpeed);
            rigidBody.velocity += transform.right * Input.GetAxisRaw("Horizontal") * horizontalSpeed;
            isMoving = true;
        }


        //Reset Player Position
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.gameObject.transform.position = new Vector3(23.019f, 0.231f, 25.645f);
            startMoving = false;
        }

        //Gravity Code
        /*if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.gameObject.transform.Rotate(0, 0, 180);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.gameObject.transform.Rotate(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.gameObject.transform.Rotate(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.gameObject.transform.Rotate(0, 0, 270);
        }*/

        //Restart Code
        if(death && currentLevel == 1)
        {
            this.gameObject.transform.position = new Vector3(23.019f, 0.231f, 25.645f);
            death = false;
            startMoving = false;
        }
        else if (death && currentLevel == 2)
        {
            this.gameObject.transform.position = new Vector3(281.98f, -0.37f, -80.2f);
            death = false;
            startMoving = false;
        }

        if(currentLevel == 2 && justPortaled)
        {
            this.gameObject.transform.position = new Vector3(281.98f, -0.37f, 80.2f);
            startMoving = false;
            justPortaled = false;
        }

        //wanim.SetBool("isMoving", isMoving);

        
    }


    //consider what the object is collider with
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
            timesJumped = 0;
        }

        if (other.gameObject.tag == "Wall")
        {
            onWall = true;
        }

        if (other.gameObject.tag == "Death")
        {
            death = true;
        }

        if (other.gameObject.tag == "Portal")
        {
            currentLevel = 2;
            justPortaled = true;
        }
    }

    //consider when character is jumping .. it will exit collision.
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }

        if (other.gameObject.tag == "Wall")
        {
            onWall = false;
        }

        if (other.gameObject.tag == "Death")
        {
            death = false;
        }
    }

}
