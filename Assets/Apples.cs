using System.Collections;
using UnityEngine;

public class Apples : MonoBehaviour
{
    private Coroutine[] _spawnerCoroutines = new Coroutine[4]; // array to store all spawner coroutines
    private AudioSource _audioSource;
    public GameObject Apple;
    Camera camera = new Camera();
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>(); // getting audioSource
        camera = Camera.main; // assigning camera
        for (int i = 0; i < 4; i++)
        {
            _spawnerCoroutines[i] = StartCoroutine(Spawner(i)); // starting 4 coroutines 
        }
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
            var randomX = Random.Range(x1, x2); // spawns apple randomly within given border
            var position = new Vector3(randomX, Screen.height + 50, 0); // defines position
            var pos = camera.ScreenToWorldPoint(position); // translates position
            pos.z = 0; // ensures z index is 0

            var weight = Random.Range(1, 11); // gives it random weight
            var apple = Instantiate(Apple, pos, Quaternion.identity); // spawns the apple
            _audioSource.Play(); // plays associated sound
            apple.AddComponent<AppleProperties>().weight = weight; // gives the apple its weight
            
            // interpolate the size to be in range 0.5 to 1 in proportion to weight (of range 1 to 10)
            // I found directly mapping weight to size creates very small apples
            // this is more visually appealing
            var size = (weight - 1) / 9f * 0.5f + 0.5f;
            // change radius of collider to match size
            apple.GetComponent<CircleCollider2D>().radius *= size;
            apple.transform.localScale = new Vector2(size, size); // updates apple's size
            Destroy(apple, 4); // destroys apple after 4 seconds 

            yield return pause; // waits for the random interval
        }
    }
    
    public void GameOver() // stops all coroutines upon Gameover
    {
        for(int i = 0; i < 4; i++) StopCoroutine(_spawnerCoroutines[i]);
    }
}

public class AppleProperties : MonoBehaviour
{
    public int weight;
}

