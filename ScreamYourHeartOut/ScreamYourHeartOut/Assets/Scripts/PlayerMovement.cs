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
    private bool cooldown = false;

    public GameObject Shout;
    public GameObject ShoutEffect;
    private Animator anim;
    public float time;
    public float timer;
    
    //Raycasting
    private RaycastHit vision; // Used for detecting Raycast collision
    public float rayLength; // Used for assigning a length to the raycast
    
    private SphereCollider sphereCollider;

    // Use this for initialization
    void Start () {
        time = 2;
        timer = time;
        anim = this.gameObject.GetComponent<Animator>();
        anim.SetBool("IsIdle", true);
        //Grab Player Rigidbody
        rbody = this.gameObject.GetComponent<Rigidbody>();
        //Raycasting
        rayLength = 4.0f;
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
        if (cooldown == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine("Increase");
                Shout = (GameObject)Instantiate(ShoutEffect, (this.transform.position + new Vector3(0, 3.3f, 0)), transform.rotation);

                Invoke("ResetCooldown", 3.0f);
                cooldown = true;
            }
        }
        
        //This will constantly draw the ray in our Scene view so we can see where the ray is going
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red, 0.5f);

        //Check for player input
        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            anim.SetBool("IsIdle", false);
            rbody.MovePosition(transform.position + new Vector3(-moveSpeed, 0));
        } else if (Input.GetKey(KeyCode.D)) {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            anim.SetBool("IsIdle", false);
            rbody.MovePosition(transform.position + new Vector3(moveSpeed, 0));
        } else
        {
            anim.SetBool("IsIdle", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Grounded == true && JumpCount < 1)
        {
            rbody.AddForce(new Vector2(0, thrust), ForceMode.Impulse);
            JumpCount++;
        }
    }

    void ResetCooldown()
    {
        cooldown = false;
    }

    IEnumerator Increase()
    {
        sphereCollider.radius = 0.8f;
        yield return new WaitForSeconds(0.2f);
        for (float i = 2f; i >= 0; i -= 0.1f)
        {
            sphereCollider.radius += 0.33f;
            yield return new WaitForSeconds(0.1f);
        }

        sphereCollider.radius = 0.8f;
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
