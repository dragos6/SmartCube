using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSquare : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] int weight = 1;
    SpriteRenderer spriteRender;
    SpriteRenderer childRender;
    public PlayerController Player;
    public bool isLocked;
    private bool WeightDecreased = false;

    private void Start()
    {
        Player.WinStatus += weight;
        spriteRender = GetComponent<SpriteRenderer>();
        childRender = GetComponentInChildren<SpriteRenderer>();
        ToggleNumbers();

    }
    private void Update()
    {

        if (!isLocked)
        {
            ToggleNumbers();
            //ToggleLocked();
        }
        else
        {
            ToggleLocked();
        }
        if (WeightDecreased)
        {

            Player.WinStatus -= 1;
            WeightDecreased = false;

        }

        if (weight == 0)
        {
            Destroy(this.gameObject, 0.2f);

        }
        

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isLocked)
        {
            weight -= 1;
            WeightDecreased = true;

        }
    }
    void ToggleNumbers()
    {
        foreach (Transform child in gameObject.transform)
            if (child.name == "Square_Nr5" && weight == 5)
            {
                child.gameObject.SetActive(enabled);
                spriteRender.color = new Color(0.952f, 0.929f, 0.788f, 1);
            }
            else if (child.name == "Square_Nr4" && weight == 4)
            {
                child.gameObject.SetActive(enabled);
                spriteRender.color = new Color(1f, 0.862f, 0.713f, 1);
            }
            else if (child.name == "Square_Nr3" && weight == 3)
            {
                child.gameObject.SetActive(enabled);
                spriteRender.color = new Color(0.819f, 0.780f, 0.729f, 1);
            }
            else if (child.name == "Square_Nr2" && weight == 2)
            {
                child.gameObject.SetActive(enabled);
                spriteRender.color = new Color(0.980f, 0.862f, 0.819f, 1);
            }
            else if (child.name == "Square_Nr1" && weight == 1)
            {
                child.gameObject.SetActive(enabled);
                spriteRender.color = new Color(1f, 0.767f, 0.705f, 1);
            }
    }
    void ToggleLocked()
    {
        //foreach (Transform child in gameObject.transform)
        spriteRender.color = new Color(0.925f, 0.858f, 0.294f, 1);
    }
}