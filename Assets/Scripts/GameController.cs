using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum keyTypes
{
    main,
    hold,
    maxtap
}

public enum Combo
{
    x1,
    x2,
    x3,
    x4
}
public class GameController : MonoBehaviour
{
    public Transform area;
    public float timetocreate;
    float createtimer;
    bool createstart;
    public float timer;
    bool end;

    [Header("Audios")]

    public AudioSource main;
    public AudioSource[] audios;
    public float maxVolumeForMain, maxVolumeForAudio1, maxVolumeForAudio2, maxVolumeForAudio3;
    bool[] audioplays = new bool[3];
    public float speedVolume;

    [Header("KeysElements")]

    public float transparent_speed;
    public float directionspeed;
    List<KeyBehavior> keysArray = new List<KeyBehavior>();
    public Transform[] points;
    public GameObject[] keys;
    Vector2 startpoint;

    [Header("UI")]

    public TextMeshProUGUI Timer;
    public GameObject tap;
    public Slider progressBar;
    public GameObject winPanel, losePanel;

    [Header("Counter")]

    public int basepoint;
    public int toCombox2, toCombox3, toCombox4;
    public int combox1,combox2,combox3,combox4;
    int currentcombo = 0;
    Combo nowCombo;
    // Start is called before the first frame update
    void Start()
    {
        nowCombo = Combo.x1;
        createtimer = 0;
        createstart = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            createstart = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            createstart = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            BtnPress(0);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            BtnPress(1);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            BtnPress(2);
        }
    }

    private void FixedUpdate()
    {
        if (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;
            Timer.text = "Timer: " + (int)timer;
            if (timer <= 0 && createstart)
            {
                Result(false);
            }
        }
        if (audioplays[0])
        {
            audios[0].volume += speedVolume;
            if (audios[0].volume >= maxVolumeForAudio1)
            {
                audios[0].volume = maxVolumeForAudio1;
                audioplays[0] = false;
            }
        }
        if (audioplays[1])
        {
            audios[1].volume += speedVolume;
            if (audios[1].volume >= maxVolumeForAudio2)
            {
                audios[1].volume = maxVolumeForAudio2;
                audioplays[1] = false;
            }
        }
        if (audioplays[2])
        {
            audios[2].volume += speedVolume;
            if (audios[2].volume >= maxVolumeForAudio3)
            {
                audios[2].volume = maxVolumeForAudio3;
                audioplays[2] = false;
            }
        }
        if (createstart)
        {
            createtimer += Time.fixedDeltaTime;
            if (createtimer >= timetocreate)
            {
                createtimer = 0;
                BeginCreating();
            }
        }
    }

    void BeginCreating()
    {
        if (!main.isPlaying)
        {
            main.Play();

            audios[0].Play();
            audios[1].Play();
            audios[2].Play();
            
            CreateKey(true);
            return;
        }
        if (currentcombo >= toCombox2)
        {
            Invoke("BadCreateKey", 0.75f);
        }
        if (currentcombo >= toCombox3)
        {
            Invoke("BadCreateKey", 1.25f);
        }
        CreateKey(false);
    }

    void BadCreateKey()
    {
        CreateKey(false);
    }

    void CreateKey(bool ftime)
    {
        if (ftime)
        {
            return;
        } 
        startpoint = points[Random.Range(0, points.Length)].position;
        GameObject newKey = Instantiate(keys[Random.Range(0, keys.Length)]);
        newKey.GetComponent<KeyBehavior>().gc = this.GetComponent<GameController>();
        keysArray.Add(newKey.GetComponent<KeyBehavior>());
        newKey.GetComponent<KeyBehavior>().SetValue(transparent_speed, startpoint, new Vector2(0, directionspeed), keyTypes.main);
    }

    void BtnPress(int btnnum)
    {
        if (end)
        {
            return;
        }
        foreach (KeyBehavior k in keysArray)
        {
            if (k.keyNum == btnnum)
            {
                DeleteFromList(k, true);
                break;
            }
            else
            {
                Mistake();
            }
        }
    }

    public void DeleteFromList(KeyBehavior k, bool tap)
    {
        keysArray.Remove(k);
        Destroy(k.gameObject);
        Debug.Log(keysArray.Count);
        if (!tap)
        {
            NotTap();
        }
        else
        {
            Tap();
        }
    }

    public void Mistake()
    {
        UpdateProgressBar(-1);
        tap.GetComponent<TextMeshProUGUI>().text = "CurCombo: " + currentcombo;
    }

    public void NotTap()
    {
        UpdateProgressBar(-1);
        tap.GetComponent<TextMeshProUGUI>().text = "CurCombo: " + currentcombo;
    }

    public void Tap()
    {
        currentcombo++;        
        UpdateProgressBar(basepoint);
        tap.GetComponent<TextMeshProUGUI>().text = "CurCombo: " + currentcombo;
    }

    void UpdateProgressBar(int i)
    {
        if (i > 0)
        {
            if (currentcombo >= toCombox2 && nowCombo == Combo.x1)
            {
                SetAudioPlay(0, true);
                nowCombo = Combo.x2;
                basepoint = combox2;
            }
            if (currentcombo >= toCombox3 && nowCombo == Combo.x2)
            {
                SetAudioPlay(1, true);
                nowCombo = Combo.x3;
                basepoint = combox3;
            }
            if (currentcombo >= toCombox4 && nowCombo == Combo.x3)
            {
                SetAudioPlay(2, true);
                nowCombo = Combo.x4;
                basepoint = combox4;
            }
        }
        else
        {
            if (nowCombo == Combo.x4)
            {
                SetAudioPlay(2, false);
                nowCombo = Combo.x3;
                basepoint = combox3;
                currentcombo = toCombox3;
            }
            if (nowCombo == Combo.x3)
            {
                SetAudioPlay(1, false);
                nowCombo = Combo.x2;
                basepoint = combox2;
                currentcombo = toCombox2;
            }
            if (nowCombo == Combo.x2)
            {
                SetAudioPlay(0, false);
                nowCombo = Combo.x1;
                basepoint = combox1;
                currentcombo = 0;
            }
            if (nowCombo == Combo.x1)
            {
                basepoint = combox1;
                currentcombo = 0;
            }
        }
        progressBar.value += i;
        if (progressBar.value < 0)
        {
            progressBar.value = 0;
        }
        if (progressBar.value >= progressBar.maxValue)
        {
            progressBar.value = progressBar.maxValue;
            Result(true);
        }
    }

    void SetAudioPlay(int audioclip, bool ready)
    {
        audios[audioclip].volume = 0;
        audioplays[audioclip] = ready;
    }

    void Result(bool res)
    {
        end = true;
        createstart = false;
        if (res)
        {
            Invoke("ShowWinPanel", 2);
        }
        else
        {
            ShowLosePanel();
        }
    }

    void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    void SceneStart(int i)
    {
        SceneManager.LoadScene(i);
    }
}
