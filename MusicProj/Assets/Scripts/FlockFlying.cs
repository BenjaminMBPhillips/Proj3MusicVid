using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockFlying : MonoBehaviour
{

    public FlockManager flockMan;

    public Vector3 offset;

    public GameObject player;

    public float speed;

    // Use this for initialization
    void Start()
    {
        flockMan = GameObject.FindGameObjectWithTag("FlockParent").GetComponent<FlockManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        speed = GameObject.FindGameObjectWithTag("Controller").GetComponent<AttemptFlight>().speed;
        Flight();
    }

    void Flight()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, speed * Time.deltaTime);
    }
}
