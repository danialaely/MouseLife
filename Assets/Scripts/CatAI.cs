using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatAI : MonoBehaviour
{
    public Transform[] patrolPoints; // Assign Waypoint1 and Waypoint2 in Inspector
    public Transform mouse; // Assign Mouse in Inspector
    public float sightRange = 5f; // Cat's vision range
    public float chaseSpeed = 5f; // Speed when chasing Mouse
    public float patrolSpeed = 2f; // Speed when patrolling
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;

    public GameObject retryPanel;

    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();
        retryPanel.SetActive(false);
    }

    void Update()
    {
        if (CanSeeMouse())
        {
            ChaseMouse();
        }
        else if (!isChasing && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }

        // Rotate the cat in the direction it's moving
        if (agent.velocity.magnitude > 0.1f) // Only rotate if moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized) * Quaternion.Euler(0, 0, 0);
            //Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up) * Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    bool CanSeeMouse()
    {
        float distance = Vector3.Distance(transform.position, mouse.position);
        if (distance < sightRange)
        {
            Vector3 directionToMouse = (mouse.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, directionToMouse) > 0.5f) // Check if Mouse is in front
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToMouse, out hit, sightRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void ChaseMouse()
    {
        isChasing = true;
        agent.speed = chaseSpeed;
        agent.destination = mouse.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Level Failed!");
            AudioManager.instance.PlaySFX(AudioManager.instance.levelFailedSFX);
            StartCoroutine(ActiveRetryPanel(1.0f));
        }
    }

    IEnumerator ActiveRetryPanel(float del) 
    {
        yield return new WaitForSeconds(del);
        retryPanel.SetActive(true);
    }
}
