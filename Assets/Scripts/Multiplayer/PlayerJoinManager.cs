using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class PlayerJoinManager : PlayerSpawnManager
{
    [SerializeField] GameObject characterLow;
    RebindingDisplay rbd;
    [SerializeField] private ControlChooser ctrlcc;

    [SerializeField] private GameObject player1UI;
    [SerializeField] private GameObject player2UI;
    [SerializeField] private GameObject player3UI;
    [SerializeField] private GameObject player4UI;
    [SerializeField] private GameObject firstTimePlayerJoinsGame;
    private static bool onceAGame;
    [SerializeField] private VisualEffect StartGameEffect;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject settingsMenu;
    public List<PlayerInput> listOfPlayers = new List<PlayerInput>();

    void OnPlayerJoined(PlayerInput playerInput)
    {
        
        PlayerDetails playerDetails = playerInput.gameObject.GetComponent<PlayerDetails>();
        // Set the player ID, add one to the index to start at Player 1
        playerDetails.playerID = playerInput.playerIndex + 1;
        playerDetails.SetDevice(playerInput.devices[0]);

        GameManager.Instance.AddPLayer(playerInput.gameObject);
        GameManager.Instance.AddInput(playerInput);
        listOfPlayers.Add(playerInput);
        ctrlcc.AddToList(playerInput);
        //Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        // Set the start spawn position of the player using the location at the associated element into the array.
        // So Player 1 spawns at the first Trasnform in the list, Player 2 on the second, and so forth.
        playerDetails.startPos = SpawnLocations[playerInput.playerIndex].position;
        ChooseActionMap(playerInput);

        if (playerDetails.playerID == 1 && firstTimePlayerJoinsGame != null)
        {
            if (!onceAGame)
            {
                soundManager.FirstPlayerSpawnedInSound();
                StartGameEffect.Play();
                onceAGame = true;
            }
           
            
            Destroy(firstTimePlayerJoinsGame);
        }

        Renderer renderer = playerInput.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        TextMeshPro indicatorText = playerInput.gameObject.GetComponentInChildren<TextMeshPro>();
        //Activates Player characteraccessories and assigns material based on characterIndex
        playerInput.gameObject.GetComponentInChildren<CharacterCustimization>().ActivateAccessories(playerInput.playerIndex, renderer, indicatorText);

    }

    private void ChooseActionMap(PlayerInput input)
    {
        if(settingsMenu.activeSelf == true || playMenu.activeSelf == true)
        {
            input.SwitchCurrentActionMap("Menu");
        }
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
