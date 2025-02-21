using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public FixedJoystick joystick;
    public float SpeedMove = 5f;
    public float rotationSpeed = 10f;

    public Transform pickedCheese;
    bool isPicked;

    public GameObject cheeseParticleEffect; // Assign the particle effect near the cheese
    public GameObject fridgeParticleEffect; // Assign the particle effect near the fridge

    private void Start()
    {
        isPicked = false;
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;
        if (moveDirection.magnitude > 0.1f) // Ensure movement input is present
        {
            // Move the player
            transform.position += moveDirection * SpeedMove * Time.deltaTime;

            // Adjust rotation to face movement direction (with 90-degree correction if needed)
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up) * Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (isPicked) 
        {
            Debug.Log("Moving");
            pickedCheese.position = transform.GetChild(7).transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cheese") 
        {
            isPicked = true;
            AudioManager.instance.PlaySFX(AudioManager.instance.pickCheeseSFX);
            cheeseParticleEffect.SetActive(false); // Stop cheese effect
            fridgeParticleEffect.SetActive(true);  // Show fridge effect
            fridgeParticleEffect.GetComponent<ParticleSystem>().Play(); // Start fridge effect
        }
    }

}
