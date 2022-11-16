using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCustimization : MonoBehaviour
{
    [SerializeField] List<CharacterItem> characterItems;

    [System.Serializable]
    public struct CharacterItem
    {
        [SerializeField] private string characterName;
        public List<GameObject> accessoryItems;
        public Material characterMaterial;
        public Material indicatorMaterial;
        public int characterIndex;
        public bool isAvailable;

        public void SetIsAvailable(bool toggle)
        {
            isAvailable = toggle;
        }
    }

    //Loops through acceossories list and activates accessories based on the CharacterIndex
    public void ActivateAccessories(int characterId, Renderer CharacterRenderer, TextMeshPro indicatorText)
    {
        foreach (CharacterItem item in characterItems)
        {
            if (characterId == item.characterIndex)
            {
                item.SetIsAvailable(false);
                ToggleItemsInList(item.accessoryItems, true);
                CharacterRenderer.material = item.characterMaterial;
                //indicator assignment
                indicatorText.text = "P" + (item.characterIndex+1);
                indicatorText.renderer.material = item.indicatorMaterial;
            }
            else
            {
                item.SetIsAvailable(true);
                ToggleItemsInList(item.accessoryItems, false);
            }
        }
    }

    // Toggle all GameObjects in list true or false
    public void ToggleItemsInList(List<GameObject> list, bool toggle)
    {
        foreach (GameObject gameObject in list)
        {
            if (toggle) { gameObject.SetActive(true); }
            else { gameObject.SetActive(false); }
        }
    }


}



