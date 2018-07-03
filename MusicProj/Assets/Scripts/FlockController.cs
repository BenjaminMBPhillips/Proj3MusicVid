using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public int flocksize;

    public PlayerFlight playerCont;

    public List<GameObject> flockLeft = new List<GameObject>();
    public List<GameObject> flockRight = new List<GameObject>();

    public GameObject player,
        flockPrefab,
        flockToFollow,
        flockParent;

    public Vector3 rightOffset,
        RAdd,
        leftOffset,
        LAdd,
        offsetAdd;


    // Use this for initialization
    void Start()
    {
        flocksize = 0;
        playerCont = GameObject.FindGameObjectWithTag("Controller").GetComponent<PlayerFlight>();
        flockParent = GameObject.FindGameObjectWithTag("FlockParent");
    }

    public void NewFlockMember()
    {
        if(flockRight.Count == 0 && flockLeft.Count == 0)
        {
            flockToFollow = player;
        }

        if(flocksize%2 == 0)
        {
            var addedFlock = Instantiate(flockPrefab, flockParent.transform, false);
            var addedScript = flockToFollow.AddComponent<FlockMemberFlight>();
            addedScript.flyingBirb = addedFlock;
            flockRight.Add(addedFlock);
            rightOffset = rightOffset + RAdd;
            addedScript.offset = rightOffset;
            flocksize++;
        }

        else if (flocksize%2 == 1)
        {
            var addedFlock = Instantiate(flockPrefab, flockParent.transform, false);
            var addedScript = addedFlock.GetComponent<FlockMemberFlight>();
            flockLeft.Add(addedFlock);
            leftOffset = leftOffset + LAdd;
            addedScript.offset = leftOffset;
            flocksize++;
        }
    }
}
