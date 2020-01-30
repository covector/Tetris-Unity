using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid_Fill : MonoBehaviour
{
    #region tile
    public Tile Oblock_0;
    public Tile Tblock_0; 
    public Tile Iblock_0;
    public Tile Lblock_0;
    public Tile Jblock_0;
    public Tile Sblock_0;
    public Tile Zblock_0;
    Tile Tblock_90;
    Tile Iblock_90;
    Tile Lblock_90;
    Tile Jblock_90;
    Tile Sblock_90;
    Tile Zblock_90;
    Tile Tblock_180;
    Tile Iblock_180;
    Tile Lblock_180;
    Tile Jblock_180;
    Tile Sblock_180;
    Tile Zblock_180;
    Tile Tblock_270;
    Tile Iblock_270;
    Tile Lblock_270;
    Tile Jblock_270;
    Tile Sblock_270;
    Tile Zblock_270;
    Tile Noblock;
    #endregion
    public int[,] mat;
    public CameraShake cam;
    Skin skinType;
    AudioSource[] soundFX;

    void Start()
    {
        soundFX = FindObjectOfType<AudioManage>().audioSources;
        mat = new int[20,10];
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                mat[y, x] = 0;
            }
        }
        skinType = FindObjectOfType<Master_Control>().skinArray[PlayerPrefs.GetInt("SkinIndex", 0)];
        if (!skinType.differentOrientation & skinType.differentColor)
        {
            Oblock_0 = skinType.oSingle;
            Tblock_0 = skinType.tSingle0;
            Tblock_90 = skinType.tSingle0;
            Tblock_180 = skinType.tSingle0;
            Tblock_270 = skinType.tSingle0;
            Iblock_0 = skinType.iSingle0;
            Iblock_90 = skinType.iSingle0;
            Iblock_180 = skinType.iSingle0;
            Iblock_270 = skinType.iSingle0;
            Lblock_0 = skinType.lSingle0;
            Lblock_90 = skinType.lSingle0;
            Lblock_180 = skinType.lSingle0;
            Lblock_270 = skinType.lSingle0;
            Jblock_0 = skinType.jSingle0;
            Jblock_90 = skinType.jSingle0;
            Jblock_180 = skinType.jSingle0;
            Jblock_270 = skinType.jSingle0;
            Sblock_0 = skinType.sSingle0;
            Sblock_90 = skinType.sSingle0;
            Sblock_180 = skinType.sSingle0;
            Sblock_270 = skinType.sSingle0;
            Zblock_0 = skinType.zSingle0;
            Zblock_90 = skinType.zSingle0;
            Zblock_180 = skinType.zSingle0;
            Zblock_270 = skinType.zSingle0;
        }
        if (!skinType.differentColor & skinType.differentOrientation)
        {
            Tblock_0 = skinType.tSingle0;
            Tblock_90 = skinType.tSingle90;
            Tblock_180 = skinType.tSingle180;
            Tblock_270 = skinType.tSingle270;
            Oblock_0 = Tblock_0;
            Iblock_0 = Tblock_0;
            Lblock_0 = Tblock_0;
            Jblock_0 = Tblock_0;
            Sblock_0 = Tblock_0;
            Zblock_0 = Tblock_0;
            Iblock_90 = Tblock_90;
            Lblock_90 = Tblock_90;
            Jblock_90 = Tblock_90;
            Sblock_90 = Tblock_90;
            Zblock_90 = Tblock_90;
            Iblock_180 = Tblock_180;
            Lblock_180 = Tblock_180;
            Jblock_180 = Tblock_180;
            Sblock_180 = Tblock_180;
            Zblock_180 = Tblock_180;
            Iblock_270 = Tblock_270;
            Lblock_270 = Tblock_270;
            Jblock_270 = Tblock_270;
            Sblock_270 = Tblock_270;
            Zblock_270 = Tblock_270;
        }
        if (!skinType.differentOrientation & !skinType.differentColor)
        {
            Oblock_0 = skinType.tSingle0;
            Tblock_0 = skinType.tSingle0;
            Tblock_90 = skinType.tSingle0;
            Tblock_180 = skinType.tSingle0;
            Tblock_270 = skinType.tSingle0;
            Iblock_0 = skinType.tSingle0;
            Iblock_90 = skinType.tSingle0;
            Iblock_180 = skinType.tSingle0;
            Iblock_270 = skinType.tSingle0;
            Lblock_0 = skinType.tSingle0;
            Lblock_90 = skinType.tSingle0;
            Lblock_180 = skinType.tSingle0;
            Lblock_270 = skinType.tSingle0;
            Jblock_0 = skinType.tSingle0;
            Jblock_90 = skinType.tSingle0;
            Jblock_180 = skinType.tSingle0;
            Jblock_270 = skinType.tSingle0;
            Sblock_0 = skinType.tSingle0;
            Sblock_90 = skinType.tSingle0;
            Sblock_180 = skinType.tSingle0;
            Sblock_270 = skinType.tSingle0;
            Zblock_0 = skinType.tSingle0;
            Zblock_90 = skinType.tSingle0;
            Zblock_180 = skinType.tSingle0;
            Zblock_270 = skinType.tSingle0;
        }
        if (skinType.differentColor & skinType.differentColor)
        {
            Oblock_0 = skinType.oSingle;
            Tblock_0 = skinType.tSingle0;
            Tblock_90 = skinType.tSingle90;
            Tblock_180 = skinType.tSingle180;
            Tblock_270 = skinType.tSingle270;
            Iblock_0 = skinType.iSingle0;
            Iblock_90 = skinType.iSingle90;
            Iblock_180 = skinType.iSingle180;
            Iblock_270 = skinType.iSingle270;
            Lblock_0 = skinType.lSingle0;
            Lblock_90 = skinType.lSingle90;
            Lblock_180 = skinType.lSingle180;
            Lblock_270 = skinType.lSingle270;
            Jblock_0 = skinType.jSingle0;
            Jblock_90 = skinType.jSingle90;
            Jblock_180 = skinType.jSingle180;
            Jblock_270 = skinType.jSingle270;
            Sblock_0 = skinType.sSingle0;
            Sblock_90 = skinType.sSingle90;
            Sblock_180 = skinType.sSingle180;
            Sblock_270 = skinType.sSingle270;
            Zblock_0 = skinType.zSingle0;
            Zblock_90 = skinType.zSingle90;
            Zblock_180 = skinType.zSingle180;
            Zblock_270 = skinType.zSingle270;
        }
        Noblock = skinType.NoBlock;


    }

    public void AddToMatrix(float y, float x, string type, int rotation)
    {
        int blockno = 0;
        if (type == "O") { blockno = 1; }
        if (type == "T") { blockno = 4; }
        if (type == "I") { blockno = 8; }
        if (type == "L") { blockno = 12; }
        if (type == "J") { blockno = 16; }
        if (type == "S") { blockno = 20; }
        if (type == "Z") { blockno = 24; }
        blockno += rotation / 90;
        Tilemap map = GetComponent<Tilemap>();
        map.SetTile(map.WorldToCell(new Vector3(x, y, 0)), Singleblock(blockno));
        mat[Coord2Grid(y, "Y"), Coord2Grid(x, "X")] = blockno;

    }

    int Coord2Grid(float coord, string XorY)
    {
        if (XorY == "X") { return (int)(2.5f * coord + 6f) - 1; }
        if (XorY == "Y") { return (int)(2.5f * coord + 12f) - 1; }
        else { return 0; }
    }

    float Grid2Coord(int grid, string XorY)
    {
        if (XorY == "X") { return 0.4f * (grid - 4.5f); }
        if (XorY == "Y") { return 0.4f * (grid - 10.5f); }
        else { return 0; }
    }


    Tile Singleblock(int typeno)
    {
        if (typeno == 1) { return Oblock_0; }
        if (typeno == 4) { return Tblock_0; }
        if (typeno == 5) { return Tblock_90; }
        if (typeno == 6) { return Tblock_180; }
        if (typeno == 7) { return Tblock_270; }
        if (typeno == 8) { return Iblock_0; }
        if (typeno == 9) { return Iblock_90; }
        if (typeno == 10) { return Iblock_180; }
        if (typeno == 11) { return Iblock_270; }
        if (typeno == 12) { return Lblock_0; }
        if (typeno == 13) { return Lblock_90; }
        if (typeno == 14) { return Lblock_180; }
        if (typeno == 15) { return Lblock_270; }
        if (typeno == 16) { return Jblock_0; }
        if (typeno == 17) { return Jblock_90; }
        if (typeno == 18) { return Jblock_180; }
        if (typeno == 19){ return Jblock_270; }
        if (typeno == 20) { return Sblock_0; }
        if (typeno == 21) { return Sblock_90; }
        if (typeno == 22) { return Sblock_180; }
        if (typeno == 23) { return Sblock_270; }
        if (typeno == 24) { return Zblock_0; }
        if (typeno == 25) { return Zblock_90; }
        if (typeno == 26) { return Zblock_180; }
        if (typeno == 27) { return Zblock_270; }
        else { return Noblock; }
    }

    public bool IsOccupied(float y, float x)
    {
        try
        {
            return mat[Coord2Grid(y, "Y"), Coord2Grid(x, "X")] != 0;
        }
        catch
        {
            return true;
        }
    }

    /* int[] GetRow(int[,] x, int row)
    {
        int[] y = new int[x.GetLength(1)];
        for (int i = 0; i < x.GetLength(1); i++)
        {
            y[i] = x[row, i];
        }
        return y;
    }

    int[] GetColumn(int[,] x, int column)
    {
        int[] y = new int[x.GetLength(0)];
        for (int i = 0; i < x.GetLength(0); i++)
        {
            y[i] = x[i, column];
        }
        return y;
    } */

    public float Highest(float y, float x) // returns the highest availabe coord for insta drop
    {
        for (int i = Coord2Grid(y, "Y") - 1; i >= 0; i--)
        {
            if (mat[i, Coord2Grid(x, "X")] != 0)
            {
                return Grid2Coord(i + 1, "Y");
            }
        }
        return Grid2Coord(0, "Y");
    }

    public void ClearLines()
    {
        int linesaved = 0;
        int linecleared = 0;
        for (int y = 0; y < 20; y++)
        {
            if (Clearable(y))
            {
                linecleared += 1;
                EmptyLine(y);
            }
            else
            {
                if (linecleared != 0)
                {
                    CopyLine(y - linecleared, y);
                    FindObjectOfType<Line_Cleared>().Addline(linecleared - linesaved);
                    FindObjectOfType<Score>().AddScore((linecleared - linesaved).ToString());
                    linesaved = linecleared;
                }
            }
        }
        for (int y = 19; y > 19 - linecleared; y--) { EmptyLine(y); }
        if (linecleared != 0) { if (soundFX.Length < 2) { soundFX = FindObjectOfType<AudioManage>().audioSources; Debug.Log("for some reason it reset?"); } soundFX[0].Play(); soundFX[1].Play(); StartCoroutine(cam.Shake(0.5f, 0.25f)); }
    }

    void CopyLine(int to, int from)
    {
        Tilemap map = GetComponent<Tilemap>();
        for (int x = 0; x < 10; x++)
        {
            mat[to, x] = mat[from, x]; //change matrix
            map.SetTile(map.WorldToCell(new Vector3(Grid2Coord(x, "X"), Grid2Coord(to, "Y"), 0)), Singleblock(mat[from,x])); // change tile
        }
    }

    void EmptyLine(int row)
    {
        Tilemap map = GetComponent<Tilemap>();
        for (int x = 0; x < 10; x++)
        {
            mat[row, x] = 0; // change matrix
            map.SetTile(map.WorldToCell(new Vector3(Grid2Coord(x, "X"), Grid2Coord(row, "Y"), 0)), Noblock); // change tile
        }
    }

    bool Clearable(int row)
    {
        for (int x = 0; x < 10; x++)
        {
            if (mat[row, x] == 0)
            {
                return false;
            }
        }
        return true;
    }

    public void PrintMatrix(int[,] matrix)
    {
        for(int y = 0; y < matrix.GetLength(0); y++)
        {
            string row = "";
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                row += matrix[y, x].ToString();
            }
            row += " " + y.ToString();
            Debug.Log(row);
        }
    }
}
