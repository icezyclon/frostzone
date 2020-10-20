using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] public int ttl = 30;

    [SerializeField] public float freezeTime = 5.0f;

    private bool _active = false;

    private float _freezeTimeLeft;


    public void Freeze()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            EnemyScript enemy = transform.parent.GetChild(i).GetComponent<EnemyScript>();
            if (enemy != null)
            {
                if (_active && !enemy.isFrozen)
                {
                    enemy.Freeze(_freezeTimeLeft, false);
                }
                else
                {
                    enemy.Freeze(_freezeTimeLeft);
                }
            }
        }
    }
    void Start()
    {
        _freezeTimeLeft = freezeTime;
        Destroy(gameObject, ttl);
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, -5);
        if (_active)
        {
            _freezeTimeLeft -= Time.deltaTime;
            Freeze();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            Freeze();
            _active = true;
            transform.position = new Vector3(
                500,
                200,
                0
            );
            Destroy(gameObject, freezeTime);
        }
    }
}
