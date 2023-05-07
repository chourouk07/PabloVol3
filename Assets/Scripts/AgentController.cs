using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public Animator enemyAnim;
    public float wanderSpeed = 5f; //movement speed mta3 agent w howa wendering 
    public float maxForce = 10f; //max force ynajem yamelha bech ybadel el deraction wala speed 
    public float detectionRadius = 10f; //radius which the agent can detect the player.
    public float avoidanceRadius = 6f; //radius which the agent can detect obstacles and avoid them.
    public float chaseSpeed = 10f; //determines how fast the agent moves when chasing the player.
    //Attack Variables
    [SerializeField] private float attackDistanceThreshold = 2f;
    [SerializeField] private float attackForce = 100f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private bool attacked = false;
    private float timeSinceLastAttack = 0f;

    private Vector3 targetPosition;
    public GameObject player;

    private void Start()
    {
        SetNewTargetPosition(); //method, which chooses a random position within a certain range
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (DetectPlayer()) { 
            ChasePlayer();
        }
       else
        {
            Wander();
       }
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackDistanceThreshold && !attacked )
        {
            enemyAnim.SetTrigger("Attack");
            attacked = true;
        }
        StartCoroutine(ResetAttack(2.0f));


    }

    private bool DetectPlayer()
    {
        //Physics.OverlapSphere() to get an array of colliders within the radius and checks if any of them belong to the player game object
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    //chase player
    private void ChasePlayer()
    {
        Vector3 chaseDirection = (player.transform.position - transform.position).normalized;
        transform.LookAt(player.transform.position);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        Vector3 chaseForce = chaseDirection * chaseSpeed - GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().AddForce(chaseForce);

        // Check if object is moving, then set "Run" animation to true
        if (chaseForce.magnitude > 0)
        {
            enemyAnim.SetBool("Run", true);
        }
        else
        {
            enemyAnim.SetBool("Run", false);
        }
    }

    private void Wander()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            SetNewTargetPosition();
        }

        Vector3 wanderForce = (targetPosition - transform.position).normalized * wanderSpeed - GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().AddForce(wanderForce);
    }

    /*private void AvoidObstacles()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, avoidanceRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Obstacle"))
            {
                Vector3 avoidanceDirection = (transform.position - collider.transform.position).normalized;
                float avoidanceWeight = Mathf.Clamp01(1f - Vector3.Distance(transform.position, collider.transform.position) / avoidanceRadius);
                GetComponent<Rigidbody>().AddForce(avoidanceDirection * maxForce * avoidanceWeight);
            }
        }
    }*/

    private void SetNewTargetPosition()
    {
        float wanderRange = 10f;
        float x = Random.Range(-wanderRange, wanderRange);
        float z = Random.Range(-wanderRange, wanderRange);
        targetPosition = new Vector3(x, transform.position.y, z);
    }

    IEnumerator ResetAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        enemyAnim.ResetTrigger("Attack");
    }

    //Zombie Animations
    private void Run()
    {
        enemyAnim.SetBool("Run",true);
    }
    private void Attack()
    {
        enemyAnim.SetTrigger("Attack");
    }
    private void Die()
    {
        enemyAnim.SetTrigger("Die");
    }
}
   
