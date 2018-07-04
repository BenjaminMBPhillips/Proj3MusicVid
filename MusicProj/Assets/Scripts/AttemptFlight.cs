using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptFlight : MonoBehaviour
{
    public GameObject birdModel,
        modelContainer;

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

        birdModel.transform.LookAt(transform.position, transform.up);

        modelContainer.transform.Rotate(Input.GetAxis("Horizontal"), 0.0f, 0.0f);

        #region Attempts
        /*Vector3 target = transform.position - birdModel.transform.position;
       float step = speed * Time.deltaTime;
       var ZForce = -Input.GetAxis("Horizontal") * 100;

       birdModel.transform.Rotate(Vector3.forward, Time.deltaTime * ZForce);


       Vector3 newTarg = target + new Vector3(target.x, target.y, ZForce);
       Vector3 newdir = Vector3.RotateTowards(birdModel.transform.forward, newTarg, step, 0.0f);
       Vector3 zRot = new Vector3(birdModel.transform.rotation.x, birdModel.transform.rotation.y, -Input.GetAxis("Horizontal"));*/

        /*Vector3 target = transform.position - birdModel.transform.position;
        var yRot = target.y;
        var xRot = target.x;
        var zRot = -Input.GetAxis("Horizontal");
        Vector3 newDir = new Vector3(yRot, xRot, zRot);
        float step = speed * Time.deltaTime;
        Vector3 lookDir = Vector3.RotateTowards(birdModel.transform.forward, newDir, step, 0.0f);
        birdModel.transform.rotation = Quaternion.Euler(lookDir);*/


        // birdModel.transform.Rotate(0.0f, 0.0f, -Input.GetAxis("Horizontal"));

        // birdModel.transform.rotation = Vector3.RotateTowards(birdModel.transform.rotation, Quaternion.Euler(new Vector3(transform.position.x, transform.position.y, -Input.GetAxis("Horizontal"))));

        //birdModel.transform.rotation = Quaternion.RotateTowards(birdModel.transform.rotation, Quaternion.Euler(new Vector3(pitchrot, 0, rollrot)), smoothRot * Time.deltaTime); 
        #endregion
    }

    #region Camera
    void CamFollow()
    {
        Vector3 camMove = birdModel.transform.position - transform.forward * 10 + Vector3.up * 5;
        float bias = 0.96f;
        Camera.main.transform.position = Camera.main.transform.position * bias + camMove * (1.0f - bias);
        Camera.main.transform.LookAt(birdModel.transform.position + transform.forward * 30);
    }
    #endregion

   
}
