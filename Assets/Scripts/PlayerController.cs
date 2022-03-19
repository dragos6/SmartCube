using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{


    private Vector3 origPos, targetPos;
    private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask colliderlayerMask;
    [SerializeField] float timeToMove = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip hitnumber;
    [SerializeField] AudioClip hitlocked;
    [SerializeField] AudioClip hitnormal;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip coin;
    [SerializeField] [Range(0f, 1f)] float nextSceneTimer = 1;
    [SerializeField] float playerJumpHeight = 2.2f;
    public int WinStatus = 0;
    public bool isMoving;
    public bool firstMove = false;
    private bool LeftCollision;
    private bool RightCollision;
    private bool playerAlive = true;
    private bool isMovingArc;
    AudioSource audioSource;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        Movement();          // Square Movement
        ProcessCollisions(); // Checks player collision with another square in the x axis
        if (WinStatus == 0)
        {
            playerAlive = false;
            Invoke("PlayerTransition", 0.5f);
            Invoke("LoadNextScene", nextSceneTimer);
        }

        else if (!playerAlive && WinStatus != 0)
        {
            Invoke("ResetCurrentLevel", nextSceneTimer);
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKey();
        }

    }
    private void ProcessCollisions()
    {
        float extraLenght = .1f;
        RaycastHit2D hitleft = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, (boxCollider.bounds.extents.x + extraLenght) * 3, colliderlayerMask);
        // draws a Ray to check for collisions left side of the player
        RaycastHit2D hitright = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, (boxCollider.bounds.extents.x + extraLenght) * 3, colliderlayerMask);
        // draws a Ray to check for collisions right side of the player
        RaycastHit2D hitdown = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, (boxCollider.bounds.extents.y +.1f), colliderlayerMask);
        Color rayColorLeft;
        Color rayColorRight;
        if (hitdown.collider != null && !isMovingArc)
        {
            isMoving = false;
        }
      else
        {
            isMoving = true;
        }
        if (hitdown.collider != null)
        {
            rayColorLeft = Color.green;
        }
        else
        {
            rayColorLeft = Color.red;
        }
        Debug.DrawRay(transform.position, Vector2.down * (boxCollider.bounds.extents.y+.1f), rayColorLeft);


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
            rayColorLeft = Color.green;
        }
        else
        {
            rayColorLeft = Color.red;
        }

        if (hitright.collider != null)
        {
            rayColorRight = Color.green;
        }
        else
        {
            rayColorRight = Color.red;
        }

        Debug.DrawRay(transform.position, Vector2.left * (boxCollider.bounds.extents.x + extraLenght) * 2, rayColorLeft);
        Debug.DrawRay(transform.position, Vector2.right * (boxCollider.bounds.extents.x + extraLenght) * 2, rayColorRight);
    }

    private void Movement()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isMoving && playerAlive)
        {
            firstMove = true;
            if (!LeftCollision)
            {
                StartCoroutine(MovePlayer(Vector3.left * 2, 1));
            }
            else
            {
                StartCoroutine(MovePlayer((Vector3.left + Vector3.up) * 2, playerJumpHeight));
            }

        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isMoving && playerAlive)
        {
            firstMove = true;
            if (!RightCollision)
            {
                StartCoroutine(MovePlayer(Vector3.right * 2, 1));
            }
            else
            {
                StartCoroutine(MovePlayer((Vector3.right + Vector3.up) * 2, playerJumpHeight));
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCurrentLevel();
        }
    }

    private void ResetCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private IEnumerator MovePlayer(Vector3 direction, float height)
    {
        isMoving = true;
        isMovingArc = true;
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
        isMovingArc = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            isMoving = false;
        }

        if (collision.gameObject.tag == "Liquid")
        {
            audioSource.Stop();
            audioSource.PlayOneShot(death);
            playerAlive = false;
            Invoke("ResetCurrentLevel", 1f);
        }
        else if (collision.gameObject.tag == "Friendly" && firstMove && !isMoving)
        {
            audioSource.PlayOneShot(hitnormal);
        }
        else if (collision.gameObject.tag == "NumberSquare" && firstMove)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(hitnumber);
        }
        else if (collision.gameObject.tag == "Locked" && firstMove)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(hitlocked);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoldCoin")
        {
            audioSource.Stop();
            audioSource.PlayOneShot(coin);
            Destroy(collision.gameObject);
        }
    }
    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x; // parabolic ecuation

        var mid = Vector2.Lerp(start, end, t);                            // liniar interpolation, used to draw the line from startpos to endpos in time t

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));  // return the moving behaviour to use in MovePlayer  
    }
    private void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }

    }
    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void PlayerTransition()
    {
        gameObject.SetActive(false);
    }
}
