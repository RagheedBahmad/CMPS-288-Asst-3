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
        var pos = transform.position;
        var mouse = camera.ScreenToWorldPoint(Input.mousePosition);
        var x = Mathf.Clamp(mouse.x, -14, 14);
        transform.position = new Vector2(x, pos.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var health = GetComponent<GameData>().playerHealth;
        if (collision.CompareTag("Apple"))
        {
            health += collision.gameObject.GetComponent<AppleProperties>().weight;
            health = Math.Min(health, 100);
            GetComponent<GameData>().playerHealth = health;
            healthDisplay.UpdateHealth(health);
            _audioSource.clip = audioClips[0];
            _audioSource.Play();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Axe"))
        {
            health -= 10;
            health = Math.Max(health, 0);
            GetComponent<GameData>().playerHealth = health;
            healthDisplay.UpdateHealth(health);
            _audioSource.clip = audioClips[1];
            _audioSource.Play();
            Destroy(collision.gameObject);
            
            var renderer = GetComponent<Renderer>();

            if (health != 0)
            {
                StartCoroutine(Flicker(renderer));
            } else {
                FindObjectOfType<Axes>().GameOver();
                FindObjectOfType<Apples>().GameOver();
                FindObjectOfType<GameOverManager>().ShowGameOverScreen();
                GameObject.FindGameObjectWithTag("Forest").GetComponent<AudioSource>().Stop();
                _audioSource.clip = audioClips[2];
                _audioSource.Play();
                StartCoroutine(Death());
            }
        }
    }

    IEnumerator Flicker(Renderer rend)
    {
        var loop = StartCoroutine(FlickerLoop(rend));
        yield return new WaitForSeconds(1f);
        StopCoroutine(loop);
        rend.enabled = true;
    }

    IEnumerator FlickerLoop(Renderer rend)
    {
        while (true)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(0.1f);
            rend.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.5f);
        var rb = this.AddComponent<Rigidbody2D>();
        var x = Random.Range(-10f, 10f);
        rb.velocity = new Vector2(x, 10f);
        rb.angularVelocity = Random.Range(-200f, 200f);
        Destroy(this, 10);
    }
}

public class GameData : MonoBehaviour
{
    public int playerHealth = 50;
}


