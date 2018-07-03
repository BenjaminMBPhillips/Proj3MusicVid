﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockFlight : MonoBehaviour
{

    public GameObject player;

    public float roll,
        pitch,
        smoothRot,
        smoothCam,
        yawrot,
        move;

    public bool enter,
        leave;

    public Vector3 offset,
        playerpos;

    public List<Vector3> playPos = new List<Vector3>();

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enter = true;
        playerpos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerpos = player.transform.position;

        if (enter == true)
        {
            gameObject.transform.position = player.transform.position + offset * 3;
            enter = false;
        }

        if (leave == true)
        {
            offset = offset * 5;
            StartCoroutine(Destroy());
            leave = false;
        }

        if (enter == false && leave == false)
        {
            //find the yaw and pitch
            roll = Input.GetAxis("Horizontal") * Time.deltaTime;
            pitch = Input.GetAxis("Vertical") * Time.deltaTime;
            var rollrot = -roll * 2500;
            var pitchrot = -pitch * 1500;


            //moves the objects at a delay and rotates them accordingly
          //  gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.transform.position + offset, move * Time.deltaTime);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position + offset, move);

            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.Euler(new Vector3(pitchrot, 0, rollrot)), smoothRot * Time.deltaTime);

            //gameObject.transform.LookAt(player.transform);
        }        
    }
    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}