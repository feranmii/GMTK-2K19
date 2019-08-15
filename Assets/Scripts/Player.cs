using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DOT;
using EventCallbacks;
using UnityEngine;

public class Player : MonoBehaviour, IDamage
{
    public bool invulnerable;
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
        if (!invulnerable)
            FirstPersonController.enabled = false;
        
        OnGameOver onGameOver = new OnGameOver();
        onGameOver.FireEvent();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Camera.main.DOShakePosition(.2f, 1f, 4, 2);

            FirstPersonController.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("OutsideBoundary"))
        {
            Die();
        }
    }
}
