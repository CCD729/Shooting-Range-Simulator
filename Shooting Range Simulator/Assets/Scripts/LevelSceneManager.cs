﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    public Text scoreText, ammoText, timeText, EndScoreText;
    public GameObject ContinueButton, RestartButton, EndButton;
    public Image crossHair;
    public Camera cam;
    List<string> endList;

    void Start()
    {
        endList = new List<string> { "Bravo! ", "WTF? ", "Wow! ", "Awesome! " };
        ContinueButton.SetActive(false);
        RestartButton.SetActive(false);
        EndButton.SetActive(false);
        EndScoreText.enabled = false;
    }
    void Update()
    {

    }

    public void Pause()
    {
        scoreText.enabled = false;
        ammoText.enabled = false;
        timeText.enabled = false;
        crossHair.enabled = false;
        ContinueButton.SetActive(true);
        RestartButton.SetActive(true);
        EndButton.SetActive(true);
        cam.GetComponent<CameraController>().StopCam();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Resume()
    {
        scoreText.enabled = true;
        ammoText.enabled = true;
        timeText.enabled = true;
        crossHair.enabled = true;
        ContinueButton.SetActive(false);
        RestartButton.SetActive(false);
        EndButton.SetActive(false);
        cam.GetComponent<CameraController>().ResumeCam();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void End(float score)
    {
        scoreText.enabled = false;
        ammoText.enabled = false;
        timeText.enabled = false;
        crossHair.enabled = false;
        var random = new Random();
        int index = Random.Range(0,3);
        EndScoreText.text = endList[index] + "You got: " + score.ToString();
        EndScoreText.enabled = true;
        EndButton.SetActive(true);
        cam.GetComponent<CameraController>().StopCam();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Perfect()
    {
        scoreText.enabled = false;
        ammoText.enabled = false;
        timeText.enabled = false;
        crossHair.enabled = false;
        EndScoreText.text = "Oh Shit! You're a BADASS!";
        EndScoreText.enabled = true;
        EndButton.SetActive(true);
        cam.GetComponent<CameraController>().StopCam();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Restart()
    {
        SceneManager.LoadScene("Level");
    }
}
