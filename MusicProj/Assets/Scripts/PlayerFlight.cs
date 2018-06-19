using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlight : MonoBehaviour
{

    public GameObject bird,
        cam;

    public Rigidbody rb,
        birdrb;

    public Vector3 offset = new Vector3(0, 0, 2),
        lookdir;

    public float baseSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        //sets the players velocity so it moves forward
        rb.velocity = transform.forward * baseSpeed;
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
        //finds the rotation needed to look at
        var blook = Quaternion.LookRotation(gameObject.transform.position - bird.transform.position);
        var clook = Quaternion.LookRotation(bird.transform.position - cam.transform.position);

        //moves the objects at a delay and rotates them accordingly
        bird.transform.position = Vector3.Lerp(bird.transform.position, gameObject.transform.position - offset, Time.deltaTime);
        cam.transform.position = Vector3.Lerp(cam.transform.position, bird.transform.position - (offset * 0.5f), Time.deltaTime);
        bird.transform.LookAt(gameObject.transform.position);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, clook, Time.deltaTime);
    }
}
