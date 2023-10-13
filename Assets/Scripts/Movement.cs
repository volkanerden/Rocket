using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float torque = 50f;
    [SerializeField] float force = 500f;
    [SerializeField] AudioClip launcherSFX;
    [SerializeField] ParticleSystem engineParticle;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrust();
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
            ApplyRotation(-torque);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(torque);
        }
    }

    private void StartThrust()
    {
        rb.AddRelativeForce(Vector3.up * force * Time.deltaTime);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(launcherSFX);
        engineParticle.Play();
    }

    private void StopThrust()
    {
        audioSource.Stop();
        engineParticle.Stop();
    }

    void ApplyRotation(float torque)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.right * torque * Time.deltaTime);
        rb.freezeRotation = false;
    }
}