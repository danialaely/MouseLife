using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class Cheese : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fridgeParticleEffect; // Assign the particle effect near the fridge
    public GameObject confettiParticleEffect; // Assign the particle effect near the cheese
    public TMP_Text LevelTxt;

    public GameObject lvlCompletePanel;
    void Start()
    {
        lvlCompletePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("MFridge"))
        {
            Debug.Log("Level Completed");

            fridgeParticleEffect.SetActive(false); // Stop cheese effect
            confettiParticleEffect.SetActive(true);  // Show fridge effect
            confettiParticleEffect.GetComponent<ParticleSystem>().Play(); // Start fridge effect
            LevelTxt.gameObject.SetActive(true);
            AudioManager.instance.PlaySFX(AudioManager.instance.levelCompleteSFX);
            StartCoroutine(DeactiveLevelTxt(1.0f));
            StartCoroutine(ActivateLvlCompPanel(1.0f));
        }
        
    }

    IEnumerator DeactiveLevelTxt(float del) 
    {
        yield return new WaitForSeconds(del);
        LevelTxt.gameObject.SetActive(false);
    }

    IEnumerator ActivateLvlCompPanel(float del) 
    {
        yield return new WaitForSeconds(del);
        lvlCompletePanel.SetActive(true);
    }
}
