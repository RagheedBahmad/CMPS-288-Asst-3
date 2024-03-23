using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    Camera camera = new Camera();
    public TextMeshProUGUI gameOverText;
    public HealthDisplay healthDisplay;
    private AudioSource _audioSource;
    public AudioClip[] audioClips;

    private Boolean _isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
        this.AddComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver)
        {
            var pos = transform.position; // get player position
            var mouse = camera.ScreenToWorldPoint(Input.mousePosition); // get mouse position in world
            var x = Mathf.Clamp(mouse.x, -14, 14); // stops the player from going out of bounds
            transform.position = new Vector2(x, pos.y); // moves player with mouse
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var health = GetComponent<GameData>().playerHealth; // stores health in variable
        if (collision.CompareTag("Apple")) // if hit apple
        {
            health += collision.gameObject.GetComponent<AppleProperties>().weight; // add apple's weight to health
            health = Math.Min(health, 100); // clamp the health at 100
            GetComponent<GameData>().playerHealth = health; // update health in GameData class
            healthDisplay.UpdateHealth(health); // update health in the display
            _audioSource.clip = audioClips[0]; // load appropriate audio
            _audioSource.Play(); // play audio
            Destroy(collision.gameObject); // destroy apple
        }

        if (collision.CompareTag("Axe")) // if hit axe
        {
            health -= 20; // remove 20 health
            health = Math.Max(health, 0); // clamp health at 0
            GetComponent<GameData>().playerHealth = health; // update health in GameData
            healthDisplay.UpdateHealth(health); // update display
            _audioSource.clip = audioClips[1]; // load appropriate audio
            _audioSource.Play(); // and play it

            var axe = collision.gameObject;
            axe.GetComponent<BoxCollider2D>().enabled = false; // to avoid the axe hitting the player more than once
            axe.GetComponent<Rigidbody2D>().velocity = new Vector3(1, 0); // gives it a bouncing off effect
            axe.GetComponent<Rigidbody2D>().angularVelocity = 0; // stops its rotation

            var renderer = GetComponent<Renderer>(); // get renderer for flicker effect

            if (health != 0) // if not dead flicker
            {
                StartCoroutine(Flicker(renderer));
            } else { // if dead go to game over
                _isGameOver = true;
                foreach (var apple in GameObject.FindGameObjectsWithTag("Apple"))
                {
                    Destroy(apple);
                }
                FindObjectOfType<Axes>().GameOver(); // stops axe spawner
                FindObjectOfType<Apples>().GameOver(); // stops apple spawner
                FindObjectOfType<GameOverManager>().ShowGameOverScreen(); // shows game over screen
                GameObject.FindGameObjectWithTag("Forest").GetComponent<AudioSource>().Stop(); // stops music
                _audioSource.clip = audioClips[2]; // loads game over chime
                _audioSource.Play(); // plays it
                StartCoroutine(Death()); // starts death coroutine
            }
        }
    }

    IEnumerator Flicker(Renderer rend)
    {
        var loop = StartCoroutine(FlickerLoop(rend)); // loop that flickers that player
        yield return new WaitForSeconds(1f); // plays the flicker for 1 second
        StopCoroutine(loop); // stops the effect
        rend.enabled = true; // ensures the renderer is enabled
    }

    IEnumerator FlickerLoop(Renderer rend) // loop that flickers the player
    {
        while (true)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(0.1f);
            rend.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Death() // death animation
    {
        yield return new WaitForSeconds(1f); // waits for a second for dramatic effect
        var rb = this.AddComponent<Rigidbody2D>(); // adds rigid body for gravity 
        var x = Random.Range(-5f, 5f); // calculates random x velocity
        rb.velocity = new Vector2(x, 15f); // gives the player that velocity
        rb.angularVelocity = Random.Range(-200f, 200f); // spins the player in either direction
        Destroy(this, 10); // destroy the player after 10 seconds
    }
}

public class GameData : MonoBehaviour
{
    public int playerHealth = 50;
}


