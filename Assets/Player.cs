using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Camera camera = new Camera();
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        var mouse = camera.ScreenToWorldPoint(Input.mousePosition);
        var x = Mathf.Clamp(mouse.x, -14, 14);
        transform.position = new Vector2(x, pos.y);
    }
}
