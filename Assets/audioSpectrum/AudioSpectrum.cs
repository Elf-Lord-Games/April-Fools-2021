using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    [SerializeField] private AudioSourceGetSpectrumDataExample musicPlayer;
    [SerializeField] private GameObject prefab;
    [SerializeField] public Vector3[] locations;
    [SerializeField] float min;
    [SerializeField] float max;
    [SerializeField] Vector3 initialPosition;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 scale = new Vector3(5, 2, 5);
    [SerializeField] float amplification = 1;
    GameObject[] visualizers;
    [SerializeField] int mode;
    [SerializeField] int skip = 1;
    [SerializeField] float highFrequencyAmplification;
    // Start is called before the first frame update
    void Start()
    {

        locations = new Vector3[64];
        visualizers = new GameObject[64];
        /*if(mode == 0)
        {
            for (int i = 0; i < 63; i += skip)
            {
                //line
                locations[i] = initialPosition + offset * i;
            }
        } else if (mode == 1)
        {
            for (int i = 0; i < 63; i += skip)
            {
                //circle
                Vector3 pos = RandomCircle(initialPosition, offset.x);
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, initialPosition - pos);
                Instantiate(prefab, pos, rot);
            }
        } else
        {
            for (int i = 0; i < 63; i += skip)
            {
                //random
                locations[i] = new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
            }
        }*/
        for (int i = 0; i < 63; i += skip)
        {
            locations[i] = initialPosition + offset * i;
            visualizers[i] = Instantiate(prefab, locations[i], Quaternion.identity, transform);
        }
        musicPlayer = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSourceGetSpectrumDataExample>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 63; i++)
        {
            visualizers[i].transform.localScale = new Vector3(scale.x, (((musicPlayer.spectrum[i] * 100) * ((i * highFrequencyAmplification)+ 1)) + scale.y) * amplification, scale.z);
        }

    }
    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    public void SetAmplification(float amp)
    {
        amplification = Mathf.RoundToInt(amp);
    }
}
