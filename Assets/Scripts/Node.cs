using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    private int[,] table;
    private int ultimaTiradaX, ultimaTiradaY;
    private int valorHeuristica;

    public Node(int[,] table, int ultimaTiradaX, int ultimaTiradaY)
    {
        this.table = table;
        valorHeuristica = 0;
        this.ultimaTiradaX = ultimaTiradaX;
        this.ultimaTiradaY = ultimaTiradaY;
    }

    public int getUltimaTiradaX()
    {
        return ultimaTiradaX;
    }

    public int getUltimaTiradaY()
    {
        return ultimaTiradaY;
    }

    public void setValorHeuristica(int valorHeuristica)
    {
        this.valorHeuristica = valorHeuristica;
    }

    public int getValorHeuristica()
    {
        return valorHeuristica;
    }

    public int[,] getTable()
    {
        int [,] tableClone = new int[51, 51];
        for (int i = 0; i < table.GetLength(1); i++)
        {
            for (int j = 0; j < table.GetLength(1); j++)
            {
                tableClone[i, j] = table[i, j];
            }
        }
        return tableClone;
    }
}
