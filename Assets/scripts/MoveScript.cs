using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] public Vector2 speed = new Vector2(1, 0);
    [SerializeField] public Vector2 direction = new Vector2(-1, 0);

    private Vector2 _velocity;
    private Rigidbody2D _rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocity = new Vector2(
          speed.x * direction.x,
          speed.y * direction.y);
    }

    void FixedUpdate()
    {
        if (_rigidBody != null)
        {

            _rigidBody.velocity = _velocity;
        }
        else
        {
            // eg. Background
            Vector3 position = transform.position;
            position.x += _velocity.x;
            position.y += _velocity.y;
            transform.position = position;
        }
    }
}
