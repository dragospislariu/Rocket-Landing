using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in the editor

    // CACHE - e.g. references for readability or sspeed

    // STATE - private instance (member) variables

    [SerializeField] AudioClip thurstSound;
    [SerializeField] AudioClip SideThurstSound;
    [SerializeField] float speed = 1;
    [SerializeField] float rotation = 1;
    [SerializeField] ParticleSystem jetParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    Rigidbody rb;
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        

        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
         if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrust();
        }

    }
    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {

            LeftRotation();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RightRotation();
        }
        else
        {
            StopRotation();
        }

    }

    private void StopThrust()
    {
        jetParticles.Stop();
        audioSource.Stop();
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * speed * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thurstSound);
        }
        if (!jetParticles.isPlaying)
        {
            jetParticles.Play();
        }
    }

    

    private void StopRotation()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    private void RightRotation()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SideThurstSound);
        }

        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }

        ApplyRotation(-rotation);
    }

    private void LeftRotation()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(SideThurstSound);
        }

        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }

        ApplyRotation(rotation);
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;//unfreezing rotation so the physics system ca take over
    }
}
