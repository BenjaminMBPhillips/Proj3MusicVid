using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public CameraScript cam;

    public List<GameObject> flockLeft = new List<GameObject>();
    public List<GameObject> flockRight = new List<GameObject>();

    public GameObject flockPrefab,
        newFlockMember,
        player;

    public Vector3 flockPos,
        rightOffset,
        rightOffsetAdd,
        leftOffset,
        leftOffsetAdd;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToFlock()
    {
        var newflock = Instantiate(flockPrefab, player.transform, false);
        var newscript = newflock.GetComponent<FlockFlight>();

        if (flockLeft.Count >= flockRight.Count)
        {
            flockRight.Add(newflock);
            rightOffset = rightOffset + rightOffsetAdd;
            flockPos = player.transform.position + rightOffset;
            newflock.transform.position = flockPos;
            newscript.offset = rightOffset;
            cam.Add(newflock.transform);
        }
        else if (flockRight.Count > flockLeft.Count) 
        {
            flockLeft.Add(newflock);
            leftOffset = leftOffset + leftOffsetAdd;
            flockPos = player.transform.position + leftOffset;
            newflock.transform.position = flockPos;
            newscript.offset = leftOffset;
            cam.Add(newflock.transform);
        }
    }
}
