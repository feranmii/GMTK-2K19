using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score;

    private void OnEnable()
    {
        OnDebrisDestroyed.RegisterListener(UpdateScore);
    }
    
    private void OnDisable()
    {
        OnDebrisDestroyed.UnregisterListener(UpdateScore);
    }
    
    public void UpdateScore(OnDebrisDestroyed onDebrisDestroyed)
    {
        score += 42;
        UIManager.Instance.UpdateScoreText(score);
        
    }
}
