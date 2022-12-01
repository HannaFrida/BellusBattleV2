using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignButtons : MonoBehaviour
{
    RebindingDisplay rebindingDisplay;
    [SerializeField] Button jumpButton;
    // Start is called before the first frame update
    void Start()
    {
        rebindingDisplay = GetComponentInParent<RebindingDisplay>();
        jumpButton.onClick.AddListener(delegate { rebindingDisplay.StartJumpRebinding(); });

    }
}
