using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderVsIa : MonoBehaviour {

    public Color temporalColor;

    private GameObject game;
    private Color initialColor;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        initialColor = rend.material.color;
        game = GameObject.Find("Game");
    }

    void OnMouseDown()
    {
        if (game.GetComponent<GameVsIa>().isPositionPosible(game.GetComponent<GameVsIa>().getTable(), transform.position.x, transform.position.y) && game.GetComponent<GameVsIa>().getBlueTurn())
        {
            game.GetComponent<GameVsIa>().addTable(transform.position.x, transform.position.y);
            Destroy(gameObject);
        }
    }


    void OnMouseEnter()
    {
        if (game.GetComponent<GameVsIa>().isPositionPosible(game.GetComponent<GameVsIa>().getTable(), transform.position.x, transform.position.y) && game.GetComponent<GameVsIa>().getBlueTurn())
        {
            rend.material.color = temporalColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = initialColor;
    }

}
