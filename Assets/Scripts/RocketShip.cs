using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShip : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    [SerializeField] float mainThrust = 2000f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine, deathExplosionSFX, successLevelSFX;
    [SerializeField] ParticleSystem mainEngineParticles, explosionParticles;
    

    Rigidbody myRigidbody;
    AudioSource myAudioSource;
    GameController gameController;
    HealthBar myHealthBar;
    

    bool isAlive = true;
    int currentHealth;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();

        gameController=FindObjectOfType<GameController>();
        myHealthBar= FindObjectOfType<HealthBar>();

        currentHealth = maxHealth;
        myHealthBar.SetMaxHealth(maxHealth);

    }
   
        
    
    void Update()
    {
        if (isAlive)
        {
            RocketMovement();
        }
      
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive || !gameController.collisionEnabled)
        { 
            return;
        }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("Im okay");
                    break;
                }
            case "Finish":
                {
                    SuccessRoutine();
                    break;
                }
            default:
                {
                    TakeDamage(25);
                    break;
                }



        }
    }

    private void DeathRoutine()
    {
        explosionParticles.Play();
        AudioSource.PlayClipAtPoint(deathExplosionSFX, Camera.main.transform.position);
        isAlive = false;
       
        gameController.ResetGame();
        
    }

    private void SuccessRoutine()
    {
        AudioSource.PlayClipAtPoint(successLevelSFX, Camera.main.transform.position);
        myRigidbody.isKinematic = true;
        gameController.NextLevel();
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        myHealthBar.SetHealth(currentHealth);

        FindObjectOfType<CanvasFade>().Fade();

        if (currentHealth==0)
        {
            DeathRoutine();
        }
    }

private void RocketMovement()
    {
        float rotationSpeed = Time.deltaTime * rotationThrust;
        
        Thrusting();
        Rotating(rotationSpeed);
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(mainEngine);
            }
            myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            mainEngineParticles.Play();
        }
        else
        {
            mainEngineParticles.Stop();
            myAudioSource.Stop();
        }
    }

    private void Rotating(float rotationSpeed)
    {
        myRigidbody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        myRigidbody.freezeRotation=false;   
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = new  Vector3(0,0,transform.localEulerAngles.z);
    }
}
