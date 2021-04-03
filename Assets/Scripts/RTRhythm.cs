using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTRhythm : MonoBehaviour
{
    public float[] spectrum;
    public float[] rawNotes;
    public bool[] notes;
    public float threshold;
    public GameObject[] gameObjects;
    public GameObject averageObject;
    public float average;
    public float[] sums;
    public int[] counts;
    public float[] averages;
    public AudioSource source;
    public AnimationCurve curve;
    public float amp;
    void Awake()
    {
        spectrum = new float[64];
        rawNotes = new float[8];
        notes = new bool[8];
        sums = new float[8];
        counts = new int[8];
        averages = new float[8];
    }

    // Update is called once per frame
    void Update()
    {
        float sum = 0;
        threshold = average * 1.1f;

        //AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        source.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        for (int i = 0; i < 64; i += 8)
        {
            float f = 0;
            for (int j = 0; j < 8; j++) {
                f += spectrum[i + j];
            }

            //rawNotes[i / 8] = curve.Evaluate(f / 8) * amp;
            
            if (i/8 < 1)
            {
                rawNotes[i / 8] = Mathf.Log10(f / 8) * -1 * amp;
            }
            else
            {
                rawNotes[i / 8] = Mathf.Log(f / 8, (i + 2) * 2) * -1 * amp;
            }
            
            gameObjects[i / 8].transform.position = new Vector3
            {
                x = i/8,
                y = rawNotes[i / 8]
            };
            if (rawNotes[i/8] >= (averages[i / 8]))
            {
                notes[i / 8] = true;
            }
            else
            {
                notes[i / 8] = false;
            }
            sum += rawNotes[i / 8];
            sums[i/8] += rawNotes[i / 8];
            counts[i / 8]++;
            averages[i / 8] = sums[i / 8] / counts[i / 8];

        }
        average = sum / 8;
        averageObject.transform.position = new Vector3
        {
            x = 3.5f,
            y = average
        };
    }
}
