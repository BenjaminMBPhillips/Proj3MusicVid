using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockFlying : MonoBehaviour
{

    public FlockManager flockMan;

    public Vector3 offset;

    public GameObject player,
        waterPrefab,
        waterL,
        waterR;

    public float speed;

    public bool enter,
        exit;

    // Use this for initialization
    void Start()
    {
        flockMan = GameObject.FindGameObjectWithTag("FlockMan").GetComponent<FlockManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + offset * 5;
        enter = true;
        StartCoroutine(LeaveFlock(30));
    }

    // Update is called once per frame
    void Update()
    {
        speed = GameObject.FindGameObjectWithTag("Controller").GetComponent<AttemptFlight>().speed;
        if (enter == true)
        {
            speed = 1;
            transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, speed * Time.deltaTime);
        }
        else
        {
            Vector3 leaveOffset = -offset + new Vector3(0, -50, 0);
            transform.position = Vector3.Lerp(transform.position, player.transform.position + leaveOffset, 0.5f * Time.deltaTime);
        }
    }

    IEnumerator LeaveFlock(float timer)
    {
        yield return new WaitForSeconds(timer);
        enter = false;
        yield return new WaitForSeconds(3);
        if (flockMan.left == flockMan.right)
        {
            flockMan.right--;
            Destroy(gameObject);
        }
        else
        {
            flockMan.left--;
            Destroy(gameObject);
        }
    }

}
