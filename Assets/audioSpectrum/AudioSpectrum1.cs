using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum1 : MonoBehaviour
{
    [SerializeField] private AudioSourceGetSpectrumDataExample musicPlayer;
    [SerializeField] private GameObject prefab;
    [SerializeField] public Vector3[] locations;
    [SerializeField] float xmin;
    [SerializeField] float xmax;
    [SerializeField] float ymin;
    [SerializeField] float ymax;
    [SerializeField] float zmin;
    [SerializeField] float zmax;
    [SerializeField] Vector3 initialPosition;
    [SerializeField] Vector3 offset;
    GameObject[] visualizers;
    [SerializeField] int mode;
    [SerializeField] int skip = 1;
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
                
            }
        }*/
        for (int i = 0; i < 63; i += skip)
        {
            //locations[i] = initialPosition + offset * i;
            locations[i] = new Vector3(Random.Range(xmin, xmax), Random.Range(ymin, ymax), Random.Range(zmin, zmax));
            visualizers[i] = Instantiate(prefab, locations[i], Quaternion.identity, transform);
        }
        musicPlayer = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSourceGetSpectrumDataExample>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 63; i++)
        {
            visualizers[i].transform.position = new Vector3(0, (musicPlayer.spectrum[i] * 100), 0) + locations[i];
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
}
