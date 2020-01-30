using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockAppearance : MonoBehaviour
{
    public string Type;
    Skin skinType;
    Tilemap map;

    void Start()
    {
        skinType = FindObjectOfType<Master_Control>().skinArray[PlayerPrefs.GetInt("SkinIndex", 0)];
        map = GetComponent<Tilemap>();
        switch (Type)
        {
            case "O":
                Tile oblock = FindObjectOfType<Grid_Fill>().Oblock_0;
                map.SetTile(new Vector3Int(0, 0, 0), oblock);
                map.SetTile(new Vector3Int(-1, 0, 0), oblock);
                map.SetTile(new Vector3Int(-1, -1, 0), oblock);
                map.SetTile(new Vector3Int(0, -1, 0), oblock);
                break;
            case "T":
                Tile tblock = FindObjectOfType<Grid_Fill>().Tblock_0;
                map.SetTile(new Vector3Int(0, 0, 0), tblock);
                map.SetTile(new Vector3Int(-1, 0, 0), tblock);
                map.SetTile(new Vector3Int(1, 0, 0), tblock);
                map.SetTile(new Vector3Int(0, 1, 0), tblock);
                break;
            case "I":
                Tile iblock = FindObjectOfType<Grid_Fill>().Iblock_0;
                map.SetTile(new Vector3Int(-2, 0, 0), iblock);
                map.SetTile(new Vector3Int(-1, 0, 0), iblock);
                map.SetTile(new Vector3Int(0, 0, 0), iblock);
                map.SetTile(new Vector3Int(1, 0, 0), iblock);
                break;
            case "L":
                Tile lblock = FindObjectOfType<Grid_Fill>().Lblock_0;
                map.SetTile(new Vector3Int(1, 1, 0), lblock);
                map.SetTile(new Vector3Int(-1, 0, 0), lblock);
                map.SetTile(new Vector3Int(0, 0, 0), lblock);
                map.SetTile(new Vector3Int(1, 0, 0), lblock);
                break;
            case "J":
                Tile jblock = FindObjectOfType<Grid_Fill>().Jblock_0;
                map.SetTile(new Vector3Int(-1, 1, 0), jblock);
                map.SetTile(new Vector3Int(-1, 0, 0), jblock);
                map.SetTile(new Vector3Int(0, 0, 0), jblock);
                map.SetTile(new Vector3Int(1, 0, 0), jblock);
                break;
            case "S":
                Tile sblock = FindObjectOfType<Grid_Fill>().Sblock_0;
                map.SetTile(new Vector3Int(0, 1, 0), sblock);
                map.SetTile(new Vector3Int(-1, 0, 0), sblock);
                map.SetTile(new Vector3Int(0, 0, 0), sblock);
                map.SetTile(new Vector3Int(1, 1, 0), sblock);
                break;
            case "Z":
                Tile zblock = FindObjectOfType<Grid_Fill>().Zblock_0;
                map.SetTile(new Vector3Int(0, 1, 0), zblock);
                map.SetTile(new Vector3Int(-1, 1, 0), zblock);
                map.SetTile(new Vector3Int(0, 0, 0), zblock);
                map.SetTile(new Vector3Int(1, 0, 0), zblock);
                break;
        }
    }


}
