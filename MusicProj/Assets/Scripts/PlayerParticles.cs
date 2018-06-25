using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public GameObject grassPos,
        cloudPos,
        grassPrefab,
        grass,
        cloudPrefab,
        cloud;

    public ParticleSystem ps;

    public float time;

    public bool deplete,
        add;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(deplete == true)
        {
            Reduce();
        }

        if(add == true)
        {
            Increase();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("grass"))
        {
            grass = Instantiate(grassPrefab, grassPos.transform);
            ps = grass.GetComponent<ParticleSystem>();
            add = true;
            time = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("grass"))
        {
            if (grass)
            {
                time = 100;
                deplete = true;
                StartCoroutine(Die(grass));
            }
        }
    }

    public IEnumerator Die(GameObject toDestroy)
    {
        yield return new WaitForSeconds(2);
        deplete = false;        
        Destroy(toDestroy);
    }

    void Reduce()
    {
        var emission = ps.emission;
        time -= Time.deltaTime * 100;
        emission.rateOverTime = time;
    }

    void Increase()
    {
        var emission = ps.emission;
        time += Time.deltaTime * 100;
        emission.rateOverTime = time;
        if(time >= 100)
        {
            add = false;
        }
    }
}
