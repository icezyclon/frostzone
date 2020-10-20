using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    [SerializeField] private float jumpForce = 11;
    [SerializeField] private float gravity = 0.3f;
    [SerializeField] private float glideGravity = 2;
    [SerializeField] private float maxGravity = 20;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayerMask;

    private Animator _animator;
    private Vector2 _velocity;
    private Rigidbody2D _rigidBody;
    private bool _isGrounded => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        ScoreScript score = GameObject.FindObjectOfType<ScoreScript>();
        if (score != null)
        {
            score.score += 1;
        }
        float xInput = Input.GetAxis("Horizontal");
        bool jumping = Input.GetAxis("Jump") > 0 ? true : false;
        bool shoot = Input.GetButton("Fire1");
        bool specialShot = Input.GetButton("Fire2");
        // bool special = Input.GetButtonDown("Special");
        float yMove;

        // SPECIAL
        // if (special)
        // {
        //     for (int i = 0; i < transform.parent.childCount; i++)
        //     {
        //         EnemyScript enemy = transform.parent.GetChild(i).GetComponent<EnemyScript>();
        //         if (enemy != null)
        //         {
        //             enemy.Freeze(5);
        //         }
        //     }
        // }

        bool shot = false;

        // SHOOTING
        if (shoot || specialShot)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                shot = weapon.Shoot(specialShot);
                if (shot)
                {
                    _animator.Play("PistolShoot");
                }
            }
        }

        // MOVEMENT
        if (jumping && _isGrounded && _velocity.y <= 0)
        {
            // JUMP
            yMove = jumpForce;
            if (!shot) _animator.Play("PistolJump");
            SoundMaker.Instance.Jump();
        }
        else if (jumping && !_isGrounded && _velocity.y <= -glideGravity)
        {
            // GLIDING
            yMove = -glideGravity;
            if (!shot) _animator.Play("PistolFlyFront");
        }
        else if (_isGrounded && _velocity.y <= 0)
        {
            // STANDING
            yMove = 0;
        }
        else
        {
            // NORMAL GRAVITY (can be mid jump with positiv y velocity)
            yMove = Math.Max(-maxGravity, _velocity.y - gravity);
        }

        _velocity = new Vector2(
            speed * xInput,
            yMove
        );

        // keep player from moving out of picture
        float distanceFromCamera = (transform.position - Camera.main.transform.position).z;
        float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceFromCamera)).x;
        float rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceFromCamera)).x;
        float topEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceFromCamera)).y;
        float bottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distanceFromCamera)).y;
        // and loop when falling down off screen
        transform.position = new Vector3(
          Mathf.Clamp(transform.position.x, leftEdge + 1, rightEdge - 1),
          transform.position.y < -bottomEdge - 1 ? -topEdge + 1 : transform.position.y,
          transform.position.z
        );

        _rigidBody.velocity = _velocity;
    }
}
