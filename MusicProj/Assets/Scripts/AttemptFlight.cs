using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptFlight : MonoBehaviour
{
    public GameObject birdModel,
        modelContainer,
        modelContRotater;

    public PlayerInteractions particles;

    public float smoothRot,
        roll,
        timer,
        BRollCheck,
        speed,
        ySpeed,
        xSpeed,
        rollrot,
        BRoll;

    public Input keydownRight,
        keydownLeft;

    public Vector3 offset;

    public bool inbounds,
        barrelRoll,
        isBoosting,
        inWater;

    public Rigidbody rb;

    public float worldTime;
    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        inbounds = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ModelMoveAndRotate();
        CamFollow();
        worldTime += 1 * Time.deltaTime;
        if (isBoosting == false)
        {
            BRoll = 0;
        }
        if (worldTime > 85 && worldTime < 100)
        {
            barrelRoll = true;
        }
        else
        {
            barrelRoll = false;
            isBoosting = false;
        }

        if (worldTime > 154)
        {
            worldTime = 0;
        }
    }

    #region Movement
    void Movement()
    {
        var y = Input.GetAxis("Vertical");
        var x = Input.GetAxis("Horizontal");

        #region Speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ySpeed += Time.deltaTime * 10;
            if (ySpeed < 30)
            {
                BoostRoll();
            }
            if (ySpeed > 30)
            {
                isBoosting = false;
                ySpeed = 50;
            }
        }

        if (y < 0)
        {
            ySpeed += Time.deltaTime * 2;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                ySpeed += Time.deltaTime * 10;
                if (ySpeed < 30)
                {
                    BoostRoll();
                }
                if (ySpeed > 30)
                {
                    isBoosting = false;
                    ySpeed = 80;
                }
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && ySpeed > 15)
            {
                ySpeed = 15;
            }
        }
        else if (ySpeed > 0 && y == 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            if (ySpeed > 30)
            {
                ySpeed -= Time.deltaTime * 5;
            }
            else
            {
                ySpeed -= Time.deltaTime * 2;
            }
        }
        else if (ySpeed > 0 && y > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            if (ySpeed > 30)
            {
                ySpeed -= Time.deltaTime * 6;
            }
            else
            {
                ySpeed -= Time.deltaTime * 4;
            }
        }

        speed = 20 + ySpeed;

        if (speed < 20)
        {
            speed = 20;
        }
        #endregion

        #region Position


        if (inWater == true)
        {
            rb.velocity = transform.forward * speed + transform.up * speed * 2;
            StartCoroutine(ResetBoundsTimer());
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            rb.velocity = transform.forward * speed + transform.up * y * speed * 1.5f;
        }
        else
        {
            rb.velocity = transform.forward * speed + transform.up * y * speed;
        }

        if (inbounds == true)
        {
            transform.Rotate(0, x, 0);
        }
        else
        {
            var boundX = 70 * Time.deltaTime;
            transform.Rotate(0, boundX, 0);
            StartCoroutine(ResetBoundsTimer());
        }

        float CheckTerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        if (CheckTerrainHeight > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, CheckTerrainHeight + 1000, transform.position.z);
        }
        #endregion

    }
    #endregion

    #region rotation
    void ModelMoveAndRotate()
    {
        //moves the objects at a delay and rotates them accordingly
        birdModel.transform.position = Vector3.Lerp(birdModel.transform.position, gameObject.transform.position, Time.deltaTime);

        birdModel.transform.LookAt(transform.position, transform.up);

        float modelxRot = Input.GetAxis("Horizontal");

        if (barrelRoll == false)
        {
            timer = 2;
            if (Input.GetAxis("Horizontal") != 0)
            {
                modelContRotater.transform.Rotate(0.0f, 0.0f, -modelxRot);
               // modelContainer.transform.Rotate(modelxRot, 0, 0);
            }
            else
            {
                modelContRotater.transform.Rotate(0, 0, (modelContRotater.transform.rotation.y * 1) * 8);
                if (isBoosting == false)
                {
                    modelContainer.transform.Rotate((modelContainer.transform.rotation.y * 1) * 8, 0, 0);
                }
            }
        }
        else
        {
            BarrelRoll();
        }

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

    
    public void BarrelRoll()
    {
        isBoosting = true;
        BRoll += 2 * Time.deltaTime;
        modelContainer.transform.Rotate(BRoll, 0, 0);
    }

    public void BoostRoll()
    {
        isBoosting = true;
        BRoll += 13 * Time.deltaTime;
        modelContainer.transform.Rotate(BRoll, 0, 0);
    }

    #endregion

    #region Camera
    void CamFollow()
    {
        Vector3 camMove = birdModel.transform.position - transform.forward * 10 + Vector3.up * 5;
        float bias = 0.96f;
        Camera.main.transform.position = Camera.main.transform.position * bias + camMove * (1.0f - bias);
        Camera.main.transform.LookAt(birdModel.transform.position + transform.forward * 30);
    }
    #endregion

    IEnumerator ResetBoundsTimer()
    {
        yield return new WaitForSeconds(2);
        inbounds = true;
        inWater = false;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("bounds"))
        {
            inbounds = false;
        }
    }
}
