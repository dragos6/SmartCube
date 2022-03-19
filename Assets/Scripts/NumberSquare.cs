using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSquare : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] int weight = 1;
    SpriteRenderer spriteRender;
    [SerializeField] float DestroyDelay = 0.5f;
    public PlayerController Player;
    public bool isLocked;
    private bool WeightDecreased = false;
    public GameObject Nr1Sprite;
    public GameObject Nr2Sprite;
    public GameObject Nr3Sprite;
    public GameObject Nr4Sprite;
    public GameObject Nr5Sprite;
    public GameObject ActiveChild;
    private void Start()
    {
        Player.WinStatus += weight;
        spriteRender = GetComponent<SpriteRenderer>();
        Nr1Sprite = transform.GetChild(0).gameObject;
        Nr2Sprite = transform.GetChild(1).gameObject;
        Nr3Sprite = transform.GetChild(2).gameObject;
        Nr4Sprite = transform.GetChild(3).gameObject;
        Nr5Sprite = transform.GetChild(4).gameObject;
        ToggleNumbers();
        ActiveChild = GetComponent<GameObject>();
        GetActiveChild();

    }

    private void GetActiveChild()
    {

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.activeSelf == true)
            {
                ActiveChild = transform.GetChild(i).gameObject;
            }
        }
    }

    private void Update()
    {

        if (!isLocked)
        {
            gameObject.tag = "NumberSquare";
            ToggleNumbers();
        }
        else
        {
            gameObject.tag = "Locked";
            ToggleLocked();
        }
        if (WeightDecreased)
        {
            ActiveChild.SetActive(false);
            Player.WinStatus -= 1;
            WeightDecreased = false;

        }

        if (weight == 0)
        {
            spriteRender.enabled = false;
            Nr1Sprite.SetActive(false); 
            Destroy(gameObject, DestroyDelay);
        }


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isLocked && Player.firstMove)
        {
            weight -= 1;
            WeightDecreased = true;

        }
    }
    void ToggleNumbers()
    {
        switch (weight)
        {
            case 1:
                Nr1Sprite.SetActive(true);
                Nr2Sprite.SetActive(false);
                Nr1Sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 0.767f, 0.705f, 1);
                spriteRender.color = new Color(1f, 0.767f, 0.705f, 1);
                break;
            case 2:
                Nr2Sprite.SetActive(true);
                Nr3Sprite.SetActive(false);
                spriteRender.color = new Color(0.980f, 0.862f, 0.819f, 1);
                Nr2Sprite.GetComponent<SpriteRenderer>().color = new Color(0.980f, 0.862f, 0.819f, 1);
                break;
            case 3:
                Nr3Sprite.SetActive(true);
                Nr4Sprite.SetActive(false);
                spriteRender.color = new Color(0.819f, 0.780f, 0.729f, 1);
                Nr3Sprite.GetComponent<SpriteRenderer>().color = new Color(0.819f, 0.780f, 0.729f, 1);
                break;
            case 4:
                Nr4Sprite.SetActive(true);
                Nr5Sprite.SetActive(false);
                spriteRender.color = new Color(1f, 0.862f, 0.713f, 1);
                Nr4Sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 0.862f, 0.713f, 1);
                break;
            case 5:
                Nr5Sprite.SetActive(true);
                spriteRender.color = new Color(0.952f, 0.929f, 0.788f, 1);
                Nr5Sprite.GetComponent<SpriteRenderer>().color = new Color(0.952f, 0.929f, 0.788f, 1);
                break;
            case 0:
                break;
        }
    }
    void ToggleLocked()
    {
        spriteRender.color = new Color(0.925f, 0.858f, 0.294f, 1);
        ActiveChild.GetComponent<SpriteRenderer>().color = new Color(0.584f, 0.560f, 0.537f);
    }
}