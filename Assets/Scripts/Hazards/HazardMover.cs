using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMover : MonoBehaviour
{
    [SerializeField] private HazardWarner hazardWarner; //OBS om det finns flera movingdeathzones är det bara en som kan ha en hazardWarner!
    [SerializeField] private Transform highestPoint;
    [SerializeField] private float timeBetweenMoving;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float smoothTime;
    [SerializeField] private BoxCollider boxCollider;
    private float timer;
    private Vector3 moveVector;
    private Vector3 lowestPosition, highestPosition;
    [SerializeField] private bool hasReachedHighestPoint;
    private bool runTimer = true;
    private bool hasPlayedSound;
    private float warningTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        moveVector = new Vector3(0f, movingSpeed, 0f);
        lowestPosition = new Vector3(transform.position.x, transform.position.y - boxCollider.size.y / 2, 0f);
        highestPosition = new Vector3(transform.position.x, highestPoint.transform.position.y + boxCollider.size.y / 2, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBetweenMoving - timer <= warningTime)
        {
            ToggleHazardWarner(true);
        }
        if (runTimer == true)
        {
            timer += Time.deltaTime;
            if(timer >= timeBetweenMoving)
            {
                runTimer = false;
                timer = 0;
            }
        }
        else
        {
            MoveHazard();
        }
    }
    private void MoveHazard()
    {
        ToggleHazardWarner(false);
        PlaySound();
        if (hasReachedHighestPoint == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position, highestPosition, ref moveVector, smoothTime);
            if (boxCollider.bounds.max.y >= highestPoint.position.y)
            {
                moveVector = Vector3.zero;
                hasReachedHighestPoint = true;
                hasPlayedSound = false;
            }
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, lowestPosition, ref moveVector, smoothTime);
            if (transform.position.y <= lowestPosition.y + 0.2f)
            {
                moveVector = Vector3.zero;
                hasReachedHighestPoint = false;
                runTimer = true;
                hasPlayedSound = false;
            }
        }
    }

    private void PlaySound()
    {
        if (hasPlayedSound) return;
        Debug.Log("hehe41");

        if(hasReachedHighestPoint == true)
        {
            SoundManager.Instance.FadeOutLavaHazard(5f);
        }
        else
        {
            SoundManager.Instance.FadeInLavaHazard();
            Debug.Log("hehe400");
        }
        hasPlayedSound = true;
    }

    private void ToggleHazardWarner(bool toggle)
    {
        if (hazardWarner == null) return;
        hazardWarner.DisplayWarning(toggle);
    }
}
