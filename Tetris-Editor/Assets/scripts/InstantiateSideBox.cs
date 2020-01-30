using UnityEngine;
using UnityEngine.Tilemaps;

public class InstantiateSideBox : MonoBehaviour
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

    Tile one;
    Tile two;
    Tile three;
    Tile four;
    Tile five;
    Tile six;
    Tile seven;
    Tile eight;
    Tile nine;

    public Tilemap map;

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

        one = skinType.one;
        two = skinType.two;
        three = skinType.three;
        four = skinType.four;
        five = skinType.five;
        six = skinType.six;
        seven = skinType.seven;
        eight = skinType.eight;
        nine = skinType.nine;

        #region Hold box
        for (int i = 8; i > 2; i--)
        {
            for (int j = -16; j < -7; j++)
            {
                if (i == 8 & j == -16) { map.SetTile(new Vector3Int(j, i, 0), topleft); continue; }
                if (i == 8 & j == -8) { map.SetTile(new Vector3Int(j, i, 0), topright); continue; }
                if (i == 3 & j == -16) { map.SetTile(new Vector3Int(j, i, 0), bottomleft); continue; }
                if (i == 3 & j == -8) { map.SetTile(new Vector3Int(j, i, 0), bottomright); continue; }
                if (i == 8) { map.SetTile(new Vector3Int(j, i, 0), top); continue; }
                if (i == 3) { map.SetTile(new Vector3Int(j, i, 0), bottom); continue; }
                if (j == -16) { map.SetTile(new Vector3Int(j, i, 0), left); continue; }
                if (j == -8) { map.SetTile(new Vector3Int(j, i, 0), right); continue; }

                if (i == 7 & j == -15) { map.SetTile(new Vector3Int(j, i, 0), one); continue; }
                if (i == 7 & j == -9) { map.SetTile(new Vector3Int(j, i, 0), three); continue; }
                if (i == 4 & j == -15) { map.SetTile(new Vector3Int(j, i, 0), seven); continue; }
                if (i == 4 & j == -9) { map.SetTile(new Vector3Int(j, i, 0), nine); continue; }
                if (i == 7) { map.SetTile(new Vector3Int(j, i, 0), two); continue; }
                if (i == 4) { map.SetTile(new Vector3Int(j, i, 0), eight); continue; }
                if (j == -15) { map.SetTile(new Vector3Int(j, i, 0), four); continue; }
                if (j == -9) { map.SetTile(new Vector3Int(j, i, 0), six); continue; }
                map.SetTile(new Vector3Int(j, i, 0), five);
            }
        }
        #endregion
        #region Score box
        for (int i = 0; i > -8; i--)
        {
            for (int j = -15; j < -7; j++)
            {
                if (i == 0 & j == -15) { map.SetTile(new Vector3Int(j, i, 0), topleft); continue; }
                if (i == 0 & j == -8) { map.SetTile(new Vector3Int(j, i, 0), topright); continue; }
                if (i == -7 & j == -15) { map.SetTile(new Vector3Int(j, i, 0), bottomleft); continue; }
                if (i == -7 & j == -8) { map.SetTile(new Vector3Int(j, i, 0), bottomright); continue; }
                if (i == 0) { map.SetTile(new Vector3Int(j, i, 0), top); continue; }
                if (i == -7) { map.SetTile(new Vector3Int(j, i, 0), bottom); continue; }
                if (j == -15) { map.SetTile(new Vector3Int(j, i, 0), left); continue; }
                if (j == -8) { map.SetTile(new Vector3Int(j, i, 0), right); continue; }

                if (i == -1 & j == -14) { map.SetTile(new Vector3Int(j, i, 0), one); continue; }
                if (i == -1 & j == -9) { map.SetTile(new Vector3Int(j, i, 0), three); continue; }
                if (i == -6 & j == -14) { map.SetTile(new Vector3Int(j, i, 0), seven); continue; }
                if (i == -6 & j == -9) { map.SetTile(new Vector3Int(j, i, 0), nine); continue; }
                if (i == -1) { map.SetTile(new Vector3Int(j, i, 0), two); continue; }
                if (i == -6) { map.SetTile(new Vector3Int(j, i, 0), eight); continue; }
                if (j == -14) { map.SetTile(new Vector3Int(j, i, 0), four); continue; }
                if (j == -9) { map.SetTile(new Vector3Int(j, i, 0), six); continue; }
                map.SetTile(new Vector3Int(j, i, 0), five);
            }
        }
        #endregion

        #region Next box
        for (int i = 8; i > -4; i--)
        {
            for (int j = 7; j < 14; j++)
            {
                if (i == 8 & j == 7) { map.SetTile(new Vector3Int(j, i, 0), topleft); continue; }
                if (i == 8 & j == 13) { map.SetTile(new Vector3Int(j, i, 0), topright); continue; }
                if (i == -3 & j == 7) { map.SetTile(new Vector3Int(j, i, 0), bottomleft); continue; }
                if (i == -3 & j == 13) { map.SetTile(new Vector3Int(j, i, 0), bottomright); continue; }
                if (i == 8) { map.SetTile(new Vector3Int(j, i, 0), top); continue; }
                if (i == -3) { map.SetTile(new Vector3Int(j, i, 0), bottom); continue; }
                if (j == 7) { map.SetTile(new Vector3Int(j, i, 0), left); continue; }
                if (j == 13) { map.SetTile(new Vector3Int(j, i, 0), right); continue; }

                if (i == 7 & j == 8) { map.SetTile(new Vector3Int(j, i, 0), one); continue; }
                if (i == 7 & j == 12) { map.SetTile(new Vector3Int(j, i, 0), three); continue; }
                if (i == -2 & j == 8) { map.SetTile(new Vector3Int(j, i, 0), seven); continue; }
                if (i == -2 & j == 12) { map.SetTile(new Vector3Int(j, i, 0), nine); continue; }
                if (i == 7) { map.SetTile(new Vector3Int(j, i, 0), two); continue; }
                if (i == -2) { map.SetTile(new Vector3Int(j, i, 0), eight); continue; }
                if (j == 8) { map.SetTile(new Vector3Int(j, i, 0), four); continue; }
                if (j == 12) { map.SetTile(new Vector3Int(j, i, 0), six); continue; }
                map.SetTile(new Vector3Int(j, i, 0), five);
            }
        }
        #endregion
    }
}
