using UnityEngine;
using UnityEngine.Tilemaps;

public class InstantiateFrame : MonoBehaviour
{
    Skin skinType;

    Tile topleft;
    Tile top;
    Tile topright;
    Tile left;
    Tile right;
    Tile bottomleft;
    Tile bottom;
    Tile bottomright;
    Tile empty;

    public Tilemap mapFrame;
    public Tilemap mapBG;

    void Start()
    {
        skinType = FindObjectOfType<Master_Control>().skinArray[PlayerPrefs.GetInt("SkinIndex", 0)];
        topleft = skinType.TopLeft;
        top = skinType.Top;
        topright = skinType.TopRight;
        left = skinType.Left;
        right = skinType.Right;
        bottomleft = skinType.BottomLeft;
        bottom = skinType.Bottom;
        bottomright = skinType.BottomRight;
        empty = skinType.NoBlock;

        for (int i = 9; i > -13; i--)
        {
            for (int j = -6; j < 6; j++)
            {
                if (i == 9 & j == -6) { mapFrame.SetTile(new Vector3Int(j, i, 0), topleft); continue; }
                if (i == 9 & j == 5) { mapFrame.SetTile(new Vector3Int(j, i, 0), topright); continue; }
                if (i == -12 & j == -6) { mapFrame.SetTile(new Vector3Int(j, i, 0), bottomleft); continue; }
                if (i == -12 & j == 5) { mapFrame.SetTile(new Vector3Int(j, i, 0), bottomright); continue; }
                if (i == 9) { mapFrame.SetTile(new Vector3Int(j, i, 0), top); continue; }
                if (i == -12) { mapFrame.SetTile(new Vector3Int(j, i, 0), bottom); continue; }
                if (j == -6) { mapFrame.SetTile(new Vector3Int(j, i, 0), left); continue; }
                if (j == 5) { mapFrame.SetTile(new Vector3Int(j, i, 0), right); continue; }
                mapBG.SetTile(new Vector3Int(j, i, 0), empty);
            }
        }
    }
}
