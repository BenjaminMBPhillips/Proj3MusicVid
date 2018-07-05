using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public AttemptFlight bird;

    public GameObject windPrefab,
        windParticle,
        windPos,
        waterPrefab,
        waterParticleL,
        waterParticleR,
        waterPosL,
        waterPosR;

    public GameObject windline1L,
        windline1R,
        windline2L,
        windline2R,
        windline3L,
        windline3R;

    public float depleteFrom;

    private void Start()
    {
        
    }

    private void Update()
    {
        WindEffect();
    }

    public void WindEffect()
    {
        if (bird.speed > 50)
        {
            windline3L.SetActive(true);
            windline3R.SetActive(true);
            /*if (!windParticle)
            {
                windParticle = Instantiate(windPrefab, windPos.transform);
                depleteFrom = 100;
            }*/
        }

        else if(bird.speed < 50)
        {
           // Deplete(windParticle);
            windline3L.SetActive(false);
            windline3R.SetActive(false);
        }  

       /* if(bird.speed > 40)
        {
            windline2L.SetActive(true);
            windline2R.SetActive(true);
        }

        else if(bird.speed < 40)
        {
            windline2L.SetActive(false);
            windline2R.SetActive(false);
        }

        if(bird.speed > 30)
        {
            windline1L.SetActive(true);
            windline1R.SetActive(true);
        }

        else if (bird.speed < 30)
        {
            windline1L.SetActive(false);
            windline1R.SetActive(false);
        }*/
    }    

    public void Deplete(GameObject particle)
    {
        var timer = depleteFrom;
        timer -= Time.deltaTime * 8;
        var emission = particle.GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = timer;
        depleteFrom = timer;
        if(timer < 40)
        {
            Destroy(windParticle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            if(!waterParticleL && !waterParticleR)
            {
                waterParticleL = Instantiate(waterPrefab, waterPosL.transform);
                waterParticleR = Instantiate(waterPrefab, waterPosR.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Destroy(waterParticleL);
            Destroy(waterParticleR);
        }
    }
}
