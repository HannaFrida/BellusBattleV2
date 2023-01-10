using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeMode : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsToDisable;
    [SerializeField] private List<GameObject> _objectsToEnable;

    private void Start()
    {
        if (GameManager.Instance._safeMode)
        {
            foreach (GameObject obj in _objectsToDisable)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in _objectsToEnable)
            {
                obj.SetActive(true);
            }
        }
    }
}
