using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    //Pause and play objects
    GameObject[] pauseObjects;
    GameObject[] playObjects;
    GameObject[] creditObjects;

    //Intro screen image
    public Image introScreen;

    //Pausing coroutine, slowly decreases timescale
    IEnumerator ScaleTime(float start, float end, float time)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = -1f;

        while (timer < time)
        {
            Time.timeScale = Mathf.Lerp(start, end, timer / time);
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }
        Time.timeScale = end;
    }
    //Coroutine makes the pause menu wait before showing up after hitting pause
    IEnumerator SlowPause()
    {
        yield return new WaitForSecondsRealtime(1);
        ShowPaused();
    }
    //Coroutine makes the pause menu wait before showing up after hitting play
    IEnumerator SlowPlay()
    {
        yield return new WaitForSecondsRealtime(1);
        HidePaused();
    }

    // Use this for initialization
    void Start () {

        //Pause controls
        Time.timeScale = 0;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        HidePaused();

        //Main menu controls
        playObjects = GameObject.FindGameObjectsWithTag("ShowOnPlay");
        ShowPlay();

        creditObjects = GameObject.FindGameObjectsWithTag("Credits");
        HideCredits();
    }
	
	// Update is called once per frame
	void Update () {
        //If pause key is hit and timescale is 1 then run coroutine and show pause objects
        if (Input.GetKeyDown("escape") && Time.timeScale == 1)
        {
            StartCoroutine(ScaleTime(1.0f, 0.0f, 1.0f));
            StartCoroutine(SlowPause());
        }
        //If pause key is hit and timescale is 0 then run coroutine and hide pause objects
        if (Input.GetKeyDown("escape") && Time.timeScale == 0)
        {
            StartCoroutine(ScaleTime(0.0f, 0.1f, 0.1f));
            StartCoroutine(SlowPlay());
        }
    }
    //Fades image
    public void Fade()
    {
        introScreen.CrossFadeAlpha(0.0f, 2.0f, true);
    }

    //UI buttons/images/text management

    //shows objects with ShowOnPause tag
    public void ShowPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }
    //Shows objects with ShowOnPlay tag
    public void ShowPlay()
    {
        foreach (GameObject h in playObjects)
        {
            h.SetActive(true);
        }
    }
    //Hides object with ShowOnPlay tag
    public void HidePlay()
    {
        foreach (GameObject h in playObjects)
        {
            h.SetActive(false);
        }
    }
    //hides objects with ShowOnPause tag
    public void HidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }
    //Hides objects with credits tag
    public void HideCredits()
    {
        foreach (GameObject i in creditObjects)
        {
            i.SetActive(false);
        }
    }
    //Shows objects with credits tag
    public void ShowCredits()
    {
        foreach (GameObject i in creditObjects)
        {
            i.SetActive(true);
        }
    }

    //Scene management

    //Reloads scene
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //Sets timescale to 1 and runs pause menu coroutine
    public void Resume()
    {
        StartCoroutine(ScaleTime(0.0f, 1.0f, 1.0f));
        StartCoroutine(SlowPlay());
    }
    public void Play()
    {
        Time.timeScale = 1;
        Fade();
        HidePlay();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
