using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spin_Control_Tpiece : MonoBehaviour
{
    #region variables and constant
    public Transform state;
    float right_hold = 0f; // how long has the right arrow key been pressed in seconds
    float right_moved = 0f; // how many unit did the momentum move displace the block
    float left_hold = 0f; // how long has the left arrow key been pressed in seconds
    float left_moved = 0f; // how many unit did the momentum move displace the block
    float dropped_time = 0f; // how long have it been falling since last touched a ground
    float dropped_unit = 0f; // how many unit did it falled
    bool landed = false; // does the block freeze
    float landed_time = 0f; // timer for it to freeze
    float reset_land = 0f; // timer before reset landing timer
    string hold = "None"; // left or right
    bool moving_already = false; // the block is already moving horizontally in this frame
    float drop_speed = 1;
    float norm_speed = 1;
    float inertia;
    float move_per_sec;
    float speed;
    float time_til_stay;
    float freeze_timer;
    float fast;
    bool ghost;
    KeyCode lefthotkey;
    KeyCode righthotkey;
    KeyCode downhotkey;
    KeyCode clockhotkey;
    KeyCode anticlockhotkey;
    KeyCode instahotkey;
    KeyCode holdhotkey;
    public Tilemap spren;
    bool blinkable = true;
    AudioSource[] soundFX;

    void Start()
    {
        soundFX = FindObjectOfType<AudioManage>().audioSources;
        UpdateSettings();
        if (PlayerPrefs.GetInt("Ghost", 1) == 1) { ghost = true; }
        else { ghost = false; }
    }
    #endregion
    void Update()
    {
        if (!(landed | FindObjectOfType<Master_Control>().paused))
        {
            moving_already = false;
            norm_speed = FindObjectOfType<Master_Control>().curspeed;
            if ((Input.GetKey(lefthotkey) & !(Input.GetKey(righthotkey)))) { hold = "Left"; }
            if ((Input.GetKey(righthotkey) & !(Input.GetKey(lefthotkey)))) { hold = "Right"; }
            if (((Input.GetKey(lefthotkey)) & (Input.GetKey(righthotkey))) | (!(Input.GetKey(lefthotkey)) & !(Input.GetKey(righthotkey)))) { hold = "None"; }

            #region Hold
            if (Input.GetKeyDown(holdhotkey) & !(Input.GetKeyDown(instahotkey)))
            {
                bool holdbk = FindObjectOfType<Master_Control>().Holdblock("T");
                if (holdbk)
                {
                    Destroy(gameObject);
                }
            }
            #endregion
            #region Right Control
            // single move
            if (Input.GetKeyDown(righthotkey) & !(Input.GetKeyDown(lefthotkey)) & !(Bump_wall_R()))
            {
                soundFX[4].Play();
                moving_already = true;
                state.position += new Vector3(0.4f, 0, 0);
                if (ghost) { StateChange(); }
            }
            // momentum move
            if (Input.GetKey(righthotkey) & !(Input.GetKey(lefthotkey)) & !(Bump_wall_R()))
            {
                moving_already = true;
                right_hold += Time.deltaTime;
                if (right_hold >= inertia)
                {
                    if (div(right_hold - inertia, 1 / move_per_sec) >= right_moved)
                    {
                        float step = div(right_hold - inertia, 1 / move_per_sec) - right_moved;
                        if (step > 0.1f) { soundFX[4].Play(); }
                        state.position += new Vector3(step * 0.4f, 0, 0);
                        right_moved += step;
                        if (ghost) { StateChange(); }
                    }
                }
            }
            else // reset after release of the arrow key
            {
                if (hold == "Right" & right_hold + Time.deltaTime < ((right_moved + 1) / move_per_sec) + inertia - Time.deltaTime)
                {
                    right_hold += Time.deltaTime;
                }
                if (!(hold == "Right") & !(Bump_wall_R())) // preserve momentum
                {
                    right_hold = 0f;
                    right_moved = 0f;
                }
            }
            #endregion
            #region Left Control
            // single move
            if (Input.GetKeyDown(lefthotkey) & !(Input.GetKeyDown(righthotkey)) & !(Bump_wall_L()))
            {
                soundFX[4].Play();
                moving_already = true;
                state.position -= new Vector3(0.4f, 0, 0);
                if (ghost) { StateChange(); }
            }
            // momentum move
            if (Input.GetKey(lefthotkey) & !(Input.GetKey(righthotkey)) & !(Bump_wall_L()))
            {
                moving_already = true;
                left_hold += Time.deltaTime;
                if (left_hold >= inertia)
                {
                    if (div(left_hold - inertia, 1 / move_per_sec) >= left_moved)
                    {
                        float step = div(left_hold - inertia, 1 / move_per_sec) - left_moved;
                        if (step > 0.1f) { soundFX[4].Play(); }
                        state.position += new Vector3(step * -0.4f, 0, 0);
                        left_moved += step;
                        if (ghost) { StateChange(); }

                    }
                }
            }
            else // reset after release of the arrow key
            {
                if (hold == "Left" & left_hold + Time.deltaTime < ((left_moved + 1) / move_per_sec) + inertia - Time.deltaTime)
                {
                    left_hold += Time.deltaTime;
                }
                if (hold != "Left" & !(Bump_wall_L())) // preserve momentum
                {
                    left_hold = 0f;
                    left_moved = 0f;
                }
            }
            #endregion
            #region Spin Control
            /* bool restrictspin = (Comp_float(state.eulerAngles.z, 270f) | Comp_float(state.eulerAngles.z, 90f)) & Bump_wall_L() & Bump_wall_R() & (div(dropped_time + Time.deltaTime, 1 / speed) > dropped_unit | FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "twoblockwalls"));
            if (Input.GetKeyDown(clockhotkey) & !(Input.GetKeyDown(anticlockhotkey)) & !(restrictspin) & !(FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "Top")))
            {
                if ((Comp_float(state.position.x, -1.8f) | FindObjectOfType<Master_Control>().Intersect("T", state.position.y, state.position.x, 180)) & Comp_float(state.eulerAngles.z, 270f))
                {
                    state.position += new Vector3(0.4f, 0, 0);
                }
                if ((Comp_float(state.position.x, 1.8f) | FindObjectOfType<Master_Control>().Intersect("T", state.position.y, state.position.x, 0)) & Comp_float(state.eulerAngles.z, 90f))
                {
                    state.position -= new Vector3(0.4f, 0, 0);
                }

                state.eulerAngles = new Vector3(0, 0, (state.eulerAngles.z - 90) % 360);
            }
            if (Input.GetKeyDown(anticlockhotkey) & !(Input.GetKeyDown(clockhotkey))& !(restrictspin) & !(FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "Top")))
            {
                if ((Comp_float(state.position.x, -1.8f) | FindObjectOfType<Master_Control>().Intersect("T", state.position.y, state.position.x, 0)) & Comp_float(state.eulerAngles.z, 270f))
                {
                    state.position += new Vector3(0.4f, 0, 0);
                }

                if ((Comp_float(state.position.x, 1.8f) | FindObjectOfType<Master_Control>().Intersect("T", state.position.y, state.position.x, 180)) & Comp_float(state.eulerAngles.z, 90f))
                {
                    state.position -= new Vector3(0.4f, 0, 0);
                }

                state.eulerAngles = new Vector3(0, 0, (state.eulerAngles.z + 90) % 360);
            }
            */
            // Clockwise spin
            if (Input.GetKeyDown(clockhotkey) & !(Input.GetKeyDown(anticlockhotkey)) & FindObjectOfType<Master_Control>().RotationChoice("T", state.position.y, state.position.x, (int)state.eulerAngles.z, -90, hold, moving_already) != null)
            {
                soundFX[5].Play();
                float [] deltapos = FindObjectOfType<Master_Control>().RotationChoice("T", state.position.y, state.position.x, (int)state.eulerAngles.z, -90, hold, moving_already);
                state.eulerAngles = new Vector3(0, 0, (state.eulerAngles.z - 90) % 360);
                state.position += new Vector3(deltapos[1], deltapos[0], 0);
            }
            // Anticlockwise spin
            if (Input.GetKeyDown(anticlockhotkey) & !(Input.GetKeyDown(clockhotkey)) & FindObjectOfType<Master_Control>().RotationChoice("T", state.position.y, state.position.x, (int)state.eulerAngles.z, 90, hold, moving_already) != null)
            {
                soundFX[5].Play();
                float[] deltapos = FindObjectOfType<Master_Control>().RotationChoice("T", state.position.y, state.position.x, (int)state.eulerAngles.z, 90, hold, moving_already);
                state.eulerAngles = new Vector3(0, 0, (state.eulerAngles.z + 90) % 360);
                state.position += new Vector3(deltapos[1], deltapos[0], 0);
            }
            #endregion
            #region Down Control
            if (Input.GetKey(downhotkey))
            {
                drop_speed = fast;
            }
            else
            {
                drop_speed = norm_speed;
            }
            #endregion
            #region Falling
            // Insta drop
            if (Input.GetKeyDown(instahotkey))
            {
                float final = FindObjectOfType<Master_Control>().InstaDrop(state.position.y, state.position.x, "T", (int)(state.eulerAngles.z));
                state.position = new Vector3(state.position.x, final, 0);
                landed = true;
                Dead_block();
            }
            // landing
            if (!Bump_ground())
            {
                dropped_time += Time.deltaTime * drop_speed;
            }
            else
            {
                landed_time += Time.deltaTime;
                if ((!(Bump_wall_L()) & !(Bump_wall_R())) & (Input.GetKey(righthotkey) & !(Input.GetKey(lefthotkey))) | (Input.GetKey(lefthotkey) & !(Input.GetKey(righthotkey))) | (Input.GetKey(clockhotkey) & !(Input.GetKey(anticlockhotkey))) | (Input.GetKey(anticlockhotkey) & !(Input.GetKey(clockhotkey))))
                {
                    reset_land = landed_time;
                }
                if ((landed_time - reset_land >= (time_til_stay - FindObjectOfType<Master_Control>().curspeed / 60)) | (landed_time >= (freeze_timer - FindObjectOfType<Master_Control>().curspeed / 15)))
                {
                    landed = true;
                    Dead_block();
                }
                if (blinkable)
                {
                    blinkable = false;
                    StartCoroutine(Blink());
                }
            }
            // dropping
            if (div(dropped_time, 1 / speed) >= dropped_unit)
            {
                float step = div(dropped_time, 1 / speed) - dropped_unit;
                state.position -= new Vector3(0, step * 0.4f, 0);
                dropped_unit += step;
                if (ghost) { StateChange(); }
                if (Input.GetKey(downhotkey)) { FindObjectOfType<Score>().AddScore("fast", (int)step); }
            }
            #endregion
            #region Ground Spin Fix
            //if (( (Comp_float(state.eulerAngles.z, 270f) & Input.GetKeyDown(clockhotkey) & !(Input.GetKeyDown(anticlockhotkey))) | (Comp_float(state.eulerAngles.z, 90f) & Input.GetKeyDown(anticlockhotkey) & !(Input.GetKeyDown(clockhotkey)))) & (Comp_float(state.position.y, -4.2f) | (FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, 270, "Down") & Input.GetKeyDown(clockhotkey) & !(Input.GetKeyDown(anticlockhotkey))) | (FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, 90, "Down") & Input.GetKeyDown(anticlockhotkey) & !(Input.GetKeyDown(clockhotkey)))))
            //Debug.Log((FindObjectOfType<Master_Control>().Intersect("T", state.position.y, state.position.x, state.eulerAngles.z)));
            if (( (Input.GetKeyDown(clockhotkey) & !(Input.GetKeyDown(anticlockhotkey))) | (Input.GetKeyDown(anticlockhotkey) & !(Input.GetKeyDown(clockhotkey))) ) & (FindObjectOfType<Master_Control>().Intersect("T", state.position.y, state.position.x, (int)state.eulerAngles.z)))
            {
                state.position += new Vector3(0, 0.4f, 0);
                if (ghost) { StateChange(); }
            }
            #endregion
        }
    }

    // floor division function
    float div(float a, float b)
    {
        return (a - (a % b)) / b;
    }

    // check if it is allowed for the block to move
    bool Bump_wall_L()
    {
        // assumpted that 1 step wouldn't be more than 1 unit
        bool clockwise = Input.GetKeyDown(anticlockhotkey);
        bool anticlockwise = Input.GetKeyDown(clockhotkey);

        bool left_wall_max = Comp_float(state.position.x, -1.8f) | Comp_float(state.position.x, -1.4f) & (Comp_float(state.eulerAngles.z, 270f) & (clockwise | anticlockwise));
        bool left_wall_norm = !(Comp_float(state.eulerAngles.z, 270f)) & Comp_float(state.position.x, -1.4f) & !(Comp_float(state.eulerAngles.z, 0f) & (clockwise));
        bool left_block = FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "Left") | (FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "LDiag") & !(FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "Down")) & div(dropped_time + Time.deltaTime, 1 / speed) > dropped_unit);
        bool left_wall = left_wall_max | left_wall_norm | left_block;

        return left_wall;
    }

    bool Bump_wall_R()
    {
        // assumpted that 1 step wouldn't be more than 1 unit
        bool clockwise = Input.GetKeyDown(anticlockhotkey);
        bool anticlockwise = Input.GetKeyDown(clockhotkey);

        bool right_wall_max = Comp_float(state.position.x, 1.8f) | Comp_float(state.position.x, 1.4f) & (Comp_float(state.eulerAngles.z, 90f) & (clockwise | anticlockwise));
        bool right_wall_norm = !(Comp_float(state.eulerAngles.z, 90f)) & Comp_float(state.position.x, 1.4f) & !(Comp_float(state.eulerAngles.z, 180f) & (clockwise));
        bool right_block = FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "Right") | (FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "RDiag") & !(FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "Down")) & div(dropped_time + Time.deltaTime, 1 / speed) > dropped_unit);
        bool right_wall = right_wall_max | right_wall_norm | right_block;

        return right_wall;
    }

        // check if the block is touching the ground
    bool Bump_ground()
    {
        bool clockwise = Input.GetKeyDown(anticlockhotkey);
        bool anticlockwise = Input.GetKeyDown(clockhotkey);

        bool vert = (Comp_float(state.position.y, -3.8f) & ((Comp_float(state.eulerAngles.z, 270f) & !(anticlockwise)) | (Comp_float(state.eulerAngles.z, 90f) & !(clockwise))));
        bool hori1 = Comp_float(state.eulerAngles.z, 0f) & (Comp_float(state.position.y, -4.2f) | (Comp_float(state.position.y, -3.8f) & (clockwise | anticlockwise)));
        bool hori2 = Comp_float(state.position.y, -3.8f) & Comp_float(state.eulerAngles.z, 180f);
        bool blocks = FindObjectOfType<Master_Control>().CheckCoord("T", state.position.y, state.position.x, state.eulerAngles.z, "Down");

        return vert | hori1 | hori2 | blocks;
    }

    bool Comp_float(float a, float b)
    {
        return ((int)(10 * a + 0.5f) == (int)(10 * b + 0.5f));
    }

    void Dead_block()
    {
        FindObjectOfType<Master_Control>().AddToBoard(state.position.y, state.position.x, (int)(state.eulerAngles.z), "T");
        if (ghost) { FindObjectOfType<Ghost>().DestroyGhost(); }
        Destroy(gameObject);
    }
    public void UpdateSettings()
    {
        lefthotkey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "LeftArrow"));
        righthotkey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "RightArrow"));
        downhotkey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "DownArrow"));
        clockhotkey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Clockwise", "UpArrow"));
        anticlockhotkey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Anticlockwise", "Z"));
        instahotkey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Insta", "Space"));
        holdhotkey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Hold", "C"));
        inertia = PlayerPrefs.GetFloat("Inertia", 0.1f);
        move_per_sec = PlayerPrefs.GetFloat("HoriSpeed", 16f);
        speed = PlayerPrefs.GetFloat("VertiSpeed", 1.5f);
        fast = PlayerPrefs.GetFloat("VertiMulti", 8f);
        time_til_stay = PlayerPrefs.GetFloat("Freeze", 0.5f);
        freeze_timer = PlayerPrefs.GetFloat("ABSFreeze", 2f);
        if (PlayerPrefs.GetInt("Ghost", 1) == 1) { ghost = true; } else { ghost = false; }
    }

    void StateChange()
    {
        float ghoststate = FindObjectOfType<Master_Control>().ChangeGhostPiece("T", state.position.y, state.position.x, (int)state.eulerAngles.z);
        FindObjectOfType<Ghost>().UpdateState(ghoststate, state.position.x, (int)state.eulerAngles.z);
    }

    IEnumerator Blink()
    {
        float time_passed = 0;
        for (int i = 0; i < 300; i++)
        {
            if (time_passed > 1) { blinkable = true; break; }
            float v = 0.75f + 0.25f * Mathf.Cos(time_passed * 2 * Mathf.PI);
            spren.color = new Color(v, v, v);
            time_passed += Time.deltaTime;
            yield return null;
        }
        spren.color = new Color(1, 1, 1);
    }
}
