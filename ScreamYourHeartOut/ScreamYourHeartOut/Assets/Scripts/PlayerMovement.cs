using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Variables
    private Rigidbody rbody;
    //Set Jump Force
    public float thrust;
    //Set Movement Speed
    public float moveSpeed;
    //Check For Ground
    private bool Grounded;
    //Jump Counter
    private int JumpCount;

    //Raycasting
    private RaycastHit vision; // Used for detecting Raycast collision
    public float rayLength; // Used for assigning a length to the raycast


    // Use this for initialization
    void Start () {
        //Grab Player Rigidbody
        rbody = this.gameObject.GetComponent<Rigidbody>();
        //Raycasting
        rayLength = 0.75f;
        JumpCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
        //This will constantly draw the ray in our Scene view so we can see where the ray is going
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red, 0.5f);

        //Check for player input
        if (Input.GetKey(KeyCode.A))
        {
            rbody.MovePosition(transform.position + new Vector3(-moveSpeed, 0));
        } else if (Input.GetKey(KeyCode.D)) {
            rbody.MovePosition(transform.position + new Vector3(moveSpeed, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space) && Grounded == true && JumpCount < 1)
        {
            rbody.AddForce(new Vector2(0, thrust), ForceMode.Impulse);
            JumpCount++;
        }
	}

    void FixedUpdate()
    {
        //This statement is called when the Raycast is hitting a collider in the scene
        if (Physics.Raycast(transform.position - new Vector3(0, -0.1f, 0), Vector3.down, out vision, rayLength))
        {
            Grounded = true;
            JumpCount = 0;
        }
        else
        {
            Grounded = false;
        }
    }
}
