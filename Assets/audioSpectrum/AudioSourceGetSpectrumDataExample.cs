using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioSourceGetSpectrumDataExample : MonoBehaviour
{
    public float[] spectrum;
    float value;
    [SerializeField] float threshold;
    [SerializeField] float difference;
    [SerializeField] Material[] materials;
    [SerializeField] Light[] lights;
    [SerializeField] int r, b, g = 100;
    [Range(0.0f, 10.0f)]
    [SerializeField] float t;
    Color currentColor;
    Color randomColor;
    bool up = false;
    bool everyOther = true;
    private void Awake()
    {
        spectrum = new float[128];
        randomColor = Random.ColorHSV();
    }
    void Update()
    {


        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        //currentColor = new Color(255 * spectrum[3], 255 * spectrum[1], 255 * spectrum[2], 100);

        if (everyOther)
        {
            currentColor = new Color(r * spectrum[3], g * spectrum[1], b * spectrum[2], 5 * spectrum[0] + .25f);
            everyOther = false;
        } else
        {
            everyOther = true;
        }
        
        foreach(Material material in materials)
        {
            material.SetColor("_EmissionColor", Color.Lerp(material.color, currentColor, t));
            material.SetColor("_Color", Color.Lerp(material.color, currentColor, t));
            /*
            material.SetColor("_EmissionColor", currentColor);
            material.SetColor("_Color", currentColor);
            */
        }
        

        foreach(Light light in lights)
        {
            light.color = Color.Lerp(light.color, currentColor, t);
            light.color = new Color(light.color.r, light.color.g, light.color.b, 1);
            //light.color = currentColor;
        }


        /*for (int i = 1; i < spectrum.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        }*/
    }

        public void SetR(float value)
    {
        r = Mathf.RoundToInt(value);
    }
    public void SetG(float value)
    {
        g = Mathf.RoundToInt(value);
    }
    public void SetB(float value)
    {
        b = Mathf.RoundToInt(value);
    }
}