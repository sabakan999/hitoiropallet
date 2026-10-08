using System.Collections.Generic;
using UnityEngine;

public class RegionDetector
{
    public List<Region> Detect(Texture2D texture)
    {
        List<Region> regions = new();

        int width = texture.width;
        int height = texture.height;

        bool[,] visited = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (visited[x, y])
                {
                    continue;
                }

                Color pixel = texture.GetPixel(x, y);

                if (pixel.grayscale < 0.2f)
                {
                    continue;
                }

                Region region = FloodFill(
                    texture,
                    visited,
                    x,
                    y,
                    regions.Count
                );

                regions.Add(region);
            }
        }

        SetNeighbors(
            texture,
            regions
        );

        return regions;
    }

    private Region FloodFill(
        Texture2D texture,
        bool[,] visited,
        int startX,
        int startY,
        int id)
    {
        Region region = new Region();

        region.id = id;

        Queue<Vector2Int> queue = new();

        float sumX = 0;
        float sumY = 0;

        queue.Enqueue(new Vector2Int(startX, startY));

        visited[startX, startY] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            region.pixels.Add(current);

            sumX += current.x;
            sumY += current.y;

            for (int i = 0; i < 4; i++)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];

                if (nx < 0 || nx >= texture.width ||
                    ny < 0 || ny >= texture.height)
                {
                    continue;
                }

                if (visited[nx, ny])
                {
                    continue;
                }

                Color pixel = texture.GetPixel(nx, ny);

                if (pixel.grayscale < 0.2f)
                {
                    continue;
                }

                visited[nx, ny] = true;

                queue.Enqueue(new Vector2Int(nx, ny));
            }
        }

        region.center = new Vector2(
            sumX / region.pixels.Count,
            sumY / region.pixels.Count
        );

        return region;
    }

    private void SetNeighbors(
        Texture2D texture,
        List<Region> regions)
    {
        int width = texture.width;
        int height = texture.height;

        int[,] regionMap = new int[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                regionMap[x, y] = -1;
            }
        }

        foreach (Region region in regions)
        {
            foreach (Vector2Int pixel in region.pixels)
            {
                regionMap[pixel.x, pixel.y] = region.id;
            }
        }

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        foreach (Region region in regions)
        {
            foreach (Vector2Int pixel in region.pixels)
            {
                for (int i = 0; i < 4; i++)
                {
                    int nx = pixel.x + dx[i];
                    int ny = pixel.y + dy[i];

                    if (nx < 0 || nx >= width ||
                        ny < 0 || ny >= height)
                    {
                        continue;
                    }

                    int neighborId = regionMap[nx, ny];

                    if (neighborId == -1)
                    {
                        continue;
                    }

                    if (neighborId == region.id)
                    {
                        continue;
                    }

                    if (!region.neighbors.Contains(neighborId))
                    {
                        region.neighbors.Add(neighborId);
                    }
                }
            }
        }
    }
}