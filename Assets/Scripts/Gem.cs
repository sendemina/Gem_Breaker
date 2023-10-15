using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] AudioClip[] gemSounds;
    [SerializeField] GameObject gemVFX;
    [SerializeField] Sprite[] hitSprites;

    // Cached reference
    Level level;

    // State variables
    [SerializeField] int timesHit;   // Serialized for debugging purposes

    private void Start()
    {
        CountBreakableGems();
    }
    
    private void CountBreakableGems()
    { 
        if (tag == "Breakable")
        {
            level = FindObjectOfType<Level>();
            level.CountGems();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            timesHit++;
            int maxHits = hitSprites.Length + 1;
            if (timesHit >= maxHits)
            { 
                DestroyGem(); 
            }
            else
            {
                ShowNextHitSprite();
            }
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Gem sprite is missing from array" + gameObject.name);
        }
    }

    private void DestroyGem()
    {
        FindObjectOfType<GameStatus>().AddToScore();
        AudioClip clip = gemSounds[Random.Range(0, gemSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        Destroy(gameObject);
        level.GemDestroyed();
        TriggerVFX();
    }

    private void TriggerVFX()
    {
        GameObject sparkles = Instantiate(gemVFX, transform.position, transform.rotation);
        Destroy(sparkles, 2f);
    }  
}
