using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public AttemptFlight bird;

    public FlockManager flock;

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

    #region wind
    public void WindEffect()
    {
        if (bird.speed > 50)
        {
            windline3L.SetActive(true);
            windline3R.SetActive(true);
        }

        else if (bird.speed < 50)
        {
            if(windline3R.activeInHierarchy == true)
            {

                windline3L.transform.parent = null;
                windline3R.transform.parent = null;
            }
            //windline3L.SetActive(false);
            //windline3R.SetActive(false);
        }
    }    
    #endregion

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

    #region TriggerInteraction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            if (!waterParticleL && !waterParticleR)
            {
                waterParticleL = Instantiate(waterPrefab, waterPosL.transform);
                waterParticleR = Instantiate(waterPrefab, waterPosR.transform);
            }
        }

        if (other.gameObject.CompareTag("addflock"))
        {
            flock.AddToFlock();
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
    #endregion
}
