using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
/*
* Author Khaled Alraas
*/
public class LevelDetails : MonoBehaviour
{

    [SerializeField] private string scene;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    private Toggle toggle;
    private LevelManager levelManager;
    private Image image;
    private TextMeshProUGUI textMesh;
    private void Start()
    {
        //levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        toggle = GetComponent<Toggle>();
        image = GetComponent<Image>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = scene;
    }
    public bool GetToggle()
    {
        return toggle.isOn;
    }
    public string GetName()
    {
        return scene;
    }

    public void SetName(string scene)
    {
        this.scene = scene;
    }
    public void ToggleValueChanged()
    {
        //print(GameManager.Instance.scenesToChooseFrom.Count);
        if (GameManager.Instance.GetScencesList().Count == 1 && toggle.isOn == true)
        {
            return;
        }
        GameManager.Instance.ChangeScenesToChooseFrom(this);
        if (toggle.isOn) image.color = color1;
        else image.color = color2;
    }
}
