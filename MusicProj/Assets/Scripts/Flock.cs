using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public PlayerFlight cam;

    public List<GameObject> flockLeft = new List<GameObject>();
    public List<GameObject> flockRight = new List<GameObject>();

    public bool flockside;

    public GameObject flockPrefab,
        newFlockMember,
        player;

    public Vector3 flockPos,
        rightOffset,
        rightOffsetAdd,
        leftOffset,
        leftOffsetAdd,
        offsetAdd;

    // Use this for initialization
    void Start()
    {
        flockside = true;
        cam = GameObject.FindGameObjectWithTag("Controller").GetComponent<PlayerFlight>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToFlock()
    {
        if (flockside == true)
        {
            var newflock = Instantiate(flockPrefab, player.transform, true);
            var newscript = newflock.GetComponent<FlockFlight>();
            flockRight.Add(newflock);
            rightOffset = rightOffset + rightOffsetAdd;
            flockPos = player.transform.position + rightOffset;
            newflock.transform.position = flockPos;
            newscript.offset = rightOffset;
            var newOffset = cam.camOffset + offsetAdd;
            cam.camOffset = newOffset;
        }
        else 
        {
            var newflock = Instantiate(flockPrefab, player.transform, true);
            var newscript = newflock.GetComponent<FlockFlight>();
            flockLeft.Add(newflock);
            leftOffset = leftOffset + leftOffsetAdd;
            flockPos = player.transform.position + leftOffset;
            newflock.transform.position = flockPos;
            newscript.offset = leftOffset;
            var newOffset = cam.camOffset + offsetAdd;
            cam.camOffset = newOffset;
        }
    }

    public void RemoveFromFlock()
    {
        if(flockside == true)
        {
            var removelast = flockRight.Count;
            var removeflock = flockRight[removelast - 1];
            var script = removeflock.GetComponent<FlockFlight>();
            script.leave = true;
            var Offset = cam.camOffset - offsetAdd;
            cam.camOffset = Offset;
        }
        else
        {
            var removelast = flockLeft.Count;
            var removeflock = flockLeft[removelast - 1];
            var script = removeflock.GetComponent<FlockFlight>();
            script.leave = true;
            var Offset = cam.camOffset = offsetAdd;
            cam.camOffset = Offset;
        }
    }
}
