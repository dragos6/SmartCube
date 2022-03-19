
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(audioClip);
            spriteRenderer.enabled = false;
            Invoke("LoadFirstLevel", 0.5f);
        }
    }

    private void LoadFirstLevel()
    {
        
        SceneManager.LoadScene(1);
    }
}
