using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    //Pause and play objects
    GameObject[] pauseObjects;
    GameObject[] playObjects;

    //Pausing coroutine, slowly decreses timescale
    IEnumerator ScaleTime(float start, float end, float time)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < time)
        {
            Time.timeScale = Mathf.Lerp(start, end, timer / time);
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }

        Time.timeScale = end;
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
    }
	
	// Update is called once per frame
	void Update () {
        //If pause key is hit and timescale is 1 then run coroutine and show pause objects
        if (Input.GetKeyDown("escape") && Time.timeScale == 1)
        {
            StartCoroutine(ScaleTime(1.0f, 0.0f, 1.0f));
            ShowPaused();
        }
        //If pause key is hit and timescale is 0 then run coroutine and hide pause objects
        if (Input.GetKeyDown("escape") && Time.timeScale == 0)
        {
            StartCoroutine(ScaleTime(0.0f, 1.0f, 1.0f));
            HidePaused();
        }
    }

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
    //Scene changes
    public void MainMenu()
    {
        SceneManager.LoadScene("Scene");
    }
    //Sets timescale to 1 and hides pause menu
    public void Resume()
    {
        StartCoroutine(ScaleTime(0.0f, 1.0f, 1.0f));
        HidePaused();
    }
    public void Play()
    {
        Time.timeScale = 1;
    }
    public void Quit()
    {
        Application.Quit();
    }
 }
