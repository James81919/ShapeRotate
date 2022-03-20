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

        for (int y = 0; y < _puzzle.height; y++)
        {
            posX = -(((_puzzle.width / 2) * tileOffset) - (tileOffset / 2));
            for (int x = 0; x < _puzzle.width; x++)
            {
                // Create tile
                GameObject tile = Instantiate(tilePrefab, transform);
                tile.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, 0);
                tile.GetComponent<RectTransform>().sizeDelta = new Vector2(tileSize, tileSize);
                tiles.Add(tile);

                posX += tileOffset;
            }
            posY -= tileOffset;
        }
    }
}
