using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    GameObject[] pauseObjects;
    GameObject[] playObjects;

    float Countdown = 0.2f;

	// Use this for initialization
	void Start () {
        //Pause controls
        Time.timeScale = 0;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        playObjects = GameObject.FindGameObjectsWithTag("ShowOnPlay");
        HidePaused();
        ShowPlay();
    }
	
	// Update is called once per frame
	void Update () {

        //Pause control
        if (Input.GetKeyDown("escape"))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ShowPaused();
   
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                HidePaused();
            }
        } 
    }

    public void PauseControl()
    //Determines if the game is paused or not
    {
        if (Time.timeScale == 1)
        {
            if (Countdown < 0)
            {
                Time.timeScale = 0;
                ShowPaused();
            }
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
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

    public void ShowPlay()
    {
        foreach (GameObject h in playObjects)
        {
            h.SetActive(true);
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

    //Hides object with ShowOnPlay tag
    public void HidePlay()
    {
        foreach (GameObject h in playObjects)
        {
            h.SetActive(false);
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Scene");
    }
    public void Resume()
    {
        Time.timeScale = 1;
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
