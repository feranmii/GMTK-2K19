using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

public class GameManager : SingletonBehaviour<GameManager>
{
    public Camera cam;

    public bool paused;
    private void Start()
    {
        cam = Camera.main;
        OnGameOver.RegisterListener(GameOverLogic);
    }

    public void DoScreenShake()
    {
        //cam.DOShakePosition(duration, strength).SetAutoKill(false);
        cam.GetComponent<Animator>().SetTrigger("Shake");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        
    }

    public void GameOverLogic(OnGameOver gameOver)
    {
        TimeScaleEvent timeScaleEvent = new TimeScaleEvent(0, 0, false, 0, true);
        timeScaleEvent.FireEvent();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
   
    }
    public void PauseGame()
    {
        if(paused)
            return;
        
        TimeScaleEvent timeScaleEvent = new TimeScaleEvent(0, 0, false, 0, true);
        timeScaleEvent.FireEvent();

        UIManager.Instance.TogglePause(true);
        paused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        if(!paused)
            return;

        
        UIManager.Instance.TogglePause(false);

        
        TimeScaleEvent timeScaleEvent = new TimeScaleEvent(1, 0, false, 0, true);
        timeScaleEvent.FireEvent();

        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

    public void RestartGame()
    {
        TimeScaleEvent timeScaleEvent = new TimeScaleEvent(1, 0, false, 0, true);
        timeScaleEvent.FireEvent();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void QuitToMenu()
    {
        TimeScaleEvent timeScaleEvent = new TimeScaleEvent(1, 0, false, 0, true);
        timeScaleEvent.FireEvent();
        SceneManager.LoadScene("Menu");
    }
    
}
