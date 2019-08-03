using System;
using System.Collections;
using System.Collections.Generic;
using FO.Utilities;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingDebrisController : MonoBehaviour
{
    public ObjectPooler pooler;

    public float range;

    public bool beginSpawn;

    [MinMaxSlider(0,3)]
    public Vector2 spawnInterval;

    [MinMaxSlider(1,4)]
    public Vector2 spawnAmount;
    
     [MinMaxSlider(1,4)]
    public Vector2 spawnSize;

    private Player _player;

    public float playerTransformOffset;
    
    private void Awake()
    {
        pooler.Init();
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        StartCoroutine(SpawnDebris());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            //SpawnDebris();
        }
    }

    public IEnumerator SpawnDebris()
    {
        
        while (beginSpawn)
        {
            yield return new WaitForSeconds(spawnInterval.GetRandomFloatValue());
            
            for (int i = 0; i < spawnAmount.GetRandomValue(); i++)
            {
                GameObject debris = pooler.GetObject("Debris").gameObject;
                debris.transform.position = RandomSpawn();
                debris.transform.localScale = debris.transform.localScale * spawnSize.GetRandomValue();
            }

            yield return null;

        }

    }


    private Vector3 RandomSpawn()
    {
        
        var retVal = Vector3.zero;

        var playerPos = _player.transform.position + new Vector3(0, 0, playerTransformOffset);
        
        var randPos = Random.insideUnitSphere;
        randPos.y = 0;
        var pos = transform.position + playerPos + randPos * range;

        retVal = pos;
        return retVal;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(transform.position, transform.position * range);
    }
}
