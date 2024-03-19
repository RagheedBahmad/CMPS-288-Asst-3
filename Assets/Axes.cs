using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Axes : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject Axe;
    Camera camera = new Camera();
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawner()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        
        var x = Screen.width;
        var y = Screen.height / 2;
        var position = new Vector3(x, y, 0);
        var pos = camera.ScreenToWorldPoint(position);
        pos.z = 0;

        yield return new WaitForSeconds(2); // for an initial delay
        
        while (true)
        {
            var axe = Instantiate(Axe, pos, Quaternion.identity);
            var rb = axe.GetComponent<Rigidbody2D>();
            audioSource.Play();
            var xVelocity = 10;
            var playerPos = player.transform.position;
            var t = Math.Abs(playerPos.x - pos.x) / xVelocity;
            var yVelocity =(float) -(-0.5 * -Physics2D.gravity.y * Math.Pow(t, 2) + pos.y - playerPos.y) / t;

            rb.velocity = new Vector2(-xVelocity, yVelocity);
            rb.angularVelocity = 200f;

            Destroy(axe, 4);
            yield return new WaitForSeconds(4);
        }
    }

    public void GameOver()
    {
        StopCoroutine(nameof(Spawner));
    }
}
