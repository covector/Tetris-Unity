using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Master_Control : MonoBehaviour
{
    #region variables and constant
    /*
    public float inertia; // seconds of pressing a button until it start fast moving horizontally
    public float move_per_sec; // speed of fast moving horizontally
    public float speed; // falling speed
    public float fast; // falling speed when pressing down arrow
    public float time_til_stay; // time it takes for the block to become still
    public float freeze_timer; // max time for the block to become still
    */
    public float time_wait; // seconds wait before generating the next block
    public float maxspeed;
    public float curspeed = 1;
    float accel;
    bool taken; // have a block just been held
    public float smaller_chance; // chances for it to reshuffle if the block is already inside of the next tab
    public bool paused = false;
    bool haveblock = false;
    public bool escapable = false;
    public bool escapception = false;
    bool ghost;
    GameObject landParticle;
    AudioSource[] soundFX;
    public Skin[] skinArray;
    public Image pause;
    public Image background;
    public SpriteRenderer hold;
    public SpriteRenderer next;

    public Score finalscore;
    public GameObject fadeback2op;
    public AudioSource bgm;
    public GameObject fade2game;
    public GameObject pausebutton;
    public GameObject fade2op;
    public GameObject quitfad;
    public GameReport report;
    public GameObject gameover;
    public GameObject countdown;
    public GameObject parent;
    public GameObject bg;
    public GameObject widget;
    public GameObject opiece;
    public GameObject tpiece;
    public GameObject ipiece;
    public GameObject lpiece;
    public GameObject jpiece;
    public GameObject spiece;
    public GameObject zpiece;
    public GameObject oghost;
    public GameObject tghost;
    public GameObject ighost;
    public GameObject lghost;
    public GameObject jghost;
    public GameObject sghost;
    public GameObject zghost;

    #endregion
    void Start()
    {
        UpdateAccel();
        StartCoroutine(musicfadein());
        countdown.SetActive(true);
        soundFX = FindObjectOfType<AudioManage>().audioSources;
        FindObjectOfType<AudioManage>().ChangeVolume(PlayerPrefs.GetInt("SoundVolume", 1) / 100f);
        Skin currSkin = skinArray[PlayerPrefs.GetInt("SkinIndex", 0)];
        pause.sprite = currSkin.pause;
        background.sprite = currSkin.Background;
        hold.sprite = currSkin.Hold;
        next.sprite = currSkin.Next;
        landParticle = currSkin.LandParticles;
    }

    private void Update()
    {
        if (curspeed < maxspeed & (! paused))
        {
            curspeed += accel * Time.deltaTime;
        }
        if (escapable & Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (paused & Input.GetKey(KeyCode.R) & Input.GetKey(KeyCode.LeftShift) & Input.GetKey(KeyCode.LeftAlt) & Input.GetKey(KeyCode.LeftControl)) { SceneManager.LoadScene(1); }
    }

    IEnumerator musicfadein()
    {
        float vol = PlayerPrefs.GetInt("Volume", 75);
        for (int x = 0; x <= 60; x++)
        {
            bgm.volume = (vol * x) / 24000f;
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator musicfadeout()
    {
        float vol = PlayerPrefs.GetInt("Volume", 75);
        for (int x = 30; x >= 0; x--)
        {
            bgm.volume = (vol * x) / 12000f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Starting()
    {
        if (PlayerPrefs.GetInt("Ghost", 1) == 1) { ghost = true; }
        else { ghost = false; }
        escapable = true;
        paused = false;
        countdown.SetActive(false);
        if (!haveblock) { LoadRandom(); }
    }

    #region drawing
    public void AddToBoard(float y, float x, int rotation, string type)
    {
        haveblock = false;
        float[,] coord = Type2Coord(type, rotation);
        for (int i = 0; i < coord.GetLength(0); i++)
        {
            bg.GetComponent<Grid_Fill>().AddToMatrix(coord[i, 0] + y, coord[i, 1] + x, type, rotation);
        }
        // bg.GetComponent<Grid_Fill>().PrintMatrix(bg.GetComponent<Grid_Fill>().mat);
        bg.GetComponent<Grid_Fill>().ClearLines();
        StartCoroutine(LoadNext());
    }

    float[,] Type2Coord(string type, int rotation)
    {
        if (type == "O") { return new float[4, 2] { {-0.2f, -0.2f}, { 0.2f, -0.2f }, { -0.2f, 0.2f }, { 0.2f, 0.2f } }; }
        if (type == "T") {
            if (rotation == 0) { return new float[4, 2] { { 0, 0 }, { 0, 0.4f }, { 0, -0.4f }, { 0.4f, 0 } }; }
            if (rotation == 90) { return new float[4, 2] { { 0, 0 }, { 0, -0.4f }, { 0.4f, 0 }, { -0.4f, 0 } }; }
            if (rotation == 180) { return new float[4, 2] { { 0, 0 }, { 0, -0.4f }, { 0, 0.4f }, { -0.4f, 0 } }; }
            else { return new float[4, 2] { { 0, 0 }, { 0, 0.4f }, { 0.4f, 0 }, { -0.4f, 0 } }; }
        }
        if (type == "I")
        {
            if (rotation == 0) { return new float[4, 2] { { 0.2f, -0.6f }, { 0.2f, -0.2f }, { 0.2f, 0.2f }, { 0.2f, 0.6f } }; }
            if (rotation == 90) { return new float[4, 2] { { 0.6f, -0.2f }, { 0.2f, -0.2f }, { -0.2f, -0.2f }, { -0.6f, -0.2f } }; }
            if (rotation == 180) { return new float[4, 2] { { -0.2f, -0.6f }, { -0.2f, -0.2f }, { -0.2f, 0.2f }, { -0.2f, 0.6f } }; }
            else { return new float[4, 2] { { 0.6f, 0.2f }, { 0.2f, 0.2f }, { -0.2f, 0.2f }, { -0.6f, 0.2f } }; }
        }
        if (type == "L")
        {
            if (rotation == 0) { return new float[4, 2] { { 0, 0 }, { 0, 0.4f }, { 0, -0.4f }, { 0.4f, 0.4f } }; }
            if (rotation == 90) { return new float[4, 2] { { 0, 0 }, { 0.4f, -0.4f }, { 0.4f, 0 }, { -0.4f, 0 } }; }
            if (rotation == 180) { return new float[4, 2] { { 0, 0 }, { 0, -0.4f }, { 0, 0.4f }, { -0.4f, -0.4f } }; }
            else { return new float[4, 2] { { 0, 0 }, { -0.4f, 0.4f }, { 0.4f, 0 }, { -0.4f, 0 } }; }
        }
        if (type == "J")
        {
            if (rotation == 0) { return new float[4, 2] { { 0, 0 }, { 0, 0.4f }, { 0, -0.4f }, { 0.4f, -0.4f } }; }
            if (rotation == 90) { return new float[4, 2] { { 0, 0 }, { -0.4f, -0.4f }, { 0.4f, 0 }, { -0.4f, 0 } }; }
            if (rotation == 180) { return new float[4, 2] { { 0, 0 }, { 0, -0.4f }, { 0, 0.4f }, { -0.4f, 0.4f } }; }
            else { return new float[4, 2] { { 0, 0 }, { 0.4f, 0.4f }, { 0.4f, 0 }, { -0.4f, 0 } }; }
        }
        if (type == "S")
        {
            if (rotation == 0) { return new float[4, 2] { { 0, 0 }, { 0.4f, 0.4f }, { 0, -0.4f }, { 0.4f, 0 } }; }
            if (rotation == 90) { return new float[4, 2] { { 0, 0 }, { 0, -0.4f }, { 0.4f, -0.4f }, { -0.4f, 0 } }; }
            if (rotation == 180) { return new float[4, 2] { { 0, 0 }, { -0.4f, -0.4f }, { 0, 0.4f }, { -0.4f, 0 } }; }
            else { return new float[4, 2] { { 0, 0 }, { 0, 0.4f }, { 0.4f, 0 }, { -0.4f, 0.4f } }; }
        }
        if (type == "Z")
        {
            if (rotation == 0) { return new float[4, 2] { { 0, 0 }, { 0, 0.4f }, { 0.4f, -0.4f }, { 0.4f, 0 } }; }
            if (rotation == 90) { return new float[4, 2] { { 0, 0 }, { 0, -0.4f }, { 0.4f, 0 }, { -0.4f, -0.4f } }; }
            if (rotation == 180) { return new float[4, 2] { { 0, 0 }, { 0, -0.4f }, { -0.4f, 0.4f }, { -0.4f, 0 } }; }
            else { return new float[4, 2] { { 0, 0 }, { 0, 0.4f }, { 0.4f, 0.4f }, { -0.4f, 0 } }; }
        }
        else { return new float[0,0]; }
    }

    public bool Intersect(string type, float y, float x, int rotation)
    { 
        float[,] blockloc = Type2Coord(type, rotation);
        for (int j = 0; j < blockloc.GetLength(0); j++)
        {
            if (bg.GetComponent<Grid_Fill>().IsOccupied(y + blockloc[j, 0], x + blockloc[j, 1])) { return true; }
        }
        return false;
    }

    public bool CheckCoord(string type, float y, float x, float rotation, string direc)
    {
        if (type == "O")
        {
            if (direc == "Left") { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.2f, x - 0.6f) | bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.2f, x - 0.6f); }
            if (direc == "Right") { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.2f, x + 0.6f) | bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.2f, x + 0.6f); }
            if (direc == "Down") { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.6f, x + 0.2f) | bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.6f, x - 0.2f); }
            if (direc == "LDiag") { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.6f, x - 0.6f); }
            if (direc == "RDiag") { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.6f, x + 0.6f); }
        }
        if (type == "T")
        {
            if (direc == "Left")
            {
                bool top = bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x - 0.4f);
                bool midleft = bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.8f);
                bool midright = bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.4f);
                bool down = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.4f);
                if (rotation == 0) { return top | midleft; }
                if (rotation == 90) { return top | midleft | down; }
                if (rotation == 180) { return down | midleft; }
                else { return top | midright | down; }
            }
            if (direc == "Right")
            {
                bool top = bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.4f);
                bool midleft = bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f);
                bool midright = bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.8f);
                bool down = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f);
                if (rotation == 0) { return top | midright; }
                if (rotation == 90) { return top | midleft | down; }
                if (rotation == 180) { return down | midright; }
                else { return top | midright | down; }
            }
            if (direc == "Down")
            {
                bool topleft = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.4f);
                bool topmid = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x);
                bool topright = bg.GetComponent<Grid_Fill>().IsOccupied(y -  0.4f, x + 0.4f);
                bool down = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x);
                if (rotation == 0) { return topleft | topmid | topright; }
                if (rotation == 90) { return topleft | down; }
                if (rotation == 180) { return topleft | down | topright; }
                else { return down | topright; }
            }
            if (direc == "LDiag") 
            { 
                bool topleft = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.8f);
                bool downright = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.4f);
                if (rotation == 0) { return topleft; }
                if (rotation == 270) { return downright; }
                else { return topleft | downright; }
            }
            if (direc == "RDiag")
            {
                bool topright = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.8f);
                bool downleft = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f);
                if (rotation == 0) { return topright; }
                if (rotation == 270) { return downleft; }
                else { return topright | downleft; }
            }
        }
        if (type == "I")
        {
            int factor = -1;
            if (rotation == 0 | rotation == 270) { factor = 1; }
            if (direc == "Left")
            {
                if (rotation == 0 | rotation == 180) { return bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f, x - 1f); }
                else
                {
                    bool one = bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.6f, x + factor * 0.2f - 0.4f);
                    bool two = bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.2f, x + factor * 0.2f - 0.4f);
                    bool three = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.2f, x + factor * 0.2f - 0.4f);
                    bool four = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.6f, x + factor * 0.2f - 0.4f);
                    return one | two | three | four;
                }
            }
            if (direc == "Right")
            {
                if (rotation == 0 | rotation == 180) { return bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f, x + 1f); }
                else
                {
                    bool one = bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.6f, x + factor * 0.2f + 0.4f);
                    bool two = bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.2f, x + factor * 0.2f + 0.4f);
                    bool three = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.2f, x + factor * 0.2f + 0.4f);
                    bool four = bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.6f, x + factor * 0.2f + 0.4f);
                    return one | two | three | four;
                }
            }
            if (direc == "Down")
            {
                if (rotation == 0 | rotation == 180)
                {
                    bool one = bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f - 0.4f, x + 0.6f);
                    bool two = bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f - 0.4f, x + 0.2f);
                    bool three = bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f - 0.4f, x - 0.2f);
                    bool four = bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f - 0.4f, x - 0.6f);
                    return one | two | three | four;
                }
                else { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 1f, x + factor * 0.2f); }
            }
            if (direc == "LDiag")
            {
                if (rotation == 0 | rotation == 180) { return bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f - 0.4f, x - 1f); }
                else { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 1f, x + factor * 0.2f - 0.4f); }
            }
            if (direc == "RDiag")
            {
                if (rotation == 0 | rotation == 180) { return bg.GetComponent<Grid_Fill>().IsOccupied(y + factor * 0.2f - 0.4f, x + 1f); }
                else { return bg.GetComponent<Grid_Fill>().IsOccupied(y - 1f, x + factor * -0.2f + 0.4f); }
            }
        }
        if (type == "L")
        {
            if (direc == "Left")
            {
                if (rotation == 90 | rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.4f) | bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.4f)) { return true; } }
                else { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.8f)) { return true; } }
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x - 0.8f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.8f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x - 0.4f)) { return true; } }
            }
            if (direc == "Right")
            {
                if (rotation == 90 | rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f) | bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.4f)) { return true; } }
                else { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.8f)) { return true; } }
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.8f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.8f)) { return true; } }
            }
            if (direc == "Down")
            {
                if (rotation == 0 | rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x) | bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f)) { return true; } }
                else { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x)) { return true; } }
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.4f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.4f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.4f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f)) { return true; } }
            }
            if (direc == "LDiag")
            {
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.8f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.4f) | bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.4f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.8f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.4f)) { return true; } }
            }
            if (direc == "RDiag")
            {
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.8f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.8f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.8f)) { return true; } }
            }
        }
        if (type == "J")
        {
            if (direc == "Left")
            {
                if (rotation == 90 | rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.4f) | bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x - 0.4f)) { return true; } }
                else { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x - 0.8f)) { return true; } }
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x - 0.8f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.8f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.4f)) { return true; } }
            }
            if (direc == "Right")
            {
                if (rotation == 90 | rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f) | bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f)) { return true; } }
                else { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.8f)) { return true; } }
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.4f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.8f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.8f)) { return true; } }
            }
            if (direc == "Down")
            {
                if (rotation == 0 | rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x) | bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.4f)) { return true; } }
                else { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x)) { return true; } }
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.4f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f)) { return true; } }
            }
            if (direc == "LDiag")
            {
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.8f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.8f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x - 0.8f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.4f)) { return true; } }
            }
            if (direc == "RDiag")
            {
                if (rotation == 0) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.8f)) { return true; } }
                if (rotation == 90) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f)) { return true; } }
                if (rotation == 180) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.8f)) { return true; } }
                if (rotation == 270) { if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f) | bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.8f)) { return true; } }
            }
        }
        if (type == "S")
        {
            int add = 0;
            if (rotation == 180 | rotation == 270) { add = 1; }
            if (direc == "Left")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x - 0.8f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f * (1 - add), x - 0.4f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.4f * (add - 2))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f * (add - 2))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * (add - 1))) { return true; }
                }
            }
            if (direc == "Right")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x + 0.4f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (add - 1), x + 0.8f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.4f * add)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f * (1 + add))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * (1 + add))) { return true; }
                }
            }
            if (direc == "Down")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x - 0.4f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x + 0.4f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * (add - 1))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f * add)) { return true; }
                }
            }
            if (direc == "LDiag")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x - 0.8f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * (add - 2))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f * (add - 1))) { return true; }
                }
            }
            if (direc == "RDiag")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x + 0.4f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x + 0.8f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f * (add + 1))) { return true; }
                }
            }
        }
        if (type == "Z")
        {
            int add = 0;
            if (rotation == 180 | rotation == 270) { add = 1; }
            if (direc == "Left")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x - 0.4f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f * (1 - add), x - 0.8f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * (add - 2))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f * (add - 2))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.4f * (add - 1))) { return true; }
                }
            }
            if (direc == "Right")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x + 0.8f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (add - 1), x + 0.4f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * add)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y, x + 0.4f * (1 + add))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y + 0.4f, x + 0.4f * (1 + add))) { return true; }
                }
            }
            if (direc == "Down")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x + 0.4f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x - 0.4f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f * (add - 1))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * add)) { return true; }
                }
            }
            if (direc == "RDiag")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x + 0.8f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f, x + 0.4f * (add + 1))) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x + 0.4f * add)) { return true; }
                }
            }
            if (direc == "LDiag")
            {
                if (rotation == 0 | rotation == 180)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * (1 + add), x - 0.4f)) { return true; }
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.4f * add, x - 0.8f)) { return true; }
                }
                if (rotation == 90 | rotation == 270)
                {
                    if (bg.GetComponent<Grid_Fill>().IsOccupied(y - 0.8f, x - 0.4f * (2 - add))) { return true; }
                }
            }
        }

        return false;
    }
    #endregion
    public float InstaDrop(float y, float x, string type, int rotation = 0, bool ghost = false)
    {
        float out_val = 0;
        float offset = 0.2f;
        float xOff = 0;
        if (type == "O")
        {
            offset = 0.4f;
            float Left = 0.2f + bg.GetComponent<Grid_Fill>().Highest(y - 0.2f, x - 0.2f);
            float Right = 0.2f + bg.GetComponent<Grid_Fill>().Highest(y - 0.2f, x + 0.2f);
            if (Left >= Right) { out_val = Left; xOff = -0.2f; }
            else { out_val = Right; xOff = 0.2f; }
            if (Left == Right) { xOff = 0; }
        }
        if (type == "T")
        {
            float Mid = -4.2f;
            float Right = -4.2f;
            float Left = -4.2f;
            offset = 0.2f;
            if (rotation == 0) {
                Left = bg.GetComponent<Grid_Fill>().Highest(y, x - 0.4f);
                Right = bg.GetComponent<Grid_Fill>().Highest(y, x + 0.4f);
                Mid = bg.GetComponent<Grid_Fill>().Highest(y, x);
            }
            else
            {
                Mid = 0.4f + bg.GetComponent<Grid_Fill>().Highest(y - 0.4f, x);
                if (rotation != 270) { Left = bg.GetComponent<Grid_Fill>().Highest(y, x - 0.4f); }
                if (rotation != 90) { Right = bg.GetComponent<Grid_Fill>().Highest(y, x + 0.4f); }
            }
            if (Mid >= Right & Mid >= Left) { out_val = Mid; if (rotation != 0) { offset = 0.6f; } }
            else
            {
                if (Left >= Right) { out_val = Left; xOff = -0.4f; }
                else { out_val = Right; xOff = 0.4f; }
            }
        }
        if (type == "I")
        {
            int factor = 1;
            if (rotation == 0 | rotation == 90) { factor = -1; }
            if (rotation == 0 | rotation == 180)
            {
                offset = 0.2f + factor * 0.2f;
                float one = bg.GetComponent<Grid_Fill>().Highest(y - factor * 0.2f, x - 0.6f) + factor * 0.2f;
                float two = bg.GetComponent<Grid_Fill>().Highest(y - factor * 0.2f, x - 0.2f) + factor * 0.2f;
                float three = bg.GetComponent<Grid_Fill>().Highest(y - factor * 0.2f, x + 0.2f) + factor * 0.2f;
                float four = bg.GetComponent<Grid_Fill>().Highest(y - factor * 0.2f, x + 0.6f) + factor * 0.2f;
                if (two >= one & two >= three & two >= four) { out_val = two; xOff = -0.2f; if (two == three) { xOff = 0; } }
                else
                {
                    if (three >= one & three >= four) { out_val = three; xOff = 0.2f; }
                    else
                    {
                        if (one >= four) { out_val = one; xOff = -0.6f; }
                        else { out_val = four; xOff = 0.6f; }
                    }
                }
            }
            else { offset = 0.8f; out_val = 0.6f + bg.GetComponent<Grid_Fill>().Highest(y - 0.6f, x + factor * 0.2f); xOff = 0.2f * factor; }
        }
        if (type == "L")
        {
            if (rotation == 0 | rotation == 180)
            {
                float Mid = bg.GetComponent<Grid_Fill>().Highest(y, x);
                float Right = bg.GetComponent<Grid_Fill>().Highest(y, x + 0.4f);
                float Left = bg.GetComponent<Grid_Fill>().Highest(y, x - 0.4f);
                if (rotation == 180)
                {
                    Left += 0.4f;
                }
                if (Mid >= Right & Mid >= Left) { out_val = Mid; }
                else
                {
                    if (Left >= Right) { out_val = Left; xOff = -0.4f; if (rotation == 180) { offset = 0.6f; } }
                    else { out_val = Right; xOff = 0.4f; }
                }
            }
            else
            {
                offset = 0.6f;
                float Another;
                float Mid = bg.GetComponent<Grid_Fill>().Highest(y - 0.4f, x) + 0.4f;
                if (rotation == 90) { Another = bg.GetComponent<Grid_Fill>().Highest(y + 0.4f, x - 0.4f) - 0.4f; }
                else { Another = bg.GetComponent<Grid_Fill>().Highest(y - 0.4f, x + 0.4f) + 0.4f; }
                if (Mid >= Another) { out_val = Mid; }
                else { out_val = Another; if (rotation == 90) { offset = -0.2f; xOff = -0.2f; } else { xOff = 0.2f; } }
            }
        }
        if (type == "J")
        {
            if (rotation == 0 | rotation == 180)
            {
                float Mid = bg.GetComponent<Grid_Fill>().Highest(y, x);
                float Right = bg.GetComponent<Grid_Fill>().Highest(y, x + 0.4f);
                float Left = bg.GetComponent<Grid_Fill>().Highest(y, x - 0.4f);
                if (rotation == 180)
                {
                    Right += 0.4f;
                }
                if (Mid >= Right & Mid >= Left) { out_val = Mid; }
                else
                {
                    if (Left >= Right) { out_val = Left; xOff = -0.4f; }
                    else { out_val = Right; xOff = 0.4f; if (rotation == 180) { offset = 0.6f; } }
                }
            }
            else
            {
                offset = 0.6f;
                float Another;
                float Mid = bg.GetComponent<Grid_Fill>().Highest(y - 0.4f, x) + 0.4f;
                if (rotation == 90) { Another = bg.GetComponent<Grid_Fill>().Highest(y - 0.4f, x - 0.4f) + 0.4f; }
                else { Another = bg.GetComponent<Grid_Fill>().Highest(y + 0.4f, x + 0.4f) - 0.4f; }
                if (Mid >= Another) { out_val = Mid; }
                else { out_val = Another; if (rotation == 270) { offset = -0.2f; xOff = 0.2f; } else { xOff = -0.2f; } }
            }
        }
        if (type == "S")
        {
            if (rotation != 0) { offset = 0.6f; }
            int add = 0;
            if (rotation == 180 | rotation == 270) { add = 1; }
            if (rotation == 0 | rotation == 180)
            {
                float Mid = bg.GetComponent<Grid_Fill>().Highest(y - add * 0.4f, x) + 0.4f * add;
                float Right = bg.GetComponent<Grid_Fill>().Highest(y + 0.4f - add * 0.4f, x + 0.4f) - 0.4f * (1 - add);
                float Left = bg.GetComponent<Grid_Fill>().Highest(y - add * 0.4f, x - 0.4f) + 0.4f * add;
                if (Mid >= Right & Mid >= Left) { out_val = Mid; }
                else
                {
                    if (Left >= Right) { out_val = Left; xOff = -0.4f; }
                    else { out_val = Right; xOff = 0.4f; offset = (add - 0.5f) * 0.4f; }
                }
            }
            else
            {
                float Left = bg.GetComponent<Grid_Fill>().Highest(y, x - 0.4f * (1 - add));
                float Right = bg.GetComponent<Grid_Fill>().Highest(y - 0.4f, x + 0.4f * add) + 0.4f;
                if (Left >= Right) { out_val = Left; xOff = -0.4f + add * 0.4f; if (rotation == 90 | rotation == 270) { offset = 0.2f; } }
                else { out_val = Right; xOff = add * 0.4f; }
            }
        }
        if (type == "Z")
        {
            if (rotation != 0) { offset = 0.6f; }
            int add = 0;
            if (rotation == 180 | rotation == 270) { add = 1; }
            if (rotation == 0 | rotation == 180)
            {
                float Mid = bg.GetComponent<Grid_Fill>().Highest(y - add * 0.4f, x) + 0.4f * add;
                float Left = bg.GetComponent<Grid_Fill>().Highest(y + 0.4f - add * 0.4f, x - 0.4f) - 0.4f * (1 - add);
                float Right = bg.GetComponent<Grid_Fill>().Highest(y - add * 0.4f, x + 0.4f) + 0.4f * add;
                if (Mid >= Right & Mid >= Left) { out_val = Mid; }
                else
                {
                    if (Left >= Right) { out_val = Left; xOff = -0.4f; offset = (add - 0.5f) * 0.4f; }
                    else { out_val = Right; xOff = 0.4f; }
                }
            }
            else
            {
                float Left = bg.GetComponent<Grid_Fill>().Highest(y - 0.4f, x - 0.4f * (1 - add)) + 0.4f;
                float Right = bg.GetComponent<Grid_Fill>().Highest(y, x + 0.4f * add);
                if (Left >= Right) { out_val = Left; xOff = -0.4f + add * 0.4f; }
                else { out_val = Right; xOff = add * 0.4f; if (rotation == 90 | rotation == 270) { offset = 0.2f; } }
            }
        }
        if (!ghost)
        {
            soundFX[2].Play();
            soundFX[3].Play();
            FindObjectOfType<Score>().AddScore("insta");
            Instantiate(landParticle, new Vector3(x + xOff, out_val - offset, 0), Quaternion.Euler(270, 0, 0));
        }
        return out_val;
    }

    public float[] RotationChoice(string type, float y, float x, int fromrot, int rotby, string hold, bool moving_already)
    {
        int rotation = (fromrot + rotby + 360) % 360;
        int hv = 0;
        int fac = -1;
        if (type == "I") 
        {
            if (fromrot == 90 | fromrot == 270) { hv = 1; }
            if (fromrot == 0 | fromrot == 270) { fac = 1; }
        }
        if (hold == "None" | moving_already)
        {
            int bias;
            if (!(Intersect(type, y, x, rotation))) { return new float[] { 0, 0 }; }
            if (!(Intersect(type, y, x - 0.4f, rotation))) { return new float[] { 0, -0.4f }; }
            if (!(Intersect(type, y, x + 0.4f, rotation))) { return new float[] { 0, 0.4f }; }
            if (!(Intersect(type, y + 0.4f, x, rotation))) { return new float[] { 0.4f, 0 }; }
            if (!(Intersect(type, y - 0.4f, x, rotation))) { return new float[] { -0.4f, 0 }; }

            if (rotation == 90) { bias = 1; }
            if (rotation == 270) { bias = -1; }
            else { if (Random.Range(0, 1) < 0.5f) { bias = 1; } else { bias = -1; } }

            if (!(Intersect(type, y - 0.4f, x - 0.4f * bias, rotation))) { return new float[] { -0.4f, -0.4f }; }
            if (!(Intersect(type, y + 0.4f, x - 0.4f * bias, rotation))) { return new float[] { 0.4f, -0.4f }; }
            if (!(Intersect(type, y - 0.4f, x + 0.4f * bias, rotation))) { return new float[] { -0.4f, 0.4f }; }
            if (!(Intersect(type, y + 0.4f, x + 0.4f * bias, rotation))) { return new float[] { 0.4f, 0.4f }; }
            if (type == "I")
            {
                if (!(Intersect(type, y + (1 - hv) * fac * 0.8f - hv * 0.4f, x + hv * fac * 0.8f, rotation))) { return new float[] { (1 - hv) * fac * 0.8f - hv * 0.4f, hv * fac * 0.8f }; }
                if (!(Intersect(type, y + (1 - hv) * fac * 0.8f, x + hv * fac * 0.8f + (1 - hv) * fac * 0.4f, rotation))) { return new float[] { (1 - hv) * fac * 0.8f, hv * fac * 0.8f + (1 - hv) * fac * 0.4f }; }
                if (!(Intersect(type, y + (1 - hv) * fac * 0.8f + hv * 0.4f, x + hv * fac * 0.8f - (1 - hv) * fac * 0.4f, rotation))) { return new float[] { (1 - hv) * fac * 0.8f + hv * 0.4f, hv * fac * 0.8f - (1 - hv) * fac * 0.4f }; }
            }
        }
        if (hold == "Left")
        {
            if (fromrot == 90 & type == "I")
            {
                if (!(Intersect(type, y - 0.4f, x - 0.8f, rotation))) { return new float[] { -0.4f, -0.8f }; }
                if (!(Intersect(type, y, x - 0.8f, rotation))) { return new float[] { 0, -0.8f }; }
                if (!(Intersect(type, y + 0.4f, x - 0.8f, rotation))) { return new float[] { 0.4f, -0.8f }; }
            }
            if (!(Intersect(type, y, x - 0.4f, rotation))) { return new float[] { 0, -0.4f }; }
            if (!(Intersect(type, y - 0.4f, x - 0.4f, rotation))) { return new float[] { -0.4f, -0.4f }; }
            if (!(Intersect(type, y + 0.4f, x - 0.4f, rotation))) { return new float[] { 0.4f, -0.4f }; }
            if ((fromrot == 0 | fromrot == 90) & type == "I") { if (!(Intersect(type, y + fac * 0.8f, x - 0.4f, rotation))) { return new float[] { fac * 0.8f, 0.4f }; } }
            if (!(Intersect(type, y, x, rotation))) { return new float[] { 0, 0 }; }
            if (!(Intersect(type, y - 0.4f, x, rotation))) { return new float[] { -0.4f, 0 }; }
            if (!(Intersect(type, y + 0.4f, x, rotation))) { return new float[] { 0.4f, 0 }; }
            if ((fromrot == 0 | fromrot == 90) & type == "I") { if (!(Intersect(type, y + fac * 0.8f, x, rotation))) { return new float[] { fac * 0.8f, 0 }; } }
            if (!(Intersect(type, y, x + 0.4f, rotation))) { return new float[] { 0, 0.4f }; }
            if (!(Intersect(type, y - 0.4f, x + 0.4f, rotation))) { return new float[] { -0.4f, 0.4f }; }
            if (!(Intersect(type, y + 0.4f, x + 0.4f, rotation))) { return new float[] { 0.4f, 0.4f }; }
            if ((fromrot == 0 | fromrot == 90) & type == "I") { if (!(Intersect(type, y + fac * 0.8f, x + 0.4f, rotation))) { return new float[] { fac * 0.8f, -0.4f }; } }
            if (fromrot == 270 & type == "I")
            {
                if (!(Intersect(type, y - 0.4f, x + 0.8f, rotation))) { return new float[] { -0.4f, 0.8f }; }
                if (!(Intersect(type, y, x + 0.8f, rotation))) { return new float[] { 0, 0.8f }; }
                if (!(Intersect(type, y + 0.4f, x + 0.8f, rotation))) { return new float[] { 0.4f, 0.8f }; }
            }
        }
        if (hold == "Right")
        {
            if (fromrot == 270 & type == "I")
            {
                if (!(Intersect(type, y - 0.4f, x + 0.8f, rotation))) { return new float[] { -0.4f, 0.8f }; }
                if (!(Intersect(type, y, x + 0.8f, rotation))) { return new float[] { 0, 0.8f }; }
                if (!(Intersect(type, y + 0.4f, x + 0.8f, rotation))) { return new float[] { 0.4f, 0.8f }; }
            }
            if (!(Intersect(type, y, x + 0.4f, rotation))) { return new float[] { 0, 0.4f }; }
            if (!(Intersect(type, y - 0.4f, x + 0.4f, rotation))) { return new float[] { -0.4f, 0.4f }; }
            if (!(Intersect(type, y + 0.4f, x + 0.4f, rotation))) { return new float[] { 0.4f, 0.4f }; }
            if ((fromrot == 0 | fromrot == 90) & type == "I") { if (!(Intersect(type, y + fac * 0.8f, x + 0.4f, rotation))) { return new float[] { fac * 0.8f, 0.4f }; } }
            if (!(Intersect(type, y, x, rotation))) { return new float[] { 0, 0 }; }
            if (!(Intersect(type, y - 0.4f, x, rotation))) { return new float[] { -0.4f, 0 }; }
            if (!(Intersect(type, y + 0.4f, x, rotation))) { return new float[] { 0.4f, 0 }; }
            if ((fromrot == 0 | fromrot == 90) & type == "I") { if (!(Intersect(type, y + fac * 0.8f, x, rotation))) { return new float[] { fac * 0.8f, 0 }; } }
            if (!(Intersect(type, y, x - 0.4f, rotation))) { return new float[] { 0, -0.4f }; }
            if (!(Intersect(type, y - 0.4f, x - 0.4f, rotation))) { return new float[] { -0.4f, -0.4f }; }
            if (!(Intersect(type, y + 0.4f, x - 0.4f, rotation))) { return new float[] { 0.4f, -0.4f }; }
            if ((fromrot == 0 | fromrot == 90) & type == "I") { if (!(Intersect(type, y + fac * 0.8f, x - 0.4f, rotation))) { return new float[] { fac * 0.8f, -0.4f }; } }
            if (fromrot == 90 & type == "I")
            {
                if (!(Intersect(type, y - 0.4f, x - 0.8f, rotation))) { return new float[] { -0.4f, -0.8f }; }
                if (!(Intersect(type, y, x - 0.8f, rotation))) { return new float[] { 0, -0.8f }; }
                if (!(Intersect(type, y + 0.4f, x - 0.8f, rotation))) { return new float[] { 0.4f, -0.8f }; }
            }
        }
        if (hv == 1 & type == "I") // last resort
        {
            if (!(Intersect(type, y + 0.8f, x, rotation))) { return new float[] { 0.8f, 0 }; }
            if (!(Intersect(type, y + 0.8f, x + fac * 0.4f, rotation))) { return new float[] { 0.8f, fac * 0.4f }; }
            if (!(Intersect(type, y + 0.8f, x - fac * 0.4f, rotation))) { return new float[] { 0.8f, fac * -0.4f }; }
            if ((fromrot == 90 & rotby == 90) | (fromrot == 270 & rotby == -90))
            {
                if (!(Intersect(type, y + 1.2f, x, rotation))) { return new float[] { 1.2f, 0 }; }
                if (!(Intersect(type, y + 1.2f, x + fac * 0.4f, rotation))) { return new float[] { 1.2f, fac * 0.4f }; }
                if (!(Intersect(type, y + 1.2f, x - fac * 0.4f, rotation))) { return new float[] { 1.2f, fac * -0.4f }; }
            }
        }
        if ((type == "L" | type == "J" | type == "S" | type == "Z") & (fromrot == 90 | fromrot == 270))
        {
            int bias = 1;
            if (((type == "L" | type == "Z") & fromrot == 270) | ((type == "J" | type == "S") & fromrot == 90)) { bias = -1; }
            {
                if (!(Intersect(type, y + 0.8f, x, rotation))) { return new float[] { 0.8f, 0 }; }
                if (!(Intersect(type, y + 0.8f, x + bias * 0.4f, rotation))) { return new float[] { 0.8f, bias * 0.4f }; }
                if (!(Intersect(type, y + 0.8f, x - bias * 0.4f, rotation))) { return new float[] { 0.8f, bias * -0.4f }; }
            }
        }
        return null;
    }

    public bool Holdblock(string type)
    {
        if (!(taken)) {
            soundFX[6].Play();
            if (ghost) { FindObjectOfType<Ghost>().DestroyGhost(); }
            string returnpiece = widget.GetComponent<Side_Widget>().Hold(type);
            if (returnpiece == "None") { LoadRandom(); }
            else { LoadPiece(returnpiece); }
            taken = true;
            return true;
        }
        return false;
    }
    #region loading
    IEnumerator LoadNext()
    {
        taken = false;
        yield return new WaitForSeconds(time_wait);
        if (!paused) {
        if (FindObjectOfType<Spin_Control_Opiece>() != null | FindObjectOfType<Spin_Control_Lpiece>() != null | FindObjectOfType<Spin_Control_Jpiece>() != null | FindObjectOfType<Spin_Control_Spiece>() != null | FindObjectOfType<Spin_Control_Zpiece>() != null | FindObjectOfType<Spin_Control_Tpiece>() != null | FindObjectOfType<Spin_Control_Ipiece>() != null)
            { Debug.Log("already have"); }
        else { LoadRandom(); }
        }
    }

    void LoadRandom()
    {
        LoadPiece(widget.GetComponent<Side_Widget>().Next());
    }


    void LoadPiece(string type)
    {
        haveblock = true;
        if (type == "O") {
            if (Intersect("O", 3.2f, 0, 0)) { Lost(type, 3.2f, 0); }
            else {
                Instantiate(opiece, new Vector3(0, 3.2f, 0), Quaternion.identity, parent.transform);
                if (ghost)
                {
                    float initghosty = InstaDrop(3.2f, 0, "O", 0, true);
                    GameObject ghost = Instantiate(oghost, new Vector3(0, initghosty, 0), Quaternion.identity, parent.transform);
                    ghost.GetComponent<Ghost>().TellCoord(initghosty, 0);
                }
            }
        }
        if (type == "T")
        {
            if (Intersect("T", 3f, 0.2f, 0)) { Lost(type, 3f, 0.2f); }
            else { 
                Instantiate(tpiece, new Vector3(0.2f, 3f, 0), Quaternion.identity, parent.transform);
                if (ghost)
                {
                    float initghosty = InstaDrop(3f, 0.2f, "T", 0, true);
                    GameObject ghost = Instantiate(tghost, new Vector3(0.2f, initghosty, 0), Quaternion.identity, parent.transform);
                    ghost.GetComponent<Ghost>().TellCoord(initghosty, 0.2f);
                }
            }
        }
        if (type == "I")
        {
            if (Intersect("I", 3.2f, 0, 0)) { Lost(type, 3.2f, 0); }
            else { 
                Instantiate(ipiece, new Vector3(0, 3.2f, 0), Quaternion.identity, parent.transform);
                if (ghost)
                {
                    float initghosty = InstaDrop(3.2f, 0, "I", 0, true);
                    GameObject ghost = Instantiate(ighost, new Vector3(0, initghosty, 0), Quaternion.identity, parent.transform);
                    ghost.GetComponent<Ghost>().TellCoord(initghosty, 0);
                }
            }
        }
        if (type == "L")
        {
            if (Intersect("L", 3f, 0.2f, 0)) { Lost(type, 3f, 0.2f); }
            else {
                Instantiate(lpiece, new Vector3(0.2f, 3f, 0), Quaternion.identity, parent.transform);
                if (ghost)
                {
                    float initghosty = InstaDrop(3f, 0.2f, "L", 0, true);
                    GameObject ghost = Instantiate(lghost, new Vector3(0.2f, initghosty, 0), Quaternion.identity, parent.transform);
                    ghost.GetComponent<Ghost>().TellCoord(initghosty, 0.2f);
                }
            }
        }
        if (type == "J")
        {
            if (Intersect("J", 3f, 0.2f, 0)) { Lost(type, 3f, 0.2f); }
            else {
                Instantiate(jpiece, new Vector3(0.2f, 3f, 0), Quaternion.identity, parent.transform);
                if (ghost)
                {
                    float initghosty = InstaDrop(3f, 0.2f, "J", 0, true);
                    GameObject ghost = Instantiate(jghost, new Vector3(0.2f, initghosty, 0), Quaternion.identity, parent.transform);
                    ghost.GetComponent<Ghost>().TellCoord(initghosty, 0.2f);
                }
            }
        }
        if (type == "S")
        {
            if (Intersect("S", 3f, 0.2f, 0)) { Lost(type, 3f, 0.2f); }
            else {
                Instantiate(spiece, new Vector3(0.2f, 3f, 0), Quaternion.identity, parent.transform);
                if (ghost)
                {
                    float initghosty = InstaDrop(3f, 0.2f, "S", 0, true);
                    GameObject ghost = Instantiate(sghost, new Vector3(0.2f, initghosty, 0), Quaternion.identity, parent.transform);
                    ghost.GetComponent<Ghost>().TellCoord(initghosty, 0.2f);
                }
            }
        }
        if (type == "Z")
        {
            if (Intersect("Z", 3f, 0.2f, 0)) { Lost(type, 3f, 0.2f); }
            else {
                Instantiate(zpiece, new Vector3(0.2f, 3f, 0), Quaternion.identity, parent.transform);
                if (ghost)
                {
                    float initghosty = InstaDrop(3f, 0.2f, "Z", 0, true);
                    GameObject ghost = Instantiate(zghost, new Vector3(0.2f, initghosty, 0), Quaternion.identity, parent.transform);
                    ghost.GetComponent<Ghost>().TellCoord(initghosty, 0.2f);
                }
            }
        }
    }
    #endregion

    public void Lost(string last, float y, float x)
    {
        if (finalscore.totalscore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)finalscore.totalscore);
        }
        pausebutton.SetActive(false);
        bool place = true;
        float[,] coord = Type2Coord(last, 0);
        for (int i = 0; i < coord.GetLength(0); i++)
        {

            if (bg.GetComponent<Grid_Fill>().IsOccupied(coord[i, 0] + y + 0.4f, coord[i, 1] + x) & ((int)(2.5f * (coord[i, 0] + y + 0.4f) + 12f) - 1) < 20) { place = false; }
        }
        if (place)
        {
            for (int i = 0; i < coord.GetLength(0); i++)
            {
                if(((int)(2.5f * (coord[i, 0] + y + 0.4f) + 12f) - 1) < 20){ bg.GetComponent<Grid_Fill>().AddToMatrix(coord[i, 0] + y + 0.4f, coord[i, 1] + x, last, 0); }
            }
        }
        StartCoroutine(musicfadeout());
        report.ChangeNumber();
        gameover.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        quitfad.SetActive(true);

    }

    public void PauseGame()
    {
        if (!paused)
        {
            paused = true;
            fade2op.SetActive(true);
        }
        else
        {
            if (escapception)
            {
                fadeback2op.SetActive(true);
                escapception = false;
            }
            else
            {
                int ghostAfter = PlayerPrefs.GetInt("Ghost", 1);
                if (ghost & ghostAfter == 0) { ghost = false; FindObjectOfType<Ghost>().DestroyGhost(); }
                if (!ghost & ghostAfter == 1)
                { 
                    ghost = true;
                    if (FindObjectOfType<Spin_Control_Opiece>() != null)
                    {
                        Transform block = FindObjectOfType<Spin_Control_Opiece>().transform;
                        float initghosty = InstaDrop(block.position.y, block.position.x, "O", (int)block.rotation.z, true);
                        GameObject ghost = Instantiate(oghost, new Vector3(block.position.x, initghosty, 0), Quaternion.Euler(0, 0, block.rotation.z), parent.transform);
                        ghost.GetComponent<Ghost>().TellCoord(initghosty, block.position.x, (int)block.rotation.z);
                    }
                    else
                    {
                        if (FindObjectOfType<Spin_Control_Tpiece>() != null)
                        {
                            Transform block = FindObjectOfType<Spin_Control_Tpiece>().transform;
                            float initghosty = InstaDrop(block.position.y, block.position.x, "T", (int)block.rotation.z, true);
                            GameObject ghost = Instantiate(tghost, new Vector3(block.position.x, initghosty, 0), Quaternion.Euler(0, 0, block.rotation.z), parent.transform);
                            ghost.GetComponent<Ghost>().TellCoord(initghosty, block.position.x, (int)block.rotation.z);
                        }
                        else
                        {
                            if (FindObjectOfType<Spin_Control_Ipiece>() != null)
                            {
                                Transform block = FindObjectOfType<Spin_Control_Ipiece>().transform;
                                float initghosty = InstaDrop(block.position.y, block.position.x, "I", (int)block.rotation.z, true);
                                GameObject ghost = Instantiate(ighost, new Vector3(block.position.x, initghosty, 0), Quaternion.Euler(0, 0, block.rotation.z), parent.transform);
                                ghost.GetComponent<Ghost>().TellCoord(initghosty, block.position.x, (int)block.rotation.z);
                            }
                            else
                            {
                                if (FindObjectOfType<Spin_Control_Lpiece>() != null)
                                {
                                    Transform block = FindObjectOfType<Spin_Control_Lpiece>().transform;
                                    float initghosty = InstaDrop(block.position.y, block.position.x, "L", (int)block.rotation.z, true);
                                    GameObject ghost = Instantiate(lghost, new Vector3(block.position.x, initghosty, 0), Quaternion.Euler(0, 0, block.rotation.z), parent.transform);
                                    ghost.GetComponent<Ghost>().TellCoord(initghosty, block.position.x, (int)block.rotation.z);
                                }
                                else
                                {
                                    if (FindObjectOfType<Spin_Control_Jpiece>() != null)
                                    {
                                        Transform block = FindObjectOfType<Spin_Control_Jpiece>().transform;
                                        float initghosty = InstaDrop(block.position.y, block.position.x, "J", (int)block.rotation.z, true);
                                        GameObject ghost = Instantiate(jghost, new Vector3(block.position.x, initghosty, 0), Quaternion.Euler(0, 0, block.rotation.z), parent.transform);
                                        ghost.GetComponent<Ghost>().TellCoord(initghosty, block.position.x, (int)block.rotation.z);
                                    }
                                    else
                                    {
                                        if (FindObjectOfType<Spin_Control_Spiece>() != null)
                                        {
                                            Transform block = FindObjectOfType<Spin_Control_Spiece>().transform;
                                            float initghosty = InstaDrop(block.position.y, block.position.x, "S", (int)block.rotation.z, true);
                                            GameObject ghost = Instantiate(sghost, new Vector3(block.position.x, initghosty, 0), Quaternion.Euler(0, 0, block.rotation.z), parent.transform);
                                            ghost.GetComponent<Ghost>().TellCoord(initghosty, block.position.x, (int)block.rotation.z);
                                        }
                                        else
                                        {
                                            if (FindObjectOfType<Spin_Control_Zpiece>() != null)
                                            {
                                                Transform block = FindObjectOfType<Spin_Control_Zpiece>().transform;
                                                float initghosty = InstaDrop(block.position.y, block.position.x, "Z", (int)block.rotation.z, true);
                                                GameObject ghost = Instantiate(zghost, new Vector3(block.position.x, initghosty, 0), Quaternion.Euler(0, 0, block.rotation.z), parent.transform);
                                                ghost.GetComponent<Ghost>().TellCoord(initghosty, block.position.x, (int)block.rotation.z);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                fade2game.SetActive(true);
                try { FindObjectOfType<Spin_Control_Opiece>().UpdateSettings(); } catch { }
                try { FindObjectOfType<Spin_Control_Tpiece>().UpdateSettings(); } catch { }
                try { FindObjectOfType<Spin_Control_Ipiece>().UpdateSettings(); } catch { }
                try { FindObjectOfType<Spin_Control_Lpiece>().UpdateSettings(); } catch { }
                try { FindObjectOfType<Spin_Control_Jpiece>().UpdateSettings(); } catch { }
                try { FindObjectOfType<Spin_Control_Spiece>().UpdateSettings(); } catch { }
                try { FindObjectOfType<Spin_Control_Zpiece>().UpdateSettings(); } catch { }
            }
        }
        escapable = false;
    }

    public void ExtraSet()
    {
        escapception = true;
        escapable = false;
    }

    public void UpdateAccel()
    {
        accel = PlayerPrefs.GetFloat("Accel", 0.005f);
    }

    public float ChangeGhostPiece(string type, float y, float x, int rotation)
    {
        return InstaDrop(y, x, type, rotation, true);
    }

}


