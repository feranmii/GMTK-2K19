using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    private Camera _cam;
    private Rigidbody _weaponRb;
    private WeaponScript _weaponScript;

    private float returnTime;

    private Vector3 originalLocPos;
    private Vector3 originalLocRot;
    private Vector3 pullPosition;
    
    [Header("Public References")]
    public Transform weapon;
    public Transform hand;
    public Transform spine;
    public Transform curvePoint;
    
    [Space]
    [Header("Parameters")]
    public float throwPower = 30;
    
    public bool pulling = false;

    public bool hasWeapon;

    public float throwDuration = 2f;
    private float _t;
    
    void Start()
    {
        Cursor.visible = false;

        _cam = Camera.main;
        _weaponRb = weapon.GetComponent<Rigidbody>();
        _weaponScript = weapon.GetComponent<WeaponScript>();
        originalLocPos = weapon.localPosition;
        originalLocRot = weapon.localEulerAngles;

    }

    private void Update()
    {

        if (hasWeapon)
        {
            if (Input.GetMouseButtonDown(0))
            {
                WeaponThrow();
            }
        }
        else
        {
            _t += Time.deltaTime;

            if (_t >= throwDuration)
            {
                WeaponStartPull();
                _t = 0;
            }
        }
        
        
        if (pulling)
        {
            if(returnTime < 1)
            {
                weapon.position = GetQuadraticCurvePoint(returnTime, pullPosition, curvePoint.position, hand.position);
                returnTime += Time.deltaTime * 1.5f;
            }
            else
            {
                WeaponCatch();
            }
        }
    }
    
    public void WeaponThrow()
    {
        hasWeapon = false;
        _weaponScript.activated = true;
        _weaponRb.isKinematic = false;
        _weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        weapon.parent = null;
        weapon.eulerAngles = new Vector3(0, -90 +transform.eulerAngles.y, 0);
        weapon.transform.position += transform.right/5;
        _weaponRb.AddForce(_cam.transform.forward * throwPower + transform.up * 2, ForceMode.Impulse);

        //Trail
        //trailRenderer.emitting = true;
        //trailParticle.Play();
    }

    public void WeaponStartPull()
    {
        pullPosition = weapon.position;
        _weaponRb.Sleep();
        _weaponRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        _weaponRb.isKinematic = true;
        weapon.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        weapon.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        _weaponScript.activated = true;
        pulling = true;
    }

    public void WeaponCatch()
    {
        returnTime = 0;
        pulling = false;
        weapon.parent = hand;
        _weaponScript.activated = false;
        weapon.localEulerAngles = originalLocPos;
        weapon.localPosition = originalLocRot;
        hasWeapon = true;

        GameManager.Instance.DoScreenShake();
        //Particle and trail
        //catchParticle.Play();
        //trailRenderer.emitting = false;
        //trailParticle.Stop();

        //Shake
        //impulseSource.GenerateImpulse(Vector3.right);

    }

    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
    
}
