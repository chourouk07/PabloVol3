using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    CharacterStats stats;
    //public Transform player;
    NavMeshAgent agent;
    public float attackDistance = 5f;
    Animator animator;
    bool canAttack = true;
    float attackCoolDown = 1f;
    // Start is called before the first frame update
    void Start()
    {

    animator = GetComponentInChildren<Animator>();
     agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        float distance = Vector3.Distance(transform.position,LevelManager.instance.player.position);
        if (distance < attackDistance)
        {
            agent.SetDestination(LevelManager.instance.player.position);
            if (distance<= agent.stoppingDistance) { 
                if (canAttack)
                {
                    Enemy_Attack_ani();
                    StartCoroutine(coolDown());
                }
            }
        }
    }
    IEnumerator coolDown()
    {
        canAttack = false; 
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && LevelManager.instance.player.GetComponent<PlayerMovement2>().playerIsAttacking)
        {
            stats.ChangeHealth(-other.GetComponentInParent<CharacterStats>().power);
            Debug.Log("Damage Enemy");
        }
    }

    public void DamagePlayer()
    {
       float posX = LevelManager.instance.player.transform.position.x;
        float posY = LevelManager.instance.player.transform.position.y + 3f;
        float posZ = LevelManager.instance.player.transform.position.z-1.0f;
        Vector3 bloodPos = new Vector3(posX, posY, posZ);

            LevelManager.instance.player.GetComponent<CharacterStats>().ChangeHealth(-stats.power);
            Instantiate(LevelManager.instance.particles[1],bloodPos, LevelManager.instance.player.transform.rotation);
        
    }
    public void Enemy_Attack_ani()
    {
        animator.SetTrigger("Attack");

    }
}
