using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This Script handles the cursor follow code, and some level of interaction with molecule nodes
public class CursorFollow : MonoBehaviour
{

    public PuzzleUI UI = null;
    public RectTransform rect = null;
    public Collider2D collide = null;

    public bool carrying = false;

    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.Find("PuzzleReadyCanvas").GetComponent<PuzzleUI>();
        rect = this.GetComponent<RectTransform>();
        collide = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = new Vector2();

        mousePosition.x = Input.mousePosition.x;
        mousePosition.y = Input.mousePosition.y;

        rect.position = mousePosition;
    }
}
