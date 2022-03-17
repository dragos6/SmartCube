using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    
    private Vector3 origPos, targetPos;
    private BoxCollider2D boxCollider;
    
    [SerializeField] private LayerMask colliderlayerMask;
    [SerializeField] float timeToMove = 1f;
    public int sum = 0;
    private bool isMoving;
    private bool LeftCollision;
    private bool RightCollision;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
        Movement();          // Square Movement
        ProcessCollisions(); // Checks player collision with another square in the x axis

    }
    private void ProcessCollisions()
    {
        float extraLenght = .1f;
        RaycastHit2D hitleft = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, (boxCollider.bounds.extents.y + extraLenght) * 3, colliderlayerMask);
        // draws a Ray to check for collisions left side of the player
        RaycastHit2D hitright = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, (boxCollider.bounds.extents.y + extraLenght) * 3, colliderlayerMask);
        // draws a Ray to check for collisions right side of the player
        Color rayColorLeft;
        Color rayColorRight;
        ColorRays(extraLenght, ref hitleft, ref hitright, out rayColorLeft, out rayColorRight);
        CheckRayCollisions(hitleft, hitright);
    }

    private void CheckRayCollisions(RaycastHit2D hitleft, RaycastHit2D hitright)
    {
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

    private void ColorRays(float extraLenght, ref RaycastHit2D hitleft, ref RaycastHit2D hitright, out Color rayColorLeft, out Color rayColorRight)
    {
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
    }

    private void Movement()
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
            //transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));   // this makes the player move in a straight line
            transform.position = Parabola(origPos, targetPos, height, (elapsedTime / timeToMove)); // this makes the player move in an parabolic line 
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x; // parabolic ecuation

        var mid = Vector2.Lerp(start, end, t);                            // liniar interpolation, used to draw the line from startpos to endpos in time t

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));  // return the moving behaviour to use in MovePlayer  
    }
   
}
