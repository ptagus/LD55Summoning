using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBehavior : MonoBehaviour
{
    [Header("Fill sprite")]
    float speed;
    public SpriteRenderer srfilled;
    public float maxSize;
    bool startfill;
    Vector2 sizeChanger;

    [Header("Key")]
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
        sizeChanger = new Vector2(0, speed);
        c = img.color;
        startfill = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (startfill)
        {
            srfilled.size += sizeChanger;
        }

        if (srfilled.size.y >= maxSize)
        {
            srfilled.size = new Vector2(srfilled.size.x, maxSize);
            startfill = false;
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
    public void SetValue(float time, Vector2 pos, float speedfill)
    {
        speed = speedfill;
        transtimer = time;
        transform.position = pos;
    }

    void Miss()
    {
        transtimer *= 4;
        click = true;
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
        srfilled.enabled = false;
        click = true;
    }
}


