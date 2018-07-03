using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptFlight : MonoBehaviour
{
    public GameObject birdModel;

    public float smoothRot;

    public float speed;

    public Vector3 offset;

    public float rollrot;

    public Rigidbody rb; 
    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ModelMoveAndRotate();
        CamFollow();
    }

    #region Movement
    void Movement()
    {
        var y = Input.GetAxis("Vertical");
        var x = Input.GetAxis("Horizontal");

        rb.velocity = transform.forward * speed + transform.up * y * speed;
        //rb.velocity = transform.up * y * speed;
        transform.Rotate(0, x, 0);

        float CheckTerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        if (CheckTerrainHeight > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, CheckTerrainHeight + 1, transform.position.z);
        }
    } 
    #endregion

    void ModelMoveAndRotate()
    {
        //find the yaw and pitch
        var roll = Input.GetAxis("Horizontal") * Time.deltaTime;
        var pitch = Input.GetAxis("Vertical") * Time.deltaTime;
        rollrot = -roll * 2500;
        var pitchrot = -pitch * 1500;

        //moves the objects at a delay and rotates them accordingly
        birdModel.transform.position = Vector3.Lerp(birdModel.transform.position, gameObject.transform.position, Time.deltaTime);
        //

        //birdModel.transform.Rotate(0.0f, 0.0f, -Input.GetAxis("Horizontal"));

        //birdModel.transform.rotation = Quaternion.RotateTowards(birdModel.transform.rotation, Quaternion.Euler(new Vector3(pitchrot, 0, rollrot)), smoothRot * Time.deltaTime);
        birdModel.transform.LookAt(transform.position);
    }

    void CamFollow()
    {
        Vector3 camMove = birdModel.transform.position - transform.forward * 10 + Vector3.up * 5;
        float bias = 0.96f;
        Camera.main.transform.position = Camera.main.transform.position * bias + camMove * (1.0f - bias);
        Camera.main.transform.LookAt(birdModel.transform.position + transform.forward * 30);
    }
}
