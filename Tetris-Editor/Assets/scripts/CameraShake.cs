using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform obj;
    float time_passed;
    public GameObject glow;
    public IEnumerator Shake(float time, float magnitude)
    {
        Instantiate(glow, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1));
        float variation = Random.Range(0f, 100f);
        time_passed = 0;
        Vector3 origin = new Vector3(0, 0, -10);
        for (int i = 0; i < 256; i++)
        {
            obj.position = origin + magnitude * (1 - time_passed / time) * new Vector3(Mathf.PerlinNoise(variation + 20f * time_passed, 0) - 0.5f, Mathf.PerlinNoise(0, variation + 20f * time_passed) - 0.5f, 0);
            time_passed += Time.deltaTime;
            if (time_passed > time) { break; }
            yield return null;
        }
    }
}