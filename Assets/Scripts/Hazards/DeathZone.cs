using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private string DeathZoneType = "Unnamed Hazard"; // Anv?nds f?r att logga data
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameDataTracker.Instance.NewKillEvent(0, other.gameObject.GetComponent<PlayerDetails>().playerID, DeathZoneType, GameManager.Instance.RoundDuration);
            GameDataTracker.Instance.IncreasePlayersKilledByHazards();
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
