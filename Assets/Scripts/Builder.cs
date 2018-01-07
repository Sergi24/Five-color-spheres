using UnityEngine;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{

    public Color hoverColor;
    public Color temporalColor;

    private GameObject game;
    private Color initialColor;
    private Renderer rend;
    private GameObject defense;

    void Start()
    {
        rend = GetComponent<Renderer>();
        initialColor = rend.material.color;
        game = GameObject.Find("Game");
    }

    void OnMouseDown()
    {
        //      if (castleHealth.restarDiners(buildManager.GetDefensePrice()))
        //     {
        game.GetComponent<Game>().addTable(transform.position.x, transform.position.y);
        //     }

        Destroy(gameObject);
    }


    void OnMouseEnter()
    {
        rend.material.color = temporalColor;
    }

    void OnMouseExit()
    {
        rend.material.color = initialColor;
    }
}
