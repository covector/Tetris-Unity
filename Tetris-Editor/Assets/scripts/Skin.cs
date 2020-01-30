using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Skins", menuName = "ScriptableObjects/Skin", order = 1)]
public class Skin : ScriptableObject
{
    public string name;
    public bool differentOrientation;
    public bool differentColor;

    [Header("O-Block")]
    public Tile oSingle;
    public Tile oBlock;

    [Header("T-Block")]
    public Tile tSingle0;
    public Tile tSingle90;
    public Tile tSingle180;
    public Tile tSingle270;
    public Tile tBlock;

    [Header("I-Block")]
    public Tile iSingle0;
    public Tile iSingle90;
    public Tile iSingle180;
    public Tile iSingle270;
    public Tile iBlock;

    [Header("L-Block")]
    public Tile lSingle0;
    public Tile lSingle90;
    public Tile lSingle180;
    public Tile lSingle270;
    public Tile lBlock;

    [Header("J-Block")]
    public Tile jSingle0;
    public Tile jSingle90;
    public Tile jSingle180;
    public Tile jSingle270;
    public Tile jBlock;

    [Header("S-Block")]
    public Tile sSingle0;
    public Tile sSingle90;
    public Tile sSingle180;
    public Tile sSingle270;
    public Tile sBlock;

    [Header("Z-Block")]
    public Tile zSingle0;
    public Tile zSingle90;
    public Tile zSingle180;
    public Tile zSingle270;
    public Tile zBlock;

    [Header("No-Block")]
    public Tile NoBlock;

    [Header("Frame")]
    public Tile TopLeft;
    public Tile Top;
    public Tile TopRight;
    public Tile Left;
    public Tile Right;
    public Tile BottomLeft;
    public Tile Bottom;
    public Tile BottomRight;

    [Header("Background")]
    public Tile one;
    public Tile two;
    public Tile three;
    public Tile four;
    public Tile five;
    public Tile six;
    public Tile seven;
    public Tile eight;
    public Tile nine;

    [Header("UI")]
    public Sprite pause;
    public Sprite Background;
    public Sprite Next;
    public Sprite Hold;

    [Header("Particles")]
    public GameObject LandParticles;
}
