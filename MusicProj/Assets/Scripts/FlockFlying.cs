using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockFlying : MonoBehaviour
{
    public FlockManager flockMan;

    public Vector3 offset;

    public GameObject player,
        waterPrefab,
        waterLPos,
        waterLPart,
        waterRPos,
        waterRPart,
        model;

    public float speed;

    public bool enter,
        exit;

    // Use this for initialization
    void Start()
    {
        flockMan = GameObject.FindGameObjectWithTag("FlockMan").GetComponent<FlockManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + player.transform.TransformDirection(offset) * 5;
        enter = true;
        //StartCoroutine(LeaveFlock(30));
    }

    // Update is called once per frame
    void Update()
    {
        speed = GameObject.FindGameObjectWithTag("Controller").GetComponent<AttemptFlight>().speed;
        if (enter == true)
        {
            speed = 1;
            transform.position = Vector3.Lerp(transform.position, player.transform.position + player.transform.TransformDirection(offset), speed * 0.5f * Time.deltaTime);
        }
        else
        {
            Vector3 leaveOffset = -offset + new Vector3(0, -50, 0);
            transform.position = Vector3.Lerp(transform.position, player.transform.position + player.transform.TransformDirection(leaveOffset), 0.5f * Time.deltaTime);
        }
        Rotation();
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

    void Rotation()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {

        }
        else
        {
            model.transform.Rotate((model.transform.rotation.y) * 8, 0, 0);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            if(!waterLPart && !waterRPart)
            {
                waterLPart = Instantiate(waterPrefab, waterLPos.transform);
                waterRPart = Instantiate(waterPrefab, waterRPos.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            StartCoroutine(Delete(waterLPart, 0.5f));
            waterLPart.transform.parent = null;
            StartCoroutine(Delete(waterRPart, 0.5f));
            waterRPart.transform.parent = null;
        }
    }

    IEnumerator Delete(GameObject toDelete, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(toDelete);
    }
}