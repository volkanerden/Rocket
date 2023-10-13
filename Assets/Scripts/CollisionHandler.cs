using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{  
    float delayTime = 1f;

    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticle;

    AudioSource audioSource;
    Movement movement;

    public bool isTransitioning = false;
    public bool debugMode = false;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (debugMode)
            {
                debugMode = false;
            }
            else
            {
                debugMode = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isTransitioning && !debugMode)
        {
            switch (collision.gameObject.tag)
            {
                case "Launch":
                    Debug.Log("Launching Plane");
                    break;
                case "Land":
                    NextLevelSequence();
                    break;
                default:
                    CrashSequence();
                    break;
            }
        }
    }

    void NextLevelSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(successSFX);
        successParticle.Play();
        movement.enabled = false;
        Invoke("NextLevel", delayTime);
    }

    void CrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(crashSFX);
        crashParticle.Play();
        movement.enabled = false;
        Invoke("ReloadLevel", delayTime);
    }

    private void ReloadLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    private void NextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevel + 1;
        if(nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
    }
}
