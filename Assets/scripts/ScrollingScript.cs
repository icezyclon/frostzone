using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingScript : MonoBehaviour
{
    [SerializeField] public Vector2 speed = new Vector2(2, 0);
    [SerializeField] public Vector2 direction = new Vector2(-1, 0);

    [SerializeField] public bool isLinkedToCamera = false;

    [SerializeField] public bool isLooping = false;


    private List<SpriteRenderer> _children;

    void Start()
    {
        if (isLooping)
        {
            _children = new List<SpriteRenderer>();
            for (int i = 0; i < transform.childCount; i++)
            {
                SpriteRenderer child = transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (child != null)
                {
                    _children.Add(child);
                }
            }
            _children.Sort((c1, c2) =>
            {
                float dx = c1.transform.position.x - c2.transform.position.x;
                if (dx > 0)
                {
                    return 1;
                }
                else if (dx < 0)
                {
                    return -1;
                }
                return 0;

            });
        }
    }

    void FixedUpdate()
    {
        // Movement
        Vector3 movement = new Vector3(
          speed.x * direction.x,
          speed.y * direction.y,
          0);

        movement *= Time.deltaTime;
        transform.Translate(movement);

        // Move the camera
        if (isLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }


        if (isLooping)
        {
            SpriteRenderer leftMost = _children[0];
            if (leftMost != null && ScrolledOfScreen(leftMost))
            {
                SpriteRenderer rightMost = _children[_children.Count - 1];
                Vector3 rightMostSize = (rightMost.bounds.max - rightMost.bounds.min);
                float rightMostRightBorder = rightMost.transform.position.x + (rightMostSize.x / 2);
                Vector3 leftMostSize = (leftMost.bounds.max - leftMost.bounds.min);

                leftMost.transform.position = new Vector3(
                    rightMostRightBorder + (leftMostSize.x / 2),
                    leftMost.transform.position.y,
                    leftMost.transform.position.z
                );
                _children.Remove(leftMost);
                _children.Add(leftMost);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                SpriteRenderer child = transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (child != null && ScrolledOfScreen(child))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
    private bool ScrolledOfScreen(SpriteRenderer renderer)
    {
        Vector3 size = (renderer.bounds.max - renderer.bounds.min);
        float rightBorder = renderer.transform.position.x + (size.x / 2);
        float distanceFromCamera = (transform.position - Camera.main.transform.position).z;
        float leftCameraEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceFromCamera)).x;
        return rightBorder < leftCameraEdge;
    }
}

