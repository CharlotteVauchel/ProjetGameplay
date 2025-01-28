using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Enemy_Simple : MonoBehaviour
{
    public Transform destination;
    [Range(0.5f, 50)]
    public float detectDistance = 3;
    public Transform[] points;
    NavMeshAgent agent;
    int destinationIndex = 0;
    Transform player;
    float speedBase;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.destination = points[destinationIndex].position;
        }
        speedBase = agent.speed;
    }

    private void Update()
    {
        Walk();
        SearchPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }

    public void Walk()
    {
        float distance = agent.remainingDistance; //Distance entre la position de notrre agent et la destination actuelle
        if (distance <= 0.05f)
        {
            int destinationRandom = Random.Range(0, points.Length);
            agent.destination = points[destinationRandom].position;
        }
    }

    public void SearchPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectDistance)
        {
            //Le joueur est détecté
            agent.destination = player.position;
            agent.speed += 0.5f;
        }
        else
        {
            agent.speed = speedBase;
        }
    }

}
