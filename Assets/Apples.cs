using System.Collections;
using UnityEngine;

public class Apples : MonoBehaviour
{
    private Coroutine[] _spawnerCoroutines = new Coroutine[4];
    private AudioSource _audioSource;
    public GameObject Apple;
    Camera camera = new Camera();
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        camera = Camera.main;
        for (int i = 0; i < 4; i++)
        {
            _spawnerCoroutines[i] = StartCoroutine(Spawner(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Spawner(int i)
    {
        // adds random initial delay to avoid all 4 apples from initially spawning together
        yield return new WaitForSeconds(Random.Range(2f, 7f)); 
        
        var screenWidth = Screen.width;
        var x1 = screenWidth * (i / 4f); // calculates the left border of the coroutine
        var x2 = screenWidth * ((i + 1) / 4f); // calculates the right border of the coroutine
        var x = (x1 + x2) / 2;
        
        while (true)
        {
            var pause = new WaitForSeconds(Random.Range(2f, 10f)); // calculates new random delay
            var randomX = Random.Range(x1, x2);
            var position = new Vector3(randomX, Screen.height + 50, 0); // defines position
            var pos = camera.ScreenToWorldPoint(position); // translates position
            pos.z = 0; // ensures z index is 0

            var weight = Random.Range(1, 11);
            var apple = Instantiate(Apple, pos, Quaternion.identity); // spawns the apple
            _audioSource.Play();
            apple.AddComponent<AppleProperties>().weight = weight;
            
            // interpolate the size to be in range 0.5 to 1 in proportion to weight (of range 1 to 10)
            var size = (weight - 1) / 9f * 0.5f + 0.5f;
            // change radius of collider to match size
            apple.GetComponent<CircleCollider2D>().radius *= size;
            apple.transform.localScale = new Vector2(size, size);
            Destroy(apple, 4); // destroys apple after 4 seconds 

            yield return pause; // waits for the random interval
        }
    }
    
    public void GameOver()
    {
        for(int i = 0; i < 4; i++) StopCoroutine(_spawnerCoroutines[i]);
    }
}

public class AppleProperties : MonoBehaviour
{
    public int weight;
}

