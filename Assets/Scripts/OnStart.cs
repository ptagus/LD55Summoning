using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnStart : MonoBehaviour
{
    public GameObject[] DlyaVseiHyini;
    public GameObject shamans;
    public GameObject ui;
    public GameObject game;
    public GameObject fader;
    public GameObject showStartPanel;
    public GameObject[] numbers;
    public GameObject nums;
    public float speedFade;
    bool fade, numb, newfade;
    float numtime = 1;
    int num = 0;
    int scene;
    Color c;
    // Start is called before the first frame update
    void Start()
    {
        fade = true;
        c = fader.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (numb)
        {
            numtime += Time.deltaTime;
            if (numtime >= 1)
            {
                Shownums(num);
                num++;
                numtime = 0;
            }
        }

        if (fade)
        {
            c.a -= Time.deltaTime * speedFade;
            fader.GetComponent<Image>().color = c;
            if (c.a <= 0)
            {
                c.a = 0;
                fade = false;
                ShowStartPanel();
            }
        }

        if (newfade)
        {
            c.a += Time.deltaTime * speedFade;
            fader.GetComponent<Image>().color = c;
            if (c.a >= 1)
            {
                c.a = 1;
                fade = false;
                SceneStart(scene);
            }
        }
    }

    public void BeforeStart()
    {
        numb = true;
        showStartPanel.SetActive(false);
        shamans.SetActive(true);
        foreach (GameObject go in DlyaVseiHyini)
            go.SetActive(true);
    }

    void Shownums(int n)
    {
        if (n > 0)
        {
            numbers[n - 1].SetActive(false);
        }
        if (n == 3)
        {
            EnableGame();
            return;
        }
        numbers[n].SetActive(true);
    }

    void EnableGame()
    {
        nums.SetActive(false);
        ui.SetActive(true);
        game.SetActive(true);
        numb = false;
    }

    void ShowStartPanel()
    {
        showStartPanel.SetActive(true);
    }

    public void EndOfScene(int i)
    {
        newfade = true;
        scene = i;
    }

    public void EndOfScene()
    {
        newfade = true;
        scene = SceneManager.GetActiveScene().buildIndex;
    }

    void SceneStart(int i)
    {
        SceneManager.LoadScene(i);
    }

}
