using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CellBehavior : MonoBehaviour
{
    GameObject? GameController;
    public int NeighbourLive ;
    public int CountNeighbour;
    List<Collider2D> NeighboursCells = new List<Collider2D>();

    void Start()
    {
        GameController = FindObjectOfType<GameController>().gameObject;

    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void OnMouseUpAsButton()
    {
        GameController.GetComponent<GameController>().ClickCellWithMouse(gameObject);
    }

    void OnMouseEnter()
    {
        GameController.GetComponent<GameController>().DrawCellWithMouse(gameObject);
    }
    public void CalculateLivesNeighbours()
    {
        NeighboursCells = new List<Collider2D>(Physics2D.OverlapBoxAll(gameObject.transform.position, new Vector2(3, 3), 0));
        NeighboursCells.Remove(NeighboursCells.Find((colider) => colider.gameObject == gameObject));
        NeighbourLive = NeighboursCells.Where((colider) => colider.gameObject.tag == "LiveCell").Count();
        CountNeighbour = NeighboursCells.Count;
    }
}
