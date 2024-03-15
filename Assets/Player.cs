using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    Camera camera = new Camera();
    public HealthDisplay healthDisplay;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
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
        if (collision.CompareTag("Apple"))
        {
            var health = GetComponent<GameData>().playerHealth += collision.gameObject.GetComponent<AppleProperties>().weight;
            health = Math.Min(health, 100);
            healthDisplay.UpdateHealth(health);
            Destroy(collision.gameObject);
        }
    }
}

public class GameData : MonoBehaviour
{
    public int playerHealth = 10;
}


