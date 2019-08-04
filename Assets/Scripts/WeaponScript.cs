using System;
using System.Collections;
using System.Collections.Generic;
using DOT;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeaponScript : MonoBehaviour
{
    public bool activated;
    public float rotationSpeed;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (activated)
        {
            transform.localEulerAngles += rotationSpeed * Time.deltaTime * Vector3.forward;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Environment"))
        {
            _rb.Sleep();
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            _rb.isKinematic = true;
            activated = false;
        }
        
        if (other.collider.GetComponent<IDamage>() != null)
        {
            other.collider.GetComponent<IDamage>().Die();
        }
        
        if (other.collider.CompareTag("Debris"))
        {
            if (other.collider.GetComponent<Debris>() != null)
            {
                //break;
                other.collider.GetComponent<Debris>().Die();
                print("Death by balling");
            }


           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
