using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridIconManager : MonoBehaviour
{
    public float tileSize = 10;
    public GameObject tilePrefab;

    private List<GameObject> tiles = new List<GameObject>();

    public void SetTileIcon(PuzzleData _puzzle)
    {
        float tileOffset = tileSize - 7.5f;

        float posX;
        float posY = ((_puzzle.height / 2) * tileOffset) - (tileOffset / 2);

        int tileNum = 0;
        for (int y = 0; y < _puzzle.height; y++)
        {
            posX = -(((_puzzle.width / 2) * tileOffset) - (tileOffset / 2));
            for (int x = 0; x < _puzzle.width; x++)
            {
                // If tile is not empty
                if (_puzzle.grid[tileNum] != 0)
                {
                    // Create tile
                    GameObject tile = Instantiate(tilePrefab, transform);
                    tile.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, 0);
                    tile.GetComponent<RectTransform>().sizeDelta = new Vector2(tileSize, tileSize);
                    tiles.Add(tile);
                }

                posX += tileOffset;
                tileNum++;
            }
            posY -= tileOffset;
        }
    }

    public void ClearTileIcon()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            Destroy(tiles[i].gameObject);
        }

        tiles.Clear();
    }
}
