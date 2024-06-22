using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public int SquareSize = 3;
    public bool IsStartGame;
    List<GameObject> CellsObjects = new List<GameObject>();
    List<GameObject> ResurrectObjects = new List<GameObject>();
    List<GameObject> KillObjects = new List<GameObject>();
    Vector2 MousePosition;

    public Camera MainCamera;
    public GridLayout GameField;
    public Material LiveCell;
    public Material DieCell;
    public GameObject DieCellGameObject;

    void Start()
    {
        GameField = gameObject.GetComponent<Grid>();
        CreateStartSquare(SquareSize);
        MainCamera.transform.position = CellsObjects[CellsObjects.Count / 2 + SquareSize / 2].gameObject.transform.position;
        MainCamera.transform.Translate(0, 0, -10f);
    }
    void Update()
    {
        MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if(IsStartGame)
            GameCircle();
    }
    void CreateStartSquare(int sideLength)
    {
        float x = 3;
        float y = 3;
        Vector2 CellPosition = GameField.transform.position;
        for (int i = 0; i < sideLength; i++)
        {
            for (int j = 0; j < sideLength; j++)
            {
                CellPosition.x += x;
                CellsObjects.Add(Instantiate(DieCellGameObject, CellPosition, quaternion.identity));
            }
            CellPosition.x = 0;
            CellPosition.y += y;
        }
    }

    public void ClickCellWithMouse(GameObject cell)
    {
        if (IsStartGame == false)
        {
            Debug.Log(cell.gameObject.tag);
            if (cell.gameObject.tag == "LiveCell")
                CreateCell(cell, DieCell);
            if (cell.gameObject.tag == "DieCell")
                CreateCell(cell, LiveCell);
        }
    }
    public void DrawCellWithMouse(GameObject cell)
    {
        if (IsStartGame == false)
        {
            if (Input.GetMouseButton((int)MouseButton.Left) & cell.gameObject.tag == "DieCell")
                CreateCell(cell, LiveCell);
            if (Input.GetMouseButton((int)MouseButton.Right) & cell.gameObject.tag == "LiveCell")
                CreateCell(cell, DieCell);
        }
    }
    public void KillCell(GameObject cell)
    {
        int NeighbourLive = cell.GetComponent<CellBehavior>().NeighbourLive;

        if (((NeighbourLive > 3) | (NeighbourLive < 2)) & cell.tag == "LiveCell")
            CreateCell(cell, DieCell);
    }
    public void ResurrectCell(GameObject cell)
    {
        int NeighbourLive = cell.GetComponent<CellBehavior>().NeighbourLive;

        if (NeighbourLive == 3 & cell.tag == "DieCell")
            CreateCell(cell, LiveCell);
    }

    void CreateCell(GameObject cell, Material cellMaterial)
    {
        cell.GetComponent<MeshRenderer>().material = cellMaterial;
        cell.tag = cellMaterial.name;
    }

    public void GameCircle()
    {
        FindCollision();
        for (int i = 0; i < KillObjects.Count; i++)
             KillCell(KillObjects[i]);
        for (int i = 0; i < ResurrectObjects.Count; i++)
            ResurrectCell(ResurrectObjects[i]);
    }
    public void OneStepCircle()
    {
        GameCircle();
    }
    public void StopGame()
    {
        IsStartGame = false;
    }
    public void StartGameCircle()
    {
        if (IsStartGame)
            IsStartGame = false;
        else
            IsStartGame = true;
    }
    public void FindCollision()
    {
        ResurrectObjects.Clear();
        KillObjects.Clear();
        for (int i = 0; i < CellsObjects.Count; i++)
            CellsObjects[i].GetComponent<CellBehavior>().CalculateLivesNeighbours();

        for (int i = 0; i < CellsObjects.Count; i++)
            if (CellsObjects[i].gameObject.GetComponent<CellBehavior>().NeighbourLive == 3)
                ResurrectObjects.Add(CellsObjects[i]);
            else if ((CellsObjects[i].gameObject.GetComponent<CellBehavior>().NeighbourLive > 3) | CellsObjects[i].gameObject.GetComponent<CellBehavior>().NeighbourLive < 2) 
                KillObjects.Add(CellsObjects[i]);
    }

    public void ExitForGame()
    {
        Application.Quit();
    }

}


