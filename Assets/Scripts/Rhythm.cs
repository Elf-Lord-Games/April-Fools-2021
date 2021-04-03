using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Rhythm : MonoBehaviour
{
    public float timeScale = 1;
    public float time;
    public AudioSource audioSource;

    public float prewarm = 1;

    public List<Note> notes;
    public List<Note> currentNotes;
    public List<Note> preWarmNotes;

    public List<Transform> displayedNotes;
    public GameObject[] keyPresses;
    public Vector3 initialPos;
    public Vector3 size;
    public GameObject prefab;
    public GameObject barPrefab;
    public GameObject keyPressPrefab;
    public float speed = 1;

    public Color goodColor;
    public Color badColor;

    public KeyCode[] keybinds = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Semicolon };

    public bool record = false;

    public float[] startTimes;

    public float score = 0;
    public TextMeshProUGUI scoreBox;

    private void Awake()
    {
        time = Time.time;
        timeScale = PlayerPrefs.GetFloat("Speed");
        Time.timeScale = timeScale;
        audioSource.pitch = timeScale;
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
        audioSource.Play();
    }
    private void Start()
    {
        //FixChange();
        //Debug.LogError("done");
        
        if (record || notes == null)
        {
            notes = new List<Note>();
        }
        
        currentNotes = new List<Note>();
        preWarmNotes = new List<Note>();

        //displayedNotes = new List<Transform>();

        startTimes = new float[keybinds.Length];

        GameObject g = Instantiate(barPrefab, transform);
        
        g.transform.localScale = new Vector3
        {
            x = 16 * size.x,
            y = 1.2f * size.y,
            z = .5f
        };
        Vector3 pos = (new Vector3 {x = -initialPos.x * size.z, y = initialPos.y * (-size.z + size.y), z = (size.z * speed * prewarm * -1) + g.transform.localScale.z } + initialPos * size.z);
        g.transform.position = pos;

        keyPresses = new GameObject[keybinds.Length];
        for(int i = 0; i < keybinds.Length; i++)
        {
            keyPresses[i] = Instantiate(keyPressPrefab, transform);
            keyPresses[i].transform.position = new Vector3 
            { 
                x = (i + initialPos.x) * size.x,
                y = pos.y,
                z = pos.z

            };
            keyPresses[i].transform.localScale = new Vector3 { x = size.x, y = 1, z = 1 } * 1.1f;
            keyPresses[i].SetActive(false);
        }

        score = 0;
    }
    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            EndLevel();
            return;
        }

        switch (record)
        {
            case true:
                RecordNotes();
                break;
            case false:
                PlayNotes();
                break;
        }
    }
    public void RecordNotes()
    {
        for (int i = 0; i < keybinds.Length; i++)
        {
            if (Input.GetKeyDown(keybinds[i]))
            {
                startTimes[i] = Time.time - time;
                Debug.Log(keybinds[i].ToString() + " key down");
            }
            else if (Input.GetKeyUp(keybinds[i]))
            {
                float length = (Time.time - time) - startTimes[i];
                Note note = new Note(startTimes[i], length, 0, i);
                notes.Add(note);
                Debug.Log(keybinds[i].ToString() + " key up");
            }
            Debug.Log(keybinds[i].ToString() + " was checked");
        }
        
    }
    public void PlayNotes()
    {
        float time = Time.time - this.time;
        RemoveOldNotes(time);
        AddNewNotes(time);
        ScoreNotes();
        UpdateNoteDisplay();
    }

    public void AddNewNotes(float time)
    {
        foreach(Note note in notes)
        {
            
            if (note.played)
            {
                continue;
            } 
            else if (note.startTime - prewarm <= time && note.startTime - prewarm > time - Time.deltaTime)
            {
                preWarmNotes.Add(note);
                AddNewDisplayNote(note);
            } 
            else if (note.startTime <= time && note.startTime > time - Time.deltaTime)
            {
                currentNotes.Add(note);
                preWarmNotes.Remove(note);
                note.played = true;
            }
        }
    }
    public void RemoveOldNotes(float time)
    {
        List<Note> queue = new List<Note>();
        foreach(Note note in currentNotes)
        {
            if (note.startTime + note.length <= time)
            {
                queue.Add(note);
                RemoveDisplayNote(note);
            }
        }
        foreach(Note note in queue)
        {
            currentNotes.Remove(note);
        }
    }
    public void ScoreNotes()
    {
        bool[] keysDown = new bool[keybinds.Length];
        bool[] keyNotes = new bool[keybinds.Length];
        
        for (int i = 0; i < keybinds.Length; i++)
        {
            keysDown[i] = Input.GetKey(keybinds[i]);
            if (Input.GetKeyDown(keybinds[i]))
            {
                keyPresses[i].SetActive(true);
            }
            else if (Input.GetKeyUp(keybinds[i]))
            {
                keyPresses[i].SetActive(false);
            }
        }
        
        foreach(Note note in currentNotes)
        {
            switch(note.type)
            {
                case 0:
                    keyNotes[note.key] = true;
                    //Debug.Log("THERE IS A NOTE HERE!!");
                    break;
                case 1:
                    
                    break;
            }
        }
        
        //Debug.Log(keysDown[0]);
        //Debug.Log(keyNotes[0]);
        for (int i = 0; i < keybinds.Length; i++)
        {
            //Debug.Log(keyNotes[i] && keysDown[i]);
            //Debug.Log(keyNotes[i]);
            //Debug.Log(keysDown[i]);
            if (keyNotes[i] && keysDown[i])
            {
                ScoreKeyboardNote(i);
                Debug.Log("found a good note");
            }
            else if(keysDown[i] ^ keyNotes[i])
            {
                score -= Time.deltaTime;
                ScoreBadKBNote(i);
            }
            else
            {
                ScoreBadKBNote(i);
            }
        }

        scoreBox.text = "Score: " + Mathf.RoundToInt(score * 100).ToString();
    }
    public void ScoreKeyboardNote(int x)
    {
        score += Time.deltaTime;
        keyPresses[x].GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", goodColor);
        keyPresses[x].GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", goodColor);
    }
    public void ScoreBadKBNote(int x)
    {
        keyPresses[x].GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", badColor);
        keyPresses[x].GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", badColor);
    }
    public void UpdateNoteDisplay()
    {
        foreach(Transform transform in displayedNotes)
        {
            transform.position -= new Vector3
            {
                z = Time.deltaTime * size.z * speed
            };
        }
    }
    public void AddNewDisplayNote(Note note)
    {
        switch (note.type)
        {
            case 0:
                Vector3 p = new Vector3
                {
                    x = (note.key + initialPos.x) * size.x,
                    y = initialPos.y * size.y,
                    z = ((note.length / 2) + initialPos.z) * size.z
                };
                p += new Vector3
                {
                    z = Time.deltaTime * size.z * speed
                };
                GameObject g = Instantiate(prefab, transform);
                g.transform.localScale = new Vector3
                {
                    x = size.x,
                    y = size.y,
                    z = size.z * note.length
                };
                g.transform.position = p;

                displayedNotes.Add(g.transform);

                break;
            case 1:
                
                break;
        }
    }
    public void RemoveDisplayNote(Note note)
    {

    }
    public void EndLevel()
    {
        if(score > 0)
        {
            PlayerPrefs.SetFloat("Score", PlayerPrefs.GetFloat("Score") + (score * timeScale));
            PlayerPrefs.SetInt("Levels", PlayerPrefs.GetInt("Levels") + 1);
            timeScale *= 1.2f;
            PlayerPrefs.SetFloat("Speed", timeScale);
            PlayerPrefs.Save();
            if(timeScale > 2)
            {
                SceneManager.LoadScene(2);
                return;
            }
        }
        
        SceneManager.LoadScene(1);
    }

    public void FixChange()
    {
        foreach(Note note in notes)
        {
            note.key = Mathf.RoundToInt(note.x);
        }
    }
}
