using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] Hands hands1;
    [SerializeField] float xPush = 3f;
    [SerializeField] float yPush = 10f;
    [SerializeField] AudioClip[] sphereSounds;
    [SerializeField] float randomFactor = 0.2f;
    [SerializeField] int maxHits = 200;
    [SerializeField] Sprite[] hitSprites;

    // State
    Vector2 handsToSphereVector;
    bool hasStarted = false;
    bool hasEnded = false;

    // Cached component references
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;

    // State variables
    [SerializeField] int timesHit;   // Serialized for debugging purposes

    // Start is called before the first frame update
    void Start()
    {
        handsToSphereVector = transform.position - hands1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasEnded)
        {
            if (!hasStarted)
            {
                LockSphereToHands();
                LaunchOnMouseClick();
            }
        }
    }

    private void LockSphereToHands()
    {
        Vector2 handsPos = new Vector2(hands1.transform.position.x, hands1.transform.position.y);
        transform.position = handsPos + handsToSphereVector;
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            myRigidBody2D.velocity = new Vector2(xPush, yPush);       
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckDamageLevel(); 
        Vector2 velocityTweak = new Vector2(Random.Range(0f, randomFactor), Random.Range(0f, randomFactor));
        if (hasStarted)
        {
            AudioClip clip = sphereSounds[Random.Range(0, sphereSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidBody2D.velocity += velocityTweak;
        }
    }

    private void CheckDamageLevel()
    {
        timesHit++;
        if (timesHit >= maxHits)
        {
            DestroySphere();
        }
        else if (timesHit >= 100)
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit / 100 - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Sphere sprite is missing from array " + gameObject.name);
        }
    }

    private void DestroySphere()
    {
        Destroy(gameObject);
        FindObjectOfType<SceneLoader>().LoadEndScene();
        hasEnded = true;
    }
}
