using UnityEngine;
using UnityEditor;
using System;

public class Game : MonoBehaviour
{
    private int[,] table = new int[50, 50];    //0 azul; 1 rojo
    public GameObject blueSphere, redSphere;

    private void Start()
    {

    }

    public void addTable(float x, float y)
    {
        GameObject sphereToInstantiate;

        table[(int)x+25, (int)y +25] = getColorSphere();

        if (getColorSphere() == 0) sphereToInstantiate = blueSphere;
        else sphereToInstantiate = redSphere;
        Instantiate(sphereToInstantiate, new Vector3(x, y, 0), transform.rotation);
    }

    private int getColorSphere()
    {
        return 1;
    }

    //  private void addTable(int x, int y, int color)
    //   {

    //  }
}