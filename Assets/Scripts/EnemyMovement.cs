using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform currentWaypoint;
    [SerializeField] private float speed;

    Animator animator;
    private float nextAttackTime = 0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentWaypoint = WaypointProvider.Instance.GetNextWaypoint();
        nextAttackTime = Time.time + Random.Range(1f, 10f);
    }

    void Update()
    {
        var direction = currentWaypoint.position - transform.position;
        var movement = direction.normalized * speed * Time.deltaTime;

        if(movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Kontrola, zda je čas na další pokus o zapnutí animace Attack
        if (Time.time >= nextAttackTime)
        {
            if (Random.Range(0f, 1f) < 0.05f) // 5% šance na zapnutí animace
            {
                animator.SetBool("Attack", true);
                nextAttackTime = Time.time + Random.Range(1f, 10f); // Aktualizace času pro další pokus
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }

        // Kontrola, zda je rychlost (speed) enemy jednotky větší než nula
        if (speed > 0)
        {
            animator.SetInteger("state", 2); // Aktivace animace pro chůzi nastavením stavu na 2
        }
        else
        {
            animator.SetInteger("state", 1); // Deaktivace animace pro chůzi nastavením stavu na 0
        }

        transform.Translate(movement);

        if(Vector3.Distance(currentWaypoint.position, transform.position) < 0.01f)
        {
            currentWaypoint = WaypointProvider.Instance.GetNextWaypoint(currentWaypoint);
            if(currentWaypoint == null)
            {
                Destroy(gameObject);
            }
        }
    }
}