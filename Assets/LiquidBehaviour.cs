using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidBehaviour : MonoBehaviour
{
    private bool shouldLerp = false;

    public float lerpTime;
    public float timeStartedLerping;
    public Vector2 endPosition;
    private Vector2 startPosition;
    public PlayerController Player;

    private void StartLerping()
    {
        timeStartedLerping = Time.time;
        startPosition = transform.position;
        


    }
    private void Start()
    {
        StartLerping();
        
    }
    private void Update()
    {
        if (Player.firstMove)
        {
            Debug.Log("PLAYER MOVED");
            
            transform.position = Lerp(startPosition, endPosition, timeStartedLerping, lerpTime);
        }       
    }

    public Vector3 Lerp(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime=1)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        var result = Vector3.Lerp(start, end, percentageComplete);
        return result;
    }
}
