using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnScript : MonoBehaviour
{
    [SerializeField] public Transform spawnPrefab;

    [SerializeField] public float interval;
    [SerializeField] public float intervalOffset;
    [SerializeField] public int intervalMissEvery;

    [SerializeField] public float yMax;
    [SerializeField] public float yMin;

    private float _time = 0;

    private Vector3 _spawnSize;
    private int _numberOfSpawn = 0;

    void Start()
    {
        SpriteRenderer renderer = spawnPrefab.GetComponent<SpriteRenderer>();
        _spawnSize = renderer.bounds.max - renderer.bounds.min;
        _time = -intervalOffset;
    }

    void Update()
    {
        if (_time >= interval)
        {
            _time = 0;
            _numberOfSpawn += 1;
            if (intervalMissEvery > 0 && _numberOfSpawn % intervalMissEvery == 0)
            {
                _numberOfSpawn = 0;
            }
            else
            {
                Spawn();
            }
        }
        else
        {
            _time += Time.deltaTime;
        }
    }

    void Spawn()
    {
        Transform spawn = Instantiate(spawnPrefab) as Transform;
        spawn.parent = transform;
        float distanceFromCamera = (transform.position - Camera.main.transform.position).z;
        float rightCameraEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceFromCamera)).x;
        spawn.position = new Vector3(
            rightCameraEdge + _spawnSize.x / 2,
            Random.Range(yMin, yMax),
            0);
    }
}
