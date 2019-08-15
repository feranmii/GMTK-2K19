using System;
using System.Collections;
using System.Collections.Generic;
using DOT;
using EventCallbacks;
using UnityEngine;
using Utils.Extensions;
using Random = UnityEngine.Random;

public class Debris : MonoBehaviour, IDamage
{
    public float rotateSpeed = 10f;
    public float debrisLifetime = 5;
    private Vector3 randRot;
    private bool canSpin = true;
    //Debris
    
    public GameObject brokenDebris;
    public AudioClip[] clips;
    public AudioSource audioSource;
    public void Damage(int amount, bool instantHit = false)
    {
        
    }

    private void OnEnable()
    {
        randRot = Random.insideUnitSphere;

        canSpin = true;
        StartCoroutine(TimeDestruction());
    }

    private void FixedUpdate()
    {
        if(canSpin)
            transform.Rotate(Time.deltaTime * transform.forward + randRot * rotateSpeed);
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

    public void Break()
    {
        //PlaySound();
        
        GameObject broken = Instantiate(brokenDebris, transform.position, transform.rotation);
        broken.transform.localScale = transform.localScale;
        Rigidbody[] rbs = broken.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(1200f, transform.position, 400f);
        }
        
        Die();
    }
    
    public void Die()
    {
        GameManager.Instance.DoScreenShake();

        PlaySound();
        
        OnDebrisDestroyed onDebrisDestroyed = new OnDebrisDestroyed();
        onDebrisDestroyed.FireEvent();
        
 
        
        gameObject.SetActive(false);

    }

  

    public void PlaySound()
    {
        var randClip = clips[0];//clips.RandomItem();

        audioSource.clip = randClip;

        audioSource.Play();

    }

    public IEnumerator TimeDestruction()
    {
        yield return new WaitForSeconds(debrisLifetime);
        
        gameObject.SetActive(false);
    }
}
