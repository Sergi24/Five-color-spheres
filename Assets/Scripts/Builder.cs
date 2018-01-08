using UnityEngine;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
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
        if (game.GetComponent<Game>().isPositionPosible(transform.position.x, transform.position.y))
        {
            game.GetComponent<Game>().addTable(transform.position.x, transform.position.y);
            Destroy(gameObject);
        }
    }


    void OnMouseEnter()
    {
        if (game.GetComponent<Game>().isPositionPosible(transform.position.x, transform.position.y))
        {
            rend.material.color = temporalColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = initialColor;
    }
}
