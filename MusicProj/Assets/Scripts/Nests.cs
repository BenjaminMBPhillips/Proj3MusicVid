using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nests : MonoBehaviour
{

    public AudioSource aud;

    public AudioClip nest1,
        nest2;

    public bool disperse;

    public GameObject bird1,
        bird2,
        bird1Mod,
        bird2Mod;

    // Use this for initialization
    void Start()
    {
        aud = GameObject.FindGameObjectWithTag("FX").GetComponent<AudioSource>();
        disperse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (disperse == false)
        {
            transform.Rotate(2, 2, 2);
        }

        else
        {
            var bird1rb = bird1.GetComponent<Rigidbody>();
            var bird2rb = bird2.GetComponent<Rigidbody>();

            /* TODO - make this work so birds stay in rotation
            bird2Mod.transform.rotation = Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, 0));
            bird1Mod.transform.rotation = Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, 0));
            */

            bird1.transform.parent = null;
            bird2.transform.parent = null;

            bird1rb.velocity = -bird1.transform.right * 50;
            bird2rb.velocity = -bird2.transform.right * 50;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            disperse = true;
            StartCoroutine(Destroy(3));
            if (aud.isPlaying)
            {

            }
            else
            {
                aud.clip = nest1;
                aud.Play();
            }
        }
    }

    IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bird1);
        Destroy(bird2);
        Destroy(gameObject);
    }

}
