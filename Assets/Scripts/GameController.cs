using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum keyTypes
{
    main,
    hold,
    maxtap
}
public class GameController : MonoBehaviour
{
    public Transform[] points;
    public GameObject[] keys;
    public Transform area;
    public float timetocreate;
    bool createstart;
    public AudioSource audio;

    List<KeyBehavior> keysArray = new List<KeyBehavior>();

    [Header("Keys")]
    Vector2 startpoint;
    public float transparent_speed;
    public float directionspeed;

    [Header("UI")]

    public GameObject tap;
    public GameObject nottap;
    public GameObject mistake;

    [Header("Counter")]

    int tapcounter, ntapcounter, miscounter = 0;
    // Start is called before the first frame update
    void Start()
    {
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
        if (createstart)
        {
            timetocreate += Time.deltaTime;
            if (timetocreate >= 2)
            {
                timetocreate = 0;
                BeginCreating();
            }
        }
    }

    void BeginCreating()
    {
        if (!audio.isPlaying)
            audio.Play();
        CreateKey();
    }

    void CreateKey()
    {
        startpoint = points[Random.Range(0, points.Length)].position;
        GameObject newKey = Instantiate(keys[Random.Range(0, keys.Length)]);
        newKey.GetComponent<KeyBehavior>().gc = this.GetComponent<GameController>();
        keysArray.Add(newKey.GetComponent<KeyBehavior>());
        newKey.GetComponent<KeyBehavior>().SetValue(transparent_speed, startpoint, new Vector2(0, directionspeed), keyTypes.main);
    }

    void BtnPress(int btnnum)
    {
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
        miscounter++;
        mistake.GetComponent<TextMeshProUGUI>().text = "mistake: " + miscounter;
        Debug.Log("Mistake");
    }

    public void NotTap()
    {
        ntapcounter++;
        nottap.GetComponent<TextMeshProUGUI>().text = "notTap: " + ntapcounter;
    }

    public void Tap()
    {
        tapcounter++;
        tap.GetComponent<TextMeshProUGUI>().text = "Tap: " + tapcounter;
    }
}
