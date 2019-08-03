using System;
using System.Collections;
using System.Collections.Generic;
using DOT;
using UnityEngine;
using Random = UnityEngine.Random;

public class Debris : MonoBehaviour, IDamage
{
    public float rotateSpeed = 10f;
    public float debrisLifetime = 5;
    private Vector3 randRot;
    private bool canSpin = true;
    
    
    public void Damage(int amount, bool instantHit = false)
    {
        
    }

    private void OnEnable()
    {
        randRot = Random.insideUnitSphere;

        canSpin = true;
        StartCoroutine(TimeDestruction());
    }

    private void Update()
    {
        if(canSpin)
            transform.Rotate(transform.forward + randRot * rotateSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        //var damageable = other.collider.GetComponent<IDamage>();
        var damageable = other.collider.GetComponent<Player>();
        if ( damageable != null )
        {
            damageable.Die();
            
        }

        if (other.collider.CompareTag("OutsideBoundary"))
        {
            gameObject.SetActive(false);
        }
        
        canSpin = false;
        //Die();
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }


    public IEnumerator TimeDestruction()
    {
        yield return new WaitForSeconds(debrisLifetime);
        
        gameObject.SetActive(false);
    }
}
