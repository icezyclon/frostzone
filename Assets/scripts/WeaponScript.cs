using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform shotPrefab;
    [SerializeField] private Transform specialShotPrefab;
    [SerializeField] private float fireRate = 0.25f;

    private float _shotCooldown = 0f;

    public bool CanShoot
    {
        get
        {
            return _shotCooldown <= 0f;
        }
    }
    public bool Shoot(bool special = false)
    {
        if (CanShoot)
        {
            _shotCooldown = fireRate;

            // Create and position new shot
            Transform shotTransform = Instantiate(special ? specialShotPrefab : shotPrefab) as Transform;
            shotTransform.position = transform.position;
            shotTransform.parent = transform.parent;

            if (special)
            {
                SoundMaker.Instance.IceShot();
            }
            else
            {
                SoundMaker.Instance.Shot();
            }

            // move in direction player is facing
            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                move.direction = this.transform.right;
            }
            return true;
        }
        return false;
    }

    void Update()
    {
        if (_shotCooldown >= 0)
        {
            _shotCooldown -= Time.deltaTime;
        }
    }
}
