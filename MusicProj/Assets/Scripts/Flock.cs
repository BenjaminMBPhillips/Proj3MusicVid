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
        player,
        flockParent;

    public Vector3 flockPos,
        rightOffset,
        rightOffsetAdd,
        leftOffset,
        leftOffsetAdd,
        offsetAdd;

    public int flocksize;

    // Use this for initialization
    void Start()
    {
        flockside = true;
        flocksize = 0;
        cam = GameObject.FindGameObjectWithTag("Controller").GetComponent<PlayerFlight>();
        flockParent = GameObject.FindGameObjectWithTag("FlockParent");
    }
    
    public void AddToFlock()
    {
        if (flockside == true)
        {
            var newflock = Instantiate(flockPrefab, flockParent.transform, false);
            var newscript = newflock.GetComponent<FlockFlight>();
            flockRight.Add(newflock);
            rightOffset = rightOffset + rightOffsetAdd;
            flockPos = player.transform.position + rightOffset;
            newflock.transform.position = flockPos;
            newscript.offset = rightOffset;
            var newOffset = cam.camOffset + offsetAdd;
            cam.camOffset = newOffset;
            flocksize++;
        }
        else 
        {
            var newflock = Instantiate(flockPrefab, flockParent.transform, false);
            var newscript = newflock.GetComponent<FlockFlight>();
            flockLeft.Add(newflock);
            leftOffset = leftOffset + leftOffsetAdd;
            flockPos = player.transform.position + leftOffset;
            newflock.transform.position = flockPos;
            newscript.offset = leftOffset;
            var newOffset = cam.camOffset + offsetAdd;
            cam.camOffset = newOffset;
            flocksize++;
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
            flocksize--;
        }
        else
        {
            var removelast = flockLeft.Count;
            var removeflock = flockLeft[removelast - 1];
            var script = removeflock.GetComponent<FlockFlight>();
            script.leave = true;
            var Offset = cam.camOffset = offsetAdd;
            cam.camOffset = Offset;
            flocksize--;
        }
    }
}