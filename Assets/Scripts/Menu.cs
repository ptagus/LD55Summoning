using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu, Tutorial;
    public Image fader;
    bool fadestart, fadeback, tutorShow;
    public float speedFade;
    Color c;
    // Start is called before the first frame update
    void Start()
    {
        c = fader.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeback)
        {
            c.a -= Time.deltaTime * speedFade;
            fader.color = c;
            if (c.a <= 0)
            {
                c.a = 0;
                fadeback = false;
                fader.enabled = false;
            }
        }
        if (fadestart)
        {
            c.a += Time.deltaTime * speedFade;
            fader.color = c;
            if (c.a >= 1)
            {
                if (tutorShow)
                {
                    StartGame(1);
                }
                c.a = 1;
                fadestart = false;
                ShowTutorial();
            }
        }
    }

    public void StartGame(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void StartFade()
    {
        fader.enabled = true;
        fadestart = true;
    }

    void ShowTutorial()
    {
        tutorShow = true;
        mainMenu.SetActive(false);
        Tutorial.SetActive(true);
        fadeback = true;
    }
}
