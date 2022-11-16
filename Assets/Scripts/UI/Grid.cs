using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    Vector3 originPosition;
    private int[,] gridArrayValue;
    private TextMesh[,] debugTextArray;
    private int[,] gridArrayNumber;
    private Dictionary<int, GameObject> gridArrayDict = new Dictionary<int, GameObject>();

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        int temp = 0;
        gridArrayValue = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        gridArrayNumber = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Plane);
                g.transform.position = originPosition + new Vector3(x * cellSize + 0.5f, y * cellSize + 0.5f, +5f);
                g.transform.rotation = Quaternion.AngleAxis(90, Vector3.left);
                g.transform.localScale = Vector3.one * cellSize / 10;
                gridArrayNumber[x, y] = temp;
                temp++;
                gridArrayDict.Add(gridArrayNumber[x,y], g);
            }
        }
        for (int x = 0; x < gridArrayValue.GetLength(0); x++)
        {
            for (int y = 0; y < gridArrayValue.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(gridArrayValue[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 5, Color.black, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.black, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.black, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.black, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.black, 100f);
    }
    public void ChangeColor(GameObject gameObject, Color color)
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    private TextMesh CreateWorldText(string text, Transform parent, Vector3 localposition, int fontSize, Color color, TextAnchor textAnchor )
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent,false);
        transform.localPosition = localposition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);

    }
    public int GetX(Vector3 worldPosition)
    {
        return Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
    }
    public int GetY(Vector3 worldPosition)
    {
        return Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    private void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArrayValue[x, y] = value;
            if(value == 0)ChangeColor(gridArrayDict[gridArrayNumber[x, y]], Color.gray);
            if(value == 1)ChangeColor(gridArrayDict[gridArrayNumber[x, y]], Color.red);
            debugTextArray[x, y].text = gridArrayValue[x, y].ToString();
        }
    }
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArrayValue[x, y];
        }
        return -1;
    }
    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x,y);
    }
}
