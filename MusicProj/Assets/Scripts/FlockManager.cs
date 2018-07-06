using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{

    public GameObject player,
        flockPrefab;

    public int flockSize,
        left,
        right;

    public Vector3 offsetL,
        offsetR;

    // Use this for initialization
    void Start()
    {
        offsetR = new Vector3(4, 0, -5);
        offsetL = new Vector3(-4, 0, -5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToFlock()
    {
        if (left == 0 && right == 0)
        {
            flockSize++;
            left++;
            var newFlockMember = Instantiate(flockPrefab, player.transform, false);
            var newFlockScript = newFlockMember.GetComponent<FlockFlying>();
            newFlockScript.offset = offsetL;
        }

        else if (left > right)
        {
            flockSize++;
            right++;
            var newoffset = offsetR * right;
            var newFlockMember = Instantiate(flockPrefab, player.transform, false);
            var newFlockScript = newFlockMember.GetComponent<FlockFlying>();
            newFlockScript.offset = newoffset;
        }
        else
        {
            flockSize++;
            left++;
            var newOffset = offsetL * left;
            var newFlockMember = Instantiate(flockPrefab, player.transform, false);
            var newFlockScript = newFlockMember.GetComponent<FlockFlying>();
            newFlockScript.offset = newOffset;
        }
    }
}
