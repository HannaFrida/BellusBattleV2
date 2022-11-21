using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private Vector3 difference = Vector3.zero;
    [Range(1, 10)] public int widthInGridUnits = 1;
    [Range(1, 10)] public int heightInGridUnits = 1;
    GridMap gridMap;
    private void Start()
    {
        gridMap = GameObject.FindGameObjectWithTag("GridMap").GetComponent<GridMap>();
    }
    private void OnMouseDown()
    {
        gridMap.SetMovableObject(this);
        gridMap.SS2(widthInGridUnits, heightInGridUnits, 0, false);
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }
    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        // see mouse position
        // get grid mouse is on
        // transform.position = the middle of the grid
    }
    private void OnMouseUp()
    {
        gridMap.SS2(widthInGridUnits, heightInGridUnits, 1, true);
        gridMap.SetMovableObject(null);
    }
}
