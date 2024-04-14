using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class SpriteTest : MonoBehaviour
{
    public float speed;
    public SpriteRenderer srfilled;
    public float maxSize;
    bool startfill;
    Vector2 sizeChanger;
    // Start is called before the first frame update
    void Start()
    {
        sizeChanger = new Vector2(0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TestAp(true);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TestAp(false);
        }

        if (startfill)
        {
            srfilled.size += sizeChanger;
        }

        if (srfilled.size.y >= maxSize)
        {
            srfilled.size = new Vector2(srfilled.size.x, maxSize);
            startfill = false;
        }
    }

    void TestAp(bool x)
    {
        if (x)
            startfill = true;
        else
        {
            startfill = false;
            srfilled.size = new Vector2(srfilled.size.x, 0);
        }
    }
}
