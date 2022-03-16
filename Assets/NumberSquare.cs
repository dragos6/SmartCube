using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSquare : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] int weight = 1;
    BoxCollider2D boxCollider;
    void OnCollisionEnter(Collision collision)
    {
        
    }

}
