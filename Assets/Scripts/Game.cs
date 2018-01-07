using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject blueSphere, redSphere;
    public GameObject turnColorImage;
    public Color blueColorImage;
    public Color redColorImage;

    private int[,] table = new int[51, 51];    //0 azul; 1 rojo
    private bool blueTurn;

    private void Start()
    {
        blueTurn = true;
        for (int i=0; i<table.GetLength(1); i++)
        {
            for (int j = 0; j <table.GetLength(1); j++)
            {
                table[i, j] = -1;
            }
        }
        table[24, 24] = 2;
    }

    public void addTable(float x, float y)
    {
        GameObject sphereToInstantiate;

        if (blueTurn) table[Convert.ToInt32(x) + 24, Convert.ToInt32(y) + 24] = 0;
        else table[Convert.ToInt32(x) + 24, Convert.ToInt32(y) + 24] = 1;

        if (blueTurn) sphereToInstantiate = blueSphere;
        else sphereToInstantiate = redSphere;
        Instantiate(sphereToInstantiate, new Vector3(x, y, 0), transform.rotation);
        Debug.Log("x: " + x + "y: " + y);

        changeTurn();
    }

    private bool isBlueTurn()
    {
        return blueTurn;
    }

    private void changeTurn()
    {
        if (blueTurn)
        {
            blueTurn = false;
            turnColorImage.GetComponent<Image>().color = redColorImage;
        }
        else
        {
            blueTurn = true;
            turnColorImage.GetComponent<Image>().color = blueColorImage;
        }
    }

    public bool isPositionPosible(float x, float y)
    {
        
        int posX = Convert.ToInt32(x) + 24;
        int posY = Convert.ToInt32(y) + 24;
        bool isPosible = false;

        if (table[posX + 1, posY] != -1 ) isPosible = true;
        else if (table[posX - 1, posY] != -1) isPosible = true;
        else if (table[posX, posY + 1] != -1) isPosible = true;
        else if (table[posX, posY - 1] != -1) isPosible = true;
        else if (table[posX + 1, posY + 1] != -1) isPosible = true;
        else if (table[posX - 1, posY + 1] != -1) isPosible = true;
        else if (table[posX + 1, posY - 1] != -1) isPosible = true;
        else if (table[posX - 1, posY - 1] != -1) isPosible = true;

        Debug.Log("posX:" +posX+"posY:"+posY+"   "+isPosible);

        return isPosible;
    }
}