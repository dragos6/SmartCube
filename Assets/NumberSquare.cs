using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSquare : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] int weight = 1;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRender;
    Color colornr5 = new Color(0.952f, 0.929f, 0.788f, 1);
    public PlayerController TotalSum;
    private bool WeightDecreased = false;
    //public sumOfWeights addedWeight;

    private void Start()
    {
      TotalSum.WinStatus += weight;
      boxCollider = GetComponent<BoxCollider2D>();
      spriteRender = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (WeightDecreased)
        {
            TotalSum.WinStatus -= 1;
            WeightDecreased = false;
        }
        
        if (weight == 0)
        {
            Destroy(this.gameObject,0.3f);
        }
        ToggleNumbers();
    }
    void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("player stepped on top");
                weight -= 1;
                WeightDecreased = true;
            }
        }
    void ToggleNumbers()
        {
            foreach(Transform child in gameObject.transform)
           
                if(child.name == "Square_Nr5" && weight==5)
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
 }
