using System.Collections.Generic;
using UnityEngine;

public class ColorPainter : MonoBehaviour
{
    public enum PaletteType
    {
        Pastel,
        Warm,
        Cool,
        Random
    }

    [SerializeField]
    private PaletteType paletteType =
        PaletteType.Random;

    [Header("最小領域サイズ")]
    [SerializeField]
    private int minRegionPixels = 120;

    [Header("パステル(8色)")]
    [SerializeField]
    private Color[] pastelColors =
    {
        new Color(1.00f,0.70f,0.70f),
        new Color(0.65f,0.80f,1.00f),
        new Color(0.70f,0.95f,0.75f),
        new Color(1.00f,0.90f,0.60f),
        new Color(0.85f,0.70f,1.00f),
        new Color(0.55f,0.90f,0.95f),
        new Color(1.00f,0.80f,0.85f),
        new Color(0.80f,1.00f,0.80f)
    };

    [Header("暖色(8色)")]
    [SerializeField]
    private Color[] warmColors =
    {
        new Color(0.90f,0.25f,0.25f),
        new Color(1.00f,0.45f,0.20f),
        new Color(1.00f,0.65f,0.25f),
        new Color(1.00f,0.82f,0.25f),
        new Color(0.95f,0.45f,0.45f),
        new Color(0.80f,0.22f,0.18f),
        new Color(0.95f,0.55f,0.35f),
        new Color(1.00f,0.78f,0.40f)
    };

    [Header("寒色(8色)")]
    [SerializeField]
    private Color[] coolColors =
    {
        new Color(0.20f,0.45f,0.95f),
        new Color(0.20f,0.70f,1.00f),
        new Color(0.20f,0.75f,0.65f),
        new Color(0.25f,0.55f,0.80f),
        new Color(0.45f,0.35f,0.90f),
        new Color(0.15f,0.60f,0.90f),
        new Color(0.30f,0.85f,0.90f),
        new Color(0.35f,0.55f,0.70f)
    };

    private Color[] currentPalette;

    public Texture2D Paint(
        Texture2D source,
        List<Region> regions)
    {
        Texture2D texture =
            new Texture2D(
                source.width,
                source.height
            );

        Color[] pixels =
            source.GetPixels();

        currentPalette =
            GetColors();

        /* 小さい領域を黒くして削除 */
        List<Region> validRegions =
            new List<Region>();

        List<Region> removeList =
            new List<Region>();

        foreach (Region region in regions)
        {
            if (region.pixels.Count >= minRegionPixels)
            {
                validRegions.Add(region);
            }
            else
            {
                foreach (Vector2Int pixel in region.pixels)
                {
                    int index =
                        pixel.x +
                        pixel.y * source.width;

                    pixels[index] = Color.black;
                }

                removeList.Add(region);
            }
        }

        foreach (Region region in removeList)
        {
            regions.Remove(region);
        }

        if (validRegions.Count == 0)
        {
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        int colorCount =
            Mathf.CeilToInt(
                validRegions.Count / 4f
            );

        colorCount =
            Mathf.Clamp(
                colorCount,
                2,
                currentPalette.Length
            );

        Region isshikiRegion =
            validRegions[
                Random.Range(0, validRegions.Count)
            ];

        int isshikiColor =
            Random.Range(0, colorCount);

        Dictionary<Region,int> colorMap =
            CreateColorPattern(
                validRegions,
                colorCount,
                isshikiRegion,
                isshikiColor
            );

        foreach (Region region in validRegions)
        {
            region.colorId =
                colorMap[region];

            region.isIsshiki =
                region == isshikiRegion;

            Color color =
                currentPalette[region.colorId];

            foreach (Vector2Int pixel in region.pixels)
            {
                int index =
                    pixel.x +
                    pixel.y * source.width;

                pixels[index] = color;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

    private Color[] GetColors()
    {
        PaletteType selected =
            paletteType;

        if (paletteType ==
            PaletteType.Random)
        {
            int random =
                Random.Range(0, 3);

            if (random == 0)
            {
                selected = PaletteType.Pastel;
            }
            else if (random == 1)
            {
                selected = PaletteType.Warm;
            }
            else
            {
                selected = PaletteType.Cool;
            }
        }

        switch (selected)
        {
            case PaletteType.Pastel:
                return pastelColors;

            case PaletteType.Warm:
                return warmColors;

            case PaletteType.Cool:
                return coolColors;

            default:
                return pastelColors;
        }
    }

    private Dictionary<Region,int> CreateColorPattern(
        List<Region> regions,
        int colorCount,
        Region isshikiRegion,
        int isshikiColor)
    {
        Dictionary<Region,int> result =
            new Dictionary<Region,int>();

        result[isshikiRegion] =
            isshikiColor;

        List<Region> order =
            new List<Region>();

        foreach (Region region in regions)
        {
            if (region != isshikiRegion)
            {
                order.Add(region);
            }
        }

        Shuffle(order);

        List<int> normalColors =
            new List<int>();

        for (int i = 0; i < colorCount; i++)
        {
            if (i != isshikiColor)
            {
                normalColors.Add(i);
            }
        }

        int[] useCount =
            new int[colorCount];

        useCount[isshikiColor] = 1;

        int index = 0;

        /* 各色最低2領域 */
        foreach (int color in normalColors)
        {
            for (int j = 0; j < 2; j++)
            {
                if (index >= order.Count)
                {
                    break;
                }

                result[order[index]] = color;
                useCount[color]++;
                index++;
            }
        }

        /* 残りは最少使用色へ均等配分 */
        while (index < order.Count)
        {
            int min =
                int.MaxValue;

            List<int> candidates =
                new List<int>();

            foreach (int color in normalColors)
            {
                if (useCount[color] < min)
                {
                    min = useCount[color];
                    candidates.Clear();
                    candidates.Add(color);
                }
                else if (useCount[color] == min)
                {
                    candidates.Add(color);
                }
            }

            int selected =
                candidates[
                    Random.Range(0, candidates.Count)
                ];

            result[order[index]] =
                selected;

            useCount[selected]++;
            index++;
        }

        return result;
    }

    private void Shuffle(
        List<Region> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int random =
                Random.Range(0, i + 1);

            Region temp = list[i];
            list[i] = list[random];
            list[random] = temp;
        }
    }

    public Color GetColor(
        int colorId)
    {
        if (currentPalette == null)
        {
            currentPalette =
                GetColors();
        }

        if (colorId < 0 ||
            colorId >= currentPalette.Length)
        {
            return Color.clear;
        }

        return currentPalette[colorId];
    }
}