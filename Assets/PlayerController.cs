using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float timeToMove = 1f;
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private BoxCollider2D boxCollider;
    //[SerializeField] float distance = 3
    [SerializeField] private LayerMask colliderlayerMask;
    private bool LeftCollision;
    private bool RightCollision;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        ProcessMovement();  
        ProcessCollisions();

    }

    private void ProcessCollisions()
    {
        float extraLenght = .1f;
        RaycastHit2D hitleft = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, (boxCollider.bounds.extents.y + extraLenght) * 3, colliderlayerMask);
        RaycastHit2D hitright = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, (boxCollider.bounds.extents.y + extraLenght) * 3, colliderlayerMask);
        Color rayColorLeft;
        Color rayColorRight;
        if (hitleft.collider != null)
        {
            rayColorLeft = Color.yellow;
        }
        else
        {
            rayColorLeft = Color.cyan;
        }

        if (hitright.collider != null)
        {
            rayColorRight = Color.green;
        }
        else
        {
            rayColorRight = Color.red;
        }

        Debug.DrawRay(transform.position, Vector2.left * (boxCollider.bounds.extents.y + extraLenght) * 2, rayColorLeft);
        Debug.DrawRay(transform.position, Vector2.right * (boxCollider.bounds.extents.y + extraLenght) * 2, rayColorRight);

        if (hitleft.collider != null)
        {
            Debug.Log(hitleft.collider.name);
            LeftCollision = true;
            
        }
        else
        {
            LeftCollision = false;
        }
        if (hitright.collider != null)
        {
            Debug.Log(hitright.collider.name);
            RightCollision = true;
        }
        else
        {
            RightCollision = false;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void ProcessMovement()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isMoving)
        {
            if (!LeftCollision)
            {
                StartCoroutine(MovePlayer(Vector3.left * 2,1));
            }
            else
            {
                StartCoroutine(MovePlayer((Vector3.left+ Vector3.up) * 2, 3));
            }

        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isMoving)
        {
            if (!RightCollision)
            {
                StartCoroutine(MovePlayer(Vector3.right * 2, 1));
            }
            else
            {
                StartCoroutine(MovePlayer((Vector3.right+Vector3.up)*2, 3));
            }
        }
    }

    private IEnumerator MovePlayer(Vector3 direction, int height)
    {
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            //transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            transform.position = Parabola(origPos, targetPos, height, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }


    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }
}
