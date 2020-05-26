using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour
{

    public float scrollspeed = 0.05f;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
         rend = GetComponent<Renderer>();   
         rend.material.renderQueue = -10;
    }

    // Update is called once per frame
    void Update()
    {
         float offset = -(Time.time * scrollspeed)/50;
         rend.material.SetTextureOffset("_MainTex", new Vector2(offset,0));
    }
}
