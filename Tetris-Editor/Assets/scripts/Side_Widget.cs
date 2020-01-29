using UnityEngine;
using UnityEngine.Tilemaps;

public class Side_Widget : MonoBehaviour
{
    #region variables and constants
    public Tile Otile;
    public Tile Itile;
    public Tile Ttile;
    public Tile Ltile;
    public Tile Jtile;
    public Tile Stile;
    public Tile Ztile;
    float chance;

    string Held = "None";
    int Next1;
    int Next2;
    int Next3;
    Tilemap map;

    void Start()
    {
        chance = FindObjectOfType<Master_Control>().smaller_chance;
        Next1 = Random.Range(1, 8);
        Next2 = LessChanceRandom(Next1);
        Next3 = LessChanceRandom(Next1, Next2);
        map = GetComponent<Tilemap>();
        map.SetTile(new Vector3Int(10, 5, 0), Typeno2Tile(Next1));
        map.SetTile(new Vector3Int(10, 2, 0), Typeno2Tile(Next2));
        map.SetTile(new Vector3Int(10, -1, 0), Typeno2Tile(Next3));
    }
    #endregion
    public string Hold(string type)
    {
        string Unhold = Held;
        Held = type;
        map.SetTile(new Vector3Int(-12, 5, 0), Type2Tile(type));
        return Unhold;
    }

    Tile Typeno2Tile(int type)
    {
        if (type == 1) { return Otile; }
        if (type == 2) { return Ttile; }
        if (type == 3) { return Itile; }
        if (type == 4) { return Ltile; }
        if (type == 5) { return Jtile; }
        if (type == 6) { return Stile; }
        if (type >= 7) { return Ztile; }
        else { return null; }
    }

    Tile Type2Tile(string type)
    {
        if (type == "O") { return Otile; }
        if (type == "T") { return Ttile; }
        if (type == "I") { return Itile; }
        if (type == "L") { return Ltile; }
        if (type == "J") { return Jtile; }
        if (type == "S") { return Stile; }
        if (type == "Z") { return Ztile; }
        else { return null; }
    }

    public string Next()
    {
        int output = Next1;
        Next1 = Next2;
        Next2 = Next3;
        Next3 = LessChanceRandom(Next1, Next2, output);
        map = GetComponent<Tilemap>();
        map.SetTile(new Vector3Int(10, 5, 0), Typeno2Tile(Next1));
        map.SetTile(new Vector3Int(10, 2, 0), Typeno2Tile(Next2));
        map.SetTile(new Vector3Int(10, -1, 0), Typeno2Tile(Next3));
        if (output == 1) { return "O"; }
        if (output == 2) { return "T"; }
        if (output == 3) { return "I"; }
        if (output == 4) { return "L"; }
        if (output == 5) { return "J"; }
        if (output == 6) { return "S"; }
        if (output >= 7) { return "Z"; }
        else { return "None"; }
    }

    int LessChanceRandom(int avoid1, int avoid2 = 0, int avoid3 = 0)
    {
        int output = 0;
        int failsafe = 0;
        while (failsafe < 100) {
            output = Random.Range(1, 8);
            if (output != avoid1 & output != avoid2 & output != avoid3 & Random.Range(0, 1) <= chance) { break; }
            failsafe += 1;
        }
        return output;
    }
}
