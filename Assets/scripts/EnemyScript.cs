using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] public Transform frozenBlockPrefab;
    [SerializeField] public int ttl = 30;
    [SerializeField] public float minSpeed;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float yMovementFactor = 1.0f;
    [SerializeField] public float xMovementFactor = 1.0f;

    MoveScript _moveScript;

    private float _frozenTimer = 0.0f;

    private Transform _frozenBlockInstance;

    Animator _animator;

    bool _playSound = true;

    public bool isFrozen
    {
        get { return _frozenTimer > 0.0f; }
    }

    public void Freeze(float seconds, bool sound = true)
    {
        _frozenTimer = Math.Max(_frozenTimer, seconds);
        _playSound = sound;
    }
    void Start()
    {
        Destroy(gameObject, ttl);
        _moveScript = GetComponent<MoveScript>();
        _animator = GetComponent<Animator>();
        if (_moveScript != null)
        {
            _moveScript.speed = new Vector2(
                UnityEngine.Random.Range(minSpeed, maxSpeed),
                0
            );
            _moveScript.direction = new Vector2(-1, 1);
        }
    }



    void FixedUpdate()
    {
        if (_moveScript != null)
        {
            if (isFrozen)
            {
                _moveScript.speed.y = 0;
                _frozenTimer -= Time.deltaTime;
                if (_frozenBlockInstance == null)
                {
                    _frozenBlockInstance = Instantiate(frozenBlockPrefab) as Transform;
                    _frozenBlockInstance.position = transform.position;
                    _frozenBlockInstance.parent = transform;
                    _animator.enabled = false;
                    if (_playSound)
                    {
                        SoundMaker.Instance.IceHit();
                    }
                }
            }
            else
            {
                if (_frozenBlockInstance != null)
                {
                    _playSound = true;
                    _animator.enabled = true;
                    Destroy(_frozenBlockInstance.gameObject);
                }
                float xPosition = transform.position.x;
                float yPosition = ((float)Math.Sin(transform.position.x * xMovementFactor) * yMovementFactor);
                _moveScript.speed.y = yPosition;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
        if (player != null && !isFrozen)
        {
            Destroy(gameObject);
            // TODO: lose game
            SceneManager.LoadScene("MenuScene");
        }
    }
}
