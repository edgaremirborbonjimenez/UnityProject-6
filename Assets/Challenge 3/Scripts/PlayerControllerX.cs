using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    public float floatForce = .2f;
    public float gravityModifier = .5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    private float limitUp = 13f,limitDown=0f;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //Physics.gravity *= gravityModifier;
        Physics.gravity = gravityModifier * new Vector3(0, -9.81f, 0);
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);

    }

    // Update is called once per frame
    void Update()
    {
        chechInBound();
        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce,ForceMode.VelocityChange);
        }
    }

    private void chechInBound()
    {
        if (transform.position.y > limitUp)
        {
            transform.position = new Vector3(transform.position.x, limitUp, transform.position.z);
            playerRb.AddForce(Vector3.down * 10, ForceMode.VelocityChange);

        }
        else
        {
            if (transform.position.y < limitDown)
            {
                transform.position = new Vector3(transform.position.x, limitDown, transform.position.z);
                playerRb.AddForce(Vector3.up * 10, ForceMode.VelocityChange);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.transform.position = other.gameObject.transform.position;
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
            Invoke("restartGame",1.0f);
            
        } 

        // if player collides with money, fireworks
         if (other.gameObject.CompareTag("Money"))
         {
             fireworksParticle.transform.position = other.gameObject.transform.position;
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }
    private void destroyPlayer(){
        Destroy(this.gameObject);
    }
    
    void restartGame()
    {
        SceneManager.LoadSceneAsync("Challenge 3");
    }
}
