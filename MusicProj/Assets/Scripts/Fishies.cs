using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishies : MonoBehaviour
{

    public bool swimming;
    public Rigidbody rb;
    public GameObject rotator;
    public List<GameObject> fishies = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rotator.transform.Rotate(0, 0, 12);

        if(swimming == true)
        {
            rb.velocity = -rb.transform.right * 500 * Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            transform.Rotate(0, 90, 0);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            print("yeet");            
            Explosion();
            rotator.transform.DetachChildren();
        }
    }

    public void Explosion()
    {
        foreach (GameObject fish in fishies)
        {
            var rb = fish.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            rb.AddExplosionForce(100, rotator.transform.position, 20, 10, ForceMode.Impulse);
            print("here");
        }
    }
}
