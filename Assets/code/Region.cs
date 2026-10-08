using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public int id;

    public List<Vector2Int> pixels = new();

    public Vector2 center;

    public List<int> neighbors = new();

    public int colorId;

    public bool isIsshiki;
}