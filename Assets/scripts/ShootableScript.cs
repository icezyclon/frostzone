using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableScript : MonoBehaviour
{

    [SerializeField] public int lives = 1;

    [SerializeField] public float shotFreezeTime = 2.0f;

    EnemyScript _enemyScript;

    void Start()
    {
        _enemyScript = GetComponent<EnemyScript>();
    }

    private bool isFrozen
    {
        get
        {
            return _enemyScript != null ? _enemyScript.isFrozen : false;
        }
    }

    private void Freeze()
    {
        if (_enemyScript != null)
        {
            _enemyScript.Freeze(shotFreezeTime);
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
        if (shot != null)
        {
            if (shot.freeze)
            {
                Freeze();
                Destroy(shot.gameObject);
            }
            else
            {
                lives -= 1;
                if (lives <= 0 || isFrozen)
                {
                    // this enemy is dead
                    ScoreScript score = GameObject.FindObjectOfType<ScoreScript>();
                    if (score != null)
                    {
                        score.score += 300;
                    }
                    SoundMaker.Instance.Kill();
                    Destroy(gameObject);
                }
                else
                {
                    SoundMaker.Instance.Hit();
                }
                // shoot through ice
                // but shot is destroyed if it hits anything else
                if (!isFrozen)
                {
                    Destroy(shot.gameObject);
                }
            }
        }
    }
}
