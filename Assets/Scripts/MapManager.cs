using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    private static int mapa;

    public int getMapa()
    {
        return mapa;
    }

    public void SetMapa1()
    {
        mapa = 0;
    }

    public void SetMapa2()
    {
        mapa = 1;
    }
}
