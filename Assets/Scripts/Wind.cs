using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private GameObject[] players;
    [SerializeField] private float windForce;
    [SerializeField] private float windDuration;
    [SerializeField] private float timeBetweenWind;
    private bool isWindActive;
    private Vector2 windDirection;
    private float windTimer;
    private float currentTargetTime;
    // Start is called before the first frame update
    void Start()
    {
        windDirection = new Vector2(windForce, 0f);
        SetTargetTime();
    }

    // Update is called once per frame
    void Update()
    {
        players = GameManager.Instance.GetAllPlayers().ToArray();
        BlowAwayPlayers();
        RunTimer();
    }

    private void BlowAwayPlayers()
    {
        if (isWindActive == false) return;

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerMovement>().AddConstantExternalForce(windDirection);
        }
    }

    private void SwichWindDirection()
    {
        windDirection.x *= -1f;
    }

    private void RunTimer()
    {
        windTimer += Time.deltaTime;
        if (windTimer >= currentTargetTime)
        {
            isWindActive = !isWindActive;
            windTimer = 0f;
            SetTargetTime();
        }
    }

    private void SetTargetTime()
    {
        switch (isWindActive)
        {
            case true:
                currentTargetTime = windDuration;
                break;
            case false:
                currentTargetTime = timeBetweenWind;
                SwichWindDirection();
                break;
        }
    }
}
