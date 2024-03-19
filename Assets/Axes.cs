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
        camera = Camera.main; // assigning camera
        audioSource = GetComponent<AudioSource>(); // getting audioSource
        StartCoroutine("Spawner"); // starting spawning coroutine
    }

    IEnumerator Spawner() // spawner
    {
        var player = GameObject.FindGameObjectWithTag("Player");// finds player
        
        // spawns axes in the same spot, the middle of the right side of the screen
        var x = Screen.width; 
        var y = Screen.height / 2;
        var position = new Vector3(x, y, 0);
        var pos = camera.ScreenToWorldPoint(position); // translate to world
        pos.z = 0; // ensures that axes are in front of background

        yield return new WaitForSeconds(2); // for an initial delay
        
        while (true)
        {
            var axe = Instantiate(Axe, pos, Quaternion.identity); // spawns the axe
            var rb = axe.GetComponent<Rigidbody2D>(); // gives it a rigid body
            audioSource.Play(); // plays associated sound
            var xVelocity = 10; // axe x velocity 
            var playerPos = player.transform.position; // assigns player position to variable
            var t = Math.Abs(playerPos.x - pos.x) / xVelocity; // calculates time
            // calculates y velocity
            var yVelocity =(float) -(0.5 * Physics2D.gravity.y * Math.Pow(t, 2) + pos.y - playerPos.y) / t;

            rb.velocity = new Vector2(-xVelocity, yVelocity); // gives calculated velocity to axe
            rb.angularVelocity = 200f; // spins the axe

            Destroy(axe, 4); // destroys the axe after 4 seconds
            yield return new WaitForSeconds(4); // waits 4 seconds before spawning a new axe
        }
    }

    public void GameOver() // upon gameover, stop spawner
    {
        StopCoroutine(nameof(Spawner));
    }
}
