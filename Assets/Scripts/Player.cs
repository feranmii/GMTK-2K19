using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DOT;
using EventCallbacks;
using UnityEngine;

public class Player : MonoBehaviour, IDamage
{
    public FirstPersonController FirstPersonController;

    private void Start()
    {
        FirstPersonController = GetComponent<FirstPersonController>();
    }

    
    
    public void Damage(int amount, bool instantHit = false)
    {
    }

    public void Die()
    {
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Camera.main.DOShakePosition(.2f, 1f, 4, 2);

            FirstPersonController.enabled = false;
        }
    }
}
