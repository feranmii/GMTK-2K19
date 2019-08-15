using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventCallbacks;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class UIManager : SingletonBehaviour<UIManager>
{
    
    public Text text;

    public GameObject PauseMenu;
    
    public GameObject GameOverMenu;
    
    private Tweener _tweener;

    private void Start()
    {
        _tweener = text.transform.DOPunchScale(Vector3.one, .25f, 0).Pause().SetAutoKill(false);
        PauseMenu.gameObject.SetActive(false);
        OnGameOver.RegisterListener(GameOver);
    }

    public void UpdateScoreText(int scoreValue)
    {
        
        text.DOText(scoreValue.ToString(), .5f);
        _tweener.Restart();
    }

    public void TogglePause(bool toggle)
    {
        PauseMenu.gameObject.SetActive(toggle);
    }
    
    
    
    public void GameOver(OnGameOver over)
    {
        GameOverMenu.SetActive(true);
    }

    public IEnumerator ShowGameOver()
    {
        
        yield return  new WaitForSeconds(1.5f);
        GameOverMenu.SetActive(true);
        
    }
}
