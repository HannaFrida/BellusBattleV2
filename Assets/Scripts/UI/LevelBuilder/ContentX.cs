using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;
/*
* Author Khaled Alraas
*/
[ExecuteAlways]
public class ContentX : MonoBehaviour
{
    /*
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField, Range(1, 10)] private int widthInGridUnits = 1;
    [SerializeField, Range(1, 10)] private int heightInGridUnits = 1;
    private static int amountOfObjectToSpawn;
    private static List<float> xPos = new List<float>();
    private static List<float> yPos = new List<float>();
    private static List<string> prefabName = new List<string>();
    private static List<Transform> tranformList = new List<Transform>();
    private string textMesh;


    private void Start()
    {
        textMesh = prefab.name;
        OnSceneGUI();
    }
    void OnSceneGUI()
    {
        if (!Application.isPlayer)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
            for (int i = 0; i < amountOfObjectToSpawn; i++) 
            {
                if (prefabName.ElementAt(i).Equals(textMesh + "(Clone)")) 
                {
                    GameObject p = Instantiate(prefab, tranformList.ElementAt(i));
                    p.transform.position = new Vector2(xPos.ElementAt(i), yPos.ElementAt(i));
                }
            }
        }
    }
    static List<GameObject> p = new List<GameObject>();
    int temp = 0;
    public void SpawnObject()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 10;
        pos = Camera.main.ScreenToWorldPoint(pos);
        spawnPoint.transform.position = pos;
        p.Add(Instantiate(prefab, spawnPoint.transform));
        p.ElementAt(amountOfObjectToSpawn).transform.parent = null;
        p.ElementAt(amountOfObjectToSpawn).AddComponent<MovableObject>();
        p.ElementAt(amountOfObjectToSpawn).GetComponent<MovableObject>().widthInGridUnits = widthInGridUnits;
        p.ElementAt(amountOfObjectToSpawn).GetComponent<MovableObject>().heightInGridUnits = heightInGridUnits;
        if (p.ElementAt(amountOfObjectToSpawn).GetComponent<BoxCollider>() == null) p.ElementAt(amountOfObjectToSpawn).AddComponent<BoxCollider>();

        prefabName.Add(p.ElementAt(amountOfObjectToSpawn).name);
        tranformList.Add(p.ElementAt(amountOfObjectToSpawn).transform);
        amountOfObjectToSpawn++;
    }
    static ContentX()
    {
        EditorApplication.playmodeStateChanged += ModeChanged;
    }

    static void ModeChanged()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode &&
             EditorApplication.isPlaying)
        {
            for(int i = 0; i < p.Count; i++) 
            {
                xPos.Add(p.ElementAt(i).transform.position.x);
                yPos.Add(p.ElementAt(i).transform.position.y);
            }
        }
    }
    */
}
