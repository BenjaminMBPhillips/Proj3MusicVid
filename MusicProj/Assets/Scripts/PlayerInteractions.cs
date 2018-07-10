using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public AttemptFlight bird;

    public FlockManager flock;

    public AudioSource aud;

    public AudioClip water1,
        water2;

    public GameObject windPrefab,
        windParticle,
        windPos,
        waterPrefab,
        waterParticleL,
        waterParticleR,
        waterPosL,
        waterPosR,
        windTrailPrefab,
        windTrailL,
        windTrailR;

    public GameObject windline1L,
        windline1R;

    public float depleteFrom,
        timer;

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
            if (!windTrailR && !windTrailL)
            {
                windTrailL = Instantiate(windTrailPrefab, windline1L.transform, false);
                windTrailR = Instantiate(windTrailPrefab, windline1R.transform, false);
            }
        }

        else if (bird.speed < 50)
        {
            if (windTrailL && windTrailR)
            {
                windTrailL.transform.parent = null;
                windTrailR.transform.parent = null;
                StartCoroutine(Delete(windTrailL, 2));
                StartCoroutine(Delete(windTrailR, 2));
            }
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
        if (timer < 40)
        {
            Destroy(windParticle);
        }
    }

    #region TriggerInteraction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            StartCoroutine(InWater());
            if (!waterParticleL && !waterParticleR)
            {
                waterParticleL = Instantiate(waterPrefab, waterPosL.transform);
                waterParticleR = Instantiate(waterPrefab, waterPosR.transform);
                if (aud.isPlaying)
                {

                }
                else
                {
                    var rnd = Random.Range(0, 2);
                    if (rnd == 0)
                    {
                        aud.clip = water1;
                        aud.Play();
                    }
                    else
                    {
                        aud.clip = water2;
                        aud.Play();
                    }
                }
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
            StopCoroutine(InWater());
            StartCoroutine(Delete(waterParticleL, 1));
            StartCoroutine(Delete(waterParticleR, 1));
            waterParticleL.transform.parent = null;
            waterParticleR.transform.parent = null;
        }
    }
    #endregion

    IEnumerator Delete(GameObject toDelete, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(toDelete);
    }

    IEnumerator InWater()
    {
        yield return new WaitForSeconds(2);
        bird.inWater = true;
    }
}
