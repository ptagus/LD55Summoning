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
    public OnStart VsyaHyinya;
    public Transform area;
    public float timetocreate;
    float createtimer;
    bool createstart;
    public float timer;
    public int pointsToWin;
    bool end;
    public Animator[] shamans;
    public Animator comboAnim, backAnim;
    public AudioClip winMusic, loseMusic;
    public float mainVolume;

    [Header("Audios")]

    public AudioSource main;
    public AudioSource[] audios;
    public float maxVolumeForMain, maxVolumeForAudio1, maxVolumeForAudio2, maxVolumeForAudio3;
    bool[] audioplays;
    public float speedVolume;

    [Header("KeysElements")]

    public float transparent_speed;
    public float directionspeed;
    List<KeyBehavior> keysArray = new List<KeyBehavior>();
    public Transform[] points;
    public GameObject[] keys;
    Vector2 startpoint;

    [Header("UI")]

    public GameObject allUI;
    public TextMeshProUGUI Timer;
    public GameObject tap;
    public Slider progressBar;
    public Image baseSlaider, fullSlider;
    public GameObject winPanel, losePanel;
    public Image[] comboSprites;
    bool combospritechange;
    GameObject tempCombo;

    [Header("Counter")]

    public int basepoint;
    public int basepointNegative;
    public int toCombox2, toCombox3, toCombox4;
    public int combox1,combox2,combox3,combox4;
    int currentcombo = 0;
    Combo nowCombo;
    // Start is called before the first frame update
    void Start()
    {
        audioplays = new bool[audios.Length];
        main.volume = maxVolumeForMain;
        progressBar.maxValue = pointsToWin;
        progressBar.value = 0;
        nowCombo = Combo.x1;
        createtimer = 2;
        createstart = true;
        Debug.Log("Hi? Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneStart(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneStart(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            BtnPress(0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            BtnPress(1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            BtnPress(2);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            BtnPress(3);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BtnPress(4);
        }
    }

    private void FixedUpdate()
    {
        if (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;
            Timer.text = ""+(int)timer;
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
            foreach(Animator a in shamans)
                a.enabled = true;

            main.Play();
            foreach (AudioSource a in audios)
                a.Play();

            comboAnim.GetComponent<Animator>().enabled = true;
            backAnim.GetComponent<Animator>().enabled = true;

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
        BadCreateKey();
    }

    void BadCreateKey()
    {
        CreateKey(false);
    }

    void CreateKey(bool ftime)
    {
        if (end)
        {
            return;
        }
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
        k.AnimOnDestroy();        
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
        UpdateProgressBar(basepointNegative);
        tap.GetComponent<TextMeshProUGUI>().text = "CurCombo: " + currentcombo;
    }

    public void NotTap()
    {
        UpdateProgressBar(basepointNegative);
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
        if (end)
        {
            return;
        }
        if (i > 0)
        {
            if (currentcombo >= toCombox2 && nowCombo == Combo.x1)
            {
                ComboChange(0, true, Combo.x2, combox2, true);
            }
            if (currentcombo >= toCombox3 && nowCombo == Combo.x2)
            {
                ComboChange(1, true, Combo.x3, combox3, true);
            }
            if (currentcombo >= toCombox4 && nowCombo == Combo.x3)
            {
                ComboChange(2, true, Combo.x4, combox4, true);
            }
        }
        else
        {
            if (nowCombo == Combo.x1)
            {
                basepoint = combox1;
                currentcombo = 0;
            }
            if (nowCombo == Combo.x2)
            {
                ComboChange(0, false, Combo.x1, combox1, false);
                currentcombo = 0;
            }
            if (nowCombo == Combo.x3)
            {
                ComboChange(1, false, Combo.x2, combox2, false);
                currentcombo = toCombox2;
            }
            if (nowCombo == Combo.x4)
            {
                ComboChange(2, false, Combo.x3, combox3, false);
                currentcombo = toCombox3;
            }
            
        }
        progressBar.value += i;
        if (progressBar.value < 0)
        {
            progressBar.value = 0;
        }
        if (progressBar.value >= pointsToWin)
        {
            progressBar.value = progressBar.maxValue;
            Result(true);
        }
    }

    void ComboChange(int audioclip, bool audiostart, Combo combo, int combomul, bool up)
    {
        SetAudioPlay(audioclip, audiostart);
        nowCombo = combo;
        basepoint = combomul;
        ChangeComboSprites(combomul, up);
    }

    void ChangeComboSprites(int comborange, bool up)
    {
        Debug.Log("Comborabge" + comborange);
        if (up)
        {
            comboSprites[comborange - 1].enabled = true;
        }
        else
        {
            comboSprites[comborange].enabled = false;
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
            baseSlaider = fullSlider;
            allUI.SetActive(false);
            Invoke("ShowWinPanel", 2);
        }
        else
        {
            ShowLosePanel();
        }
    }

    void ShowWinPanel()
    {
        VsyaHyinya.AfterWinLose();
        allUI.SetActive(false);
        main.clip = winMusic;
        main.Play();
        main.volume = mainVolume;
        foreach(AudioSource a in audios)
        {
            a.volume = 0;
        }
        foreach (Animator a in shamans)
            a.gameObject.SetActive(false);
        comboAnim.GetComponent<Animator>().enabled = false;
        backAnim.GetComponent<Animator>().enabled = false;
        winPanel.SetActive(true);
    }

    void ShowLosePanel()
    {
        VsyaHyinya.AfterWinLose();
        allUI.SetActive(false);
        main.clip = loseMusic;
        main.Play();
        foreach (AudioSource a in audios)
        {
            a.volume = 0;
        }
        foreach (Animator a in shamans)
            a.gameObject.SetActive(false);
        comboAnim.GetComponent<Animator>().enabled = false;
        backAnim.GetComponent<Animator>().enabled = false;
        losePanel.SetActive(true);
    }

    void SceneStart(int i)
    {
        if (SceneManager.sceneCount == i | i< 0)
        {
            return;
        }
        SceneManager.LoadScene(i);
    }
}
