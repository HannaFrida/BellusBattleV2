using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
/*
* Author Hanna Rud�fors
*/
public class PlayerJoinManager : PlayerSpawnManager
{
    [SerializeField] GameObject characterLow;
    RebindingDisplay rbd;
    [SerializeField] private ControlChooser ctrlcc;
    [SerializeField] private SettingsUIHandler suih;

    [SerializeField] private GameObject player1UI;
    [SerializeField] private GameObject player2UI;
    [SerializeField] private GameObject player3UI;
    [SerializeField] private GameObject player4UI;
    [SerializeField] private GameObject firstTimePlayerJoinsGame;
    private static bool onceAGame;
    [SerializeField] private VisualEffect StartGameEffect;

    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject settingsMenu;
    public List<PlayerInput> listOfPlayers = new List<PlayerInput>();

    void OnPlayerJoined(PlayerInput playerInput)
    {
        
        PlayerDetails playerDetails = playerInput.gameObject.GetComponent<PlayerDetails>();
        // Set the player ID, add one to the index to start at Player 1
        playerDetails.playerID = playerInput.playerIndex + 1;
        playerDetails.SetDevice(playerInput.devices[0]);
        Debug.Log(suih.oneHandText.text + "is text");
        

        GameManager.Instance.AddPLayer(playerInput.gameObject);
        GameManager.Instance.AddInput(playerInput);
        listOfPlayers.Add(playerInput);
        ctrlcc.AddToList(playerInput);
        //Debug.Log("PlayerInput ID: " + playerInput.playerIndex);

        // Set the start spawn position of the player using the location at the associated element into the array.
        // So Player 1 spawns at the first Trasnform in the list, Player 2 on the second, and so forth.
        playerDetails.startPos = SpawnLocations[playerInput.playerIndex].position;
        ChooseActionMap(playerInput);

        if (playerDetails.playerID == 1 && firstTimePlayerJoinsGame != null  )
        {
            if (!onceAGame)
            {
                SoundManager.Instance.FirstPlayerSpawnedInSound();
                StartGameEffect.Play();
                onceAGame = true;
            }
                
            
           
            
            Destroy(firstTimePlayerJoinsGame);
        }

        Renderer renderer = playerInput.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        TextMeshPro indicatorText = playerInput.gameObject.GetComponentInChildren<TextMeshPro>();
        //Activates Player characteraccessories and assigns material based on characterIndex
        playerInput.gameObject.GetComponentInChildren<CharacterCustimization>().ActivateAccessories(playerInput.playerIndex, renderer, indicatorText);


        if (PlayerPrefs.GetInt("OneHandMode") == 1)
        {
            Debug.Log("ydada");
            playerInput.SwitchCurrentActionMap("PlayerAccessibilityLeft");
        }

    }

    private void ChooseActionMap(PlayerInput input)
    {
        if(settingsMenu.activeSelf == true || playMenu.activeSelf == true)
        {
            input.SwitchCurrentActionMap("Menu");
        }
    }

}
