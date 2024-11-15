using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    public float hitPoints = 100f;
    public float turnSpeed = 15f;
    public Transform target;
    public float ChaseRange;
    private NavMeshAgent agent;
    private float DistancetoTarget;
    private float DistancetoDefault;
    private Animator anim;
    Vector3 DefaultPosition;

    public void TakeDamage(float damage)
    {
        hitPoints -=damage;
        Debug.Log("TakeDamage called, HitPoints: " + hitPoints);
        anim.SetTrigger("GetHit");
        anim.SetFloat("Hitpoint", hitPoints);
        if (hitPoints <= 0)
        {
            Destroy(gameObject,3f);
        }

    }

    // private IEnumerator HandleDeath()
    // {
    //     yield return new WaitForSeconds(3f); // Menunggu 3 detik
    //     Destroy(gameObject); // Menghancurkan musuh setelah animasi Death selesai
    // }

  /*  public void HitConnect()
    {
        if (DistancetoTarget <= agent.stoppingDistance)
        {
            target.GetComponent<MovementLogic>().PlayerGetHit(50f);
        }
    }*/
    // Start is called before the first frame update
    void Start()
    {
    agent = this.GetComponent<NavMeshAgent>();
    anim = this.GetComponentInChildren<Animator>();
    anim.SetFloat("Hitpoint", hitPoints);
    DefaultPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DistancetoTarget = Vector3.Distance(target.position, transform.position);
        DistancetoDefault = Vector3.Distance(DefaultPosition, transform.position);
   

        if (DistancetoTarget <= ChaseRange && hitPoints != 0)
        {
            FaceTarget(target.position);
            if (DistancetoTarget > agent.stoppingDistance + 2f)
            {
                ChaseTarget();
            }
            else if (DistancetoTarget <= agent.stoppingDistance)
            {
                Attack();
                agent.ResetPath();
                anim.SetBool("Run", false);
            }
        }
        else if (DistancetoTarget >= ChaseRange && DistancetoTarget < ChaseRange * 2)
        {
            Debug.Log("Time to stop");
            agent.SetDestination(DefaultPosition);
            anim.SetBool("Run",false);
            anim.SetBool("Attack",false);
        }
         }

         private void FaceTarget(Vector3 destination)
         {
            Vector3 direction = (destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
         }

public void Attack()
{
    Debug.Log("Attack");
    anim.SetBool("Run",false);
    anim.SetBool("Attack",true);
}

public void ChaseTarget()
{
    agent.SetDestination(target.position);
    anim.SetBool("Run",true);
    anim.SetBool("Attack",false);
}

void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, ChaseRange);
}

}


