using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : PlayerSpawnManager
{
    [SerializeField] GameObject characterLow;
    RebindingDisplay rbd;

    [SerializeField] private GameObject player1UI;
    [SerializeField] private GameObject player2UI;
    [SerializeField] private GameObject player3UI;
    [SerializeField] private GameObject player4UI;

    public List<PlayerInput> listOfPlayers = new List<PlayerInput>();

    void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerDetails playerDetails = playerInput.gameObject.GetComponent<PlayerDetails>();
        // Set the player ID, add one to the index to start at Player 1
        playerDetails.playerID = playerInput.playerIndex + 1;
        playerDetails.SetDevice(playerInput.devices[0]);

        GameManager.Instance.AddPLayer(playerInput.gameObject);
        listOfPlayers.Add(playerInput);
        //Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        // Set the start spawn position of the player using the location at the associated element into the array.
        // So Player 1 spawns at the first Trasnform in the list, Player 2 on the second, and so forth.
        playerDetails.startPos = SpawnLocations[playerInput.playerIndex].position;

        // Rebind settings canvas in player posiotioning depending on PlayerID
        RectTransform rtf;
        rbd = GameObject.FindGameObjectWithTag("Rebind").GetComponent<RebindingDisplay>();
        rtf = rbd.panel.gameObject.GetComponent<RectTransform>();
        if (playerDetails.playerID == 1)
        {
            rtf.SetTop(0);
            rbd.playerIDText.text = playerDetails.playerID.ToString();
            //rbd.panel.transform.position = rbd.PosP1.transform.position;
        }
        else if (playerDetails.playerID == 2)
        {
            //rbd.panel.transform.position = rbd.PosP2.transform.position;
            rtf.SetLeft(960);
            rtf.sizeDelta = new Vector2(1920, 1080);
            rbd.playerIDText.text = playerDetails.playerID.ToString();
        }
        else if (playerDetails.playerID == 3)
        {
            //rbd.panel.transform.position = rbd.PosP3.transform.position;
            rtf.SetBottom(540);
            rtf.sizeDelta = new Vector2(1920, 1080);
            rbd.playerIDText.text = playerDetails.playerID.ToString();
        }
        else if (playerDetails.playerID == 4)
        {
            //rbd.panel.transform.position = rbd.PosP4.transform.position;
            rtf.SetLeft(960);
            rtf.SetBottom(540);
            rtf.sizeDelta = new Vector2(1920, 1080);
            rbd.playerIDText.text = playerDetails.playerID.ToString();
        }
        ActivateUI(playerDetails.playerID);

        rtf = null;
        rbd = null; 

        Renderer renderer = playerInput.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        TextMeshPro indicatorText = playerInput.gameObject.GetComponentInChildren<TextMeshPro>();
        //Activates Player characteraccessories and assigns material based on characterIndex
        playerInput.gameObject.GetComponentInChildren<CharacterCustimization>().ActivateAccessories(playerInput.playerIndex, renderer, indicatorText);

    }

    private void ActivateUI(int playerID)
    {
        if (playerID == 1)
        {
            player1UI.SetActive(true);
        }
        if (playerID == 2)
        {
            player2UI.SetActive(true);
        }
        if (playerID == 3)
        {
            player3UI.SetActive(true);
        }
        if (playerID == 4)
        {
            player4UI.SetActive(true);
        }
    }
}
