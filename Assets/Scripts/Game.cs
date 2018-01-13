using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Game : NetworkBehaviour
{
    public GameObject blueSphere, redSphere;
    public GameObject turnColorImage;
    public Color blueColorImage;
    public Color redColorImage;
    public TMPro.TextMeshProUGUI textGuanyador;
    public GameObject finishedMenu;

    private int[,] table = new int[51, 51];    //0 azul; 1 rojo

    private GameObject[,] tableSphere;
    [SyncVar]
    private bool blueTurn;
    private int offsetSpheres = 25;


    private void Start()
    {
        if (isServer) blueTurn = true;

        if (blueTurn) turnColorImage.GetComponent<Image>().color = blueColorImage;
        else turnColorImage.GetComponent<Image>().color = redColorImage;
        for (int i=0; i<table.GetLength(1); i++)
        {
            for (int j = 0; j <table.GetLength(1); j++)
            {
                table[i, j] = -1;
            }
        }
        table[offsetSpheres, offsetSpheres] = 2;
        tableSphere = new GameObject[50, 50];
        finishedMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void addTable(float x, float y, GameObject sphere)
    {
        GameObject sphereToInstantiate;

        Destroy(sphere);

   //     if (blueTurn) table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 0;
  //      else table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 1;

        if (blueTurn) sphereToInstantiate = blueSphere;
        else sphereToInstantiate = redSphere;
        tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = Instantiate(sphereToInstantiate, new Vector3(x, y, 6.43f), transform.rotation);
        NetworkServer.Spawn(tableSphere[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres]);

        RpcAddTable(x, y, blueTurn);

        if (hasWon(Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres))
        {
            RpcPlayerHasWon(blueTurn);
        } else changeTurn(blueTurn);
    }

    [ClientRpc]
    private void RpcAddTable(float x, float y, bool isBlueTurn)
    {
        Debug.Log("ClientRpc: x:" + Convert.ToInt32(x) + ", y:" + Convert.ToInt32(y)+ " BlueTurn: " + isBlueTurn);
        if (isBlueTurn) table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 0;
        else table[Convert.ToInt32(x) + offsetSpheres, Convert.ToInt32(y) + offsetSpheres] = 1;
    }

    [ClientRpc]
    private void RpcPlayerHasWon(bool isBlueTurn)
    {
        if (isBlueTurn) textGuanyador.SetText("BLUE PLAYER WINS");
        else textGuanyador.SetText("RED PLAYER WINS");
        textGuanyador.gameObject.SetActive(true);
        Time.timeScale = 0f;
        finishedMenu.SetActive(true);
    }

    private bool hasWon(int x, int y)
    {
        if (lookLine(x, y, 0, 1)) return true;
        else if (lookLine(x, y, 1, 1)) return true;
        else if (lookLine(x, y, 1, 0)) return true;
        else if (lookLine(x, y, -1, 1)) return true;
        else return false;
    }

    private bool lookLine(int x, int y, int incX, int incY)
    {
        bool win = false;
        int sameColor;
        int contador = 0;
        
        //trobar inici fila i/o columna
        int i = x, j = y;
        while (i > 0 && i < table.GetLength(1)-1 && j > 0 && j < table.GetLength(1)-1)
        {
            i -= incX;
            j -= incY;
        }

        if (blueTurn) sameColor = 0;
        else sameColor = 1;
        while (i < table.GetLength(1) && j < table.GetLength(1) && i >= 0 && j >=0 && !win)
        {
            //       Debug.Log("TABLE["+(i - offsetSpheres) + "]["+ (j - offsetSpheres) + "]: "+table[i, j]);
            if (table[i, j] == sameColor) contador++;
            else contador = 0;
            if (contador == 5)
            {
                win = true;
                for (int k=0; k<5; k++)
                {
                    tableSphere[i, j].GetComponent<Renderer>().material.color = Color.yellow;
                    i -= incX;
                    j -= incY;
                }
            }
            i += incX;
            j += incY;
        }
        return win;
    }

    private void changeTurn(bool isBlueTurn)
    {
        RpcChangeTurn(isBlueTurn);

        if (isBlueTurn) blueTurn = false;
        else blueTurn = true;
    }

    [ClientRpc]
    private void RpcChangeTurn(bool isBlueTurn)
    {
        if (isBlueTurn)
        {
            turnColorImage.GetComponent<Image>().color = redColorImage;
        }
        else
        {
            turnColorImage.GetComponent<Image>().color = blueColorImage;
        }
    }

    public bool isMyTurn(int numPlayer)
    {
        if (numPlayer == 1 && blueTurn) return true;
        else if (numPlayer == 2 && !blueTurn) return true;
        else return false;
    }

    public bool isPositionPosible(float x, float y)
    {
        int posX = Convert.ToInt32(x) + offsetSpheres;
        int posY = Convert.ToInt32(y) + offsetSpheres;
        Debug.Log("posX: " + posX + "posY: " + posY);
        bool isPosible = false;

        if (table[posX + 1, posY] != -1 ) isPosible = true;
        else if (table[posX - 1, posY] != -1) isPosible = true;
        else if (table[posX, posY + 1] != -1) isPosible = true;
        else if (table[posX, posY - 1] != -1) isPosible = true;
        else if (table[posX + 1, posY + 1] != -1) isPosible = true;
        else if (table[posX - 1, posY + 1] != -1) isPosible = true;
        else if (table[posX + 1, posY - 1] != -1) isPosible = true;
        else if (table[posX - 1, posY - 1] != -1) isPosible = true;

        return isPosible;
    }

    public void Quit()
    {
        Application.Quit();
    }
}