using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GridMap : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 4;
    private MovableObject movableObject;
    Grid grid;

    public void Start()
    {
        grid = new Grid(width, height, cellSize, transform.position);
        //DrawGrid();
    }
    public void SetMovableObject(MovableObject mo)
    {
        movableObject = mo;
    }
    public void SS2(int w, int h, int value, bool move)
    {
        //if (!CheckIfCanDoSS2(w, h)) return;
        float tempw = 0;
        float temph = 0;
        for(int i = 0; i < w; i++)
        {
            if (i % 2 == 1)
            {
                tempw += grid.GetX(movableObject.transform.position + new Vector3(cellSize * (i + 1) / 2, 0)) * cellSize + transform.position.x + (cellSize * 0.5f);
                grid.SetValue(movableObject.transform.position + new Vector3(cellSize * (i + 1) / 2, 0), value);
            }
            else
            {
                tempw += grid.GetX(movableObject.transform.position - new Vector3(cellSize * i / 2, 0)) * cellSize + transform.position.x + (cellSize * 0.5f);
                grid.SetValue(movableObject.transform.position - new Vector3(cellSize * i / 2, 0), value);
            }
        }
        for (int i = 0; i < h; i++)
        {
            if (i % 2 == 1)
            {
                temph += grid.GetY(movableObject.transform.position + new Vector3(0, cellSize * (i + 1) / 2)) * cellSize + transform.position.y + (cellSize * 0.5f);
                grid.SetValue(GetMaouseWorldPosition() + new Vector3(0, cellSize * (i + 1) / 2), value);
            }
            else
            {
                temph += grid.GetY(movableObject.transform.position - new Vector3(0, cellSize * i / 2)) * cellSize + transform.position.y + (cellSize * 0.5f);
                grid.SetValue(movableObject.transform.position - new Vector3(0, cellSize * i / 2), value);
            }
        }
        if(move) movableObject.transform.position = new Vector2(tempw / w, temph / h);
    }
    private bool CheckIfCanDoSS2(int w, int h)
    {
        for (int i = 0; i < w; i++)
        {
            if (i % 2 == 1)
            {
                if (!(grid.GetValue(GetMaouseWorldPosition() + new Vector3(cellSize * i, 0)) == 0)) return false;
            }
            else
            {
                if (!(grid.GetValue(GetMaouseWorldPosition() + new Vector3(cellSize * i, 0)) == 0)) return false;
            }
        }
        for (int i = 0; i < h; i++)
        {
            if (!(grid.GetValue(GetMaouseWorldPosition() + new Vector3(0, cellSize * i)) == 0)) return false;
        }
        return true;
    }
    private void Update()
    {
        //TestGrid();
    }
    private void TestGrid()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GetMaouseWorldPosition(), 1);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(GetMaouseWorldPosition()));
        }
    }
    private Vector3 GetMaouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        return vec;
    }
    public int GetValue(Vector3 worldPosition)
    {
        return grid.GetValue(worldPosition);
    }
    public int GetX(Vector3 worldPosition)
    {
        return grid.GetX(worldPosition);
    }
    public int GetY(Vector3 worldPosition)
    {
        return grid.GetY(worldPosition);
    }
    public void DrawGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject g =GameObject.CreatePrimitive(PrimitiveType.Plane);
                g.transform.position = transform.position + new Vector3(x, y, -1f);
                g.transform.rotation = Quaternion.AngleAxis(90, Vector3.left);
                g.transform.localScale = Vector3.one * cellSize/10;
            }
        }
    }
}
// solution 1
//if (i % 2 == 1)
//{
//    tempw += grid.GetX(movableObject.transform.position + new Vector3(cellSize * (i + 1) / 2, 0)) * cellSize + transform.position.x + (cellSize * 0.5f);
//    grid.SetValue(movableObject.transform.position + new Vector3(cellSize * (i + 1) / 2, 0), value);
//}
//else
//{
//    tempw += grid.GetX(movableObject.transform.position - new Vector3(cellSize * i / 2, 0)) * cellSize + transform.position.x + (cellSize * 0.5f);
//    grid.SetValue(movableObject.transform.position - new Vector3(cellSize * i / 2, 0), value);
//}






// solution 2
//if (i % 2 == 1)
//{
//    tempw += grid.GetX(GetMaouseWorldPosition() + new Vector3(cellSize * (i + 1) / 2, 0)) * cellSize + transform.position.x + (cellSize * 0.5f);
//    grid.SetValue(GetMaouseWorldPosition() + new Vector3(cellSize * (i + 1) / 2, 0), value);
//}
//else
//{
//    tempw += grid.GetX(GetMaouseWorldPosition() - new Vector3(cellSize * i / 2, 0)) * cellSize + transform.position.x + (cellSize * 0.5f);
//    grid.SetValue(GetMaouseWorldPosition() - new Vector3(cellSize * i / 2, 0), value);
//}