using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBehavior : MonoBehaviour
{
    public GameController gc;
    public Sprite[] spritetypes;
    public int keyNum;
    public float livetime;
    float transtimer;
    bool click;
    //public Image image;
    public SpriteRenderer img;
    Color c;
    // Start is called before the first frame update
    void Start()
    {
        c = img.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        livetime -= Time.deltaTime;
        if (livetime <= 0 && !click)
        {
                Miss();
        }

        if (click)
        {
            c.a -= transtimer;
            img.color = c;
        }

        if (c.a <= 0)
        {
            DestroyIt();
        }
    }
    public void SetValue(float time, Vector2 pos)
    {
        transtimer = time;
        transform.position = pos;
    }

    void Miss()
    {
        transtimer *= 4;
        click = true;
        img.sprite = spritetypes[0];
        gc.DeleteFromList(this, false);
    }

    void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    public void AnimOnDestroy()
    {
        transtimer *= 4;
        img.sprite = spritetypes[1];
        click = true;
    }
}


