using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMemberFlight : MonoBehaviour
{

    public GameObject flyingBirb;

    public Vector3 offset,
        lookdir;

    public float baseSpeed,
        roll,
        pitch,
        smoothRot,
        smoothCam,
        yawrot;

    void Start()
    {

    }

    void FixedUpdate()
    {
        //sets the players velocity so it moves forward
        /*if (start == true)
        {
            rb.velocity = transform.forward * baseSpeed;
        }*/
        PlayerInput();
        BirdMovement();
    }

    //player input
    public void PlayerInput()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

        transform.Translate(x, 0, 0);
        transform.Translate(0, y, 0);
        
    }

    //Moves the player bird toward the point of flight
    public void BirdMovement()
    {
        //find the yaw and pitch
        roll = Input.GetAxis("Horizontal") * Time.deltaTime;
        pitch = Input.GetAxis("Vertical") * Time.deltaTime;
        var rollrot = -roll * 2500;
        var pitchrot = -pitch * 1500;
        var campitch = -pitch * 250;
        var camroll = -roll * 250;

        //moves the objects at a delay and rotates them accordingly
        flyingBirb.transform.position = Vector3.Lerp(flyingBirb.transform.position, gameObject.transform.position - offset, Time.deltaTime);
        flyingBirb.transform.rotation = Quaternion.RotateTowards(flyingBirb.transform.rotation, Quaternion.Euler(new Vector3(pitchrot, 0, rollrot)), smoothRot * Time.deltaTime);        
    }
}


/*public GameObject player;
    public FlockController flockCont;

    public float timer,
        moveSpeed,
        smoothRot;

    public List<Vector3> moveTo = new List<Vector3>();

    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        timer = 0.5f;
        player = GameObject.FindGameObjectWithTag("Player");
        flockCont = GameObject.FindGameObjectWithTag("FlockParent").GetComponent<FlockController>();
        AddToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTo.Count > 19)
        {
            moveTo.Remove(moveTo[0]);
            AddToList();
        }
        else
        {
            AddToList();
        }
        var dist = Vector3.Distance(gameObject.transform.position, moveTo[0]);

        if (dist < 0.1)
        {
            moveTo.Remove(moveTo[0]);
            AddToList();
            print("Yeet");
        }

        FlockMovement();
    }

    public void AddToList()
    {
        moveTo.Add(player.transform.position + offset);
    }

    public void FlockMovement()
    {
        var roll = Input.GetAxis("Horizontal") * Time.deltaTime;
        var pitch = Input.GetAxis("Vertical") * Time.deltaTime;
        var rollrot = -roll * 2500;
        var pitchrot = -pitch * 1500;

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, moveTo[0], moveSpeed * Time.deltaTime);

        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.Euler(new Vector3(pitchrot, 0, rollrot)), smoothRot * Time.deltaTime);

    }
}*/
