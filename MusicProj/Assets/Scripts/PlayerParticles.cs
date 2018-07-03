using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public FlockController flock;

    public Flight bird;

    public GameObject grassPos,
        cloudPos,
        windcirclePos,
        grassPrefab,
        grass,
        cloudPrefab,
        cloud,
        windcircleprefab,
        windcircle;

    public ParticleSystem ps;

    public float time,
        cooldown;

    public bool deplete,
        add,
        boosting;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (deplete == true)
        {
            Reduce();
        }

        if (add == true)
        {
            Increase();
        }

      //  SpeedBurst();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("grass"))
        {
            if (!grass)
            {
                grass = Instantiate(grassPrefab, grassPos.transform);
                ps = grass.GetComponent<ParticleSystem>();
                add = true;
                time = 1;
            }
        }

        if (other.gameObject.CompareTag("cloud"))
        {
            if (!cloud)
            {
                cloud = Instantiate(cloudPrefab, cloudPos.transform);
                ps = cloud.GetComponent<ParticleSystem>();
                add = true;
                time = 1;
            }
        }

        if (other.gameObject.CompareTag("addflock"))
        {
            flock.NewFlockMember();
        }

        if (other.gameObject.CompareTag("removeflock"))
        {
           
        }

        if (other.gameObject.CompareTag("WindPoint"))
        {
            boosting = true;
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

        if (other.gameObject.CompareTag("cloud"))
        {
            if (cloud)
            {
                time = 100;
                deplete = true;
                StartCoroutine(Die(cloud));
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
        if (time >= 100)
        {
            add = false;
        }
    }

    /*void SpeedBurst()
    {
        if(boosting == true)
        {
            bird.baseSpeed = 50;
            windcircle = Instantiate(windcircleprefab, windcirclePos.transform);
            time = 100;
            boosting = false;
        }

        else if(bird.baseSpeed > 10)
        {
            bird.baseSpeed -= Time.deltaTime * cooldown;
            if (windcircle)
            {
                var wind = windcircle.GetComponent<ParticleSystem>();
                var windemission = wind.emission;
                time -= Time.deltaTime * 10;
                windemission.rateOverTime = time;
                if (time < 40)
                {
                    windcircle.transform.position = Vector3.Lerp(gameObject.transform.position, gameObject.transform.position + new Vector3(0,0,-10), Time.deltaTime);
                }
                if (time < 30)
                {
                    Destroy(windcircle);
                }
            }
        }
    }*/
    
}
