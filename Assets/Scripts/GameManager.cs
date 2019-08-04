using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utils.Extensions;

public class GameManager : SingletonBehaviour<GameManager>
{


    public Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    public void DoScreenShake(float duration, float strength)
    {
        cam.DOShakePosition(duration, strength).SetAutoKill(false);
    }
}
