using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBehavior : MonoBehaviour
{
    public GameController gc;
    public int keyNum;
    float transtimer;
    Vector2 direction;
    keyTypes ktype;
    bool del;
    //public Image image;
    public SpriteRenderer img;
    Color c;
    // Start is called before the first frame update
    void Start()
    {
        c = img.color;
        //c = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //transform.Translate(direction);
        c.a -= transtimer;
        img.color = c;
        //image.color = c;
        if (c.a <= 0 && del == false)
        {
            del = true;
            DestroyIt();
        }
    }
    public void SetValue(float time, Vector2 pos, Vector2 dir, keyTypes key)
    {
        transtimer = time;
        transform.position = pos;
        direction = dir;
        ktype = key;
    }

    void DestroyIt()
    {
        gc.DeleteFromList(this, false);
    }

    public void AnimOnDestroy()
    {
        Destroy(this.gameObject);
    }
}


