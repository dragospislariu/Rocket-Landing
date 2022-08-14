using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayValue = 1f;
    [SerializeField]AudioClip explosionSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem successParticles;


    AudioSource audioSource;

    bool isTransitoning=false;
    bool isCollisionDisabled = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

    private void Update()
    {
        RespondToDebugKeys();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isTransitoning || isCollisionDisabled) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Bumped into a friedly obstacle");
                break;
           
            case "Finish":
                StartLoadSequence();
                break;
            default:
                StartCrashSequence();

                break;

        }
    }

    void StartLoadSequence()
    {
        isTransitoning = true;
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        GetComponent<Movement>().enabled = false;
        Invoke("FinishLevel", delayValue);
    }

    void StartCrashSequence()
    {
        isTransitoning = true;
        explosionParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(explosionSound);
        
        GetComponent<Movement>().enabled = false;
       Invoke("ReloadLevel",delayValue);
    }

    void ReloadLevel()
        {
            int curentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(curentSceneIndex);
        }

        void FinishLevel()
        {
            int curentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = curentSceneIndex + 1;
            if(nextSceneIndex==SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            FinishLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled; //toggle collision
        }
    }

}
