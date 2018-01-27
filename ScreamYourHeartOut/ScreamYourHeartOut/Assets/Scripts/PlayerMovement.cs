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
    //Force applied to enemy
    public int Force;

    //Raycasting
    private RaycastHit vision; // Used for detecting Raycast collision
    public float rayLength; // Used for assigning a length to the raycast
    
    private SphereCollider sphereCollider;

    // Use this for initialization
    void Start () {
        //Grab Player Rigidbody
        rbody = this.gameObject.GetComponent<Rigidbody>();
        //Raycasting
        rayLength = 0.75f;
        //Jump Count
        JumpCount = 0;
        //Get the player's sphere collider component
        sphereCollider = this.gameObject.GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(other.transform.position.x < this.transform.position.x)
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector2(-Force, 0), ForceMode.Impulse);
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector2(Force, 0), ForceMode.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.R))
        {
            StartCoroutine("Increase");
        }
        
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

    IEnumerator Increase()
    {
        for (float i = 2f; i >= 0; i -= 0.1f)
        {
            sphereCollider.radius += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine("Decrease");
    }

    IEnumerator Decrease()
    {
        for (float d = 2f; d >= 0; d -= 0.1f)
        {
            sphereCollider.radius -= 0.1f;
            yield return new WaitForSeconds(0.1f);
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
