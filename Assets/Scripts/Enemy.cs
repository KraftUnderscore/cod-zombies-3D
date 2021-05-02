using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private AudioSource walkSource;

    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] attackClips;
    [SerializeField] private AudioClip[] walkClips;

    [SerializeField] private ParticleSystem particles;
    private NavMeshAgent agent;
    private Animator anim;
    private BoxCollider attackCollider;
    private CapsuleCollider hitBox;

    private Transform target;

    public float Speed
    {
        set
        {
            agent.speed = value;
        }
    }    

    public float health;
    public float damage;
    public float attackCooldown;
    private float attackTimer;

    public int pointsForHit;
    public int pointsForKill;

    private void OnTriggerStay(Collider other)
    {
        if (attackTimer > 0f) return;
        attackTimer = attackCooldown;

        if(other.CompareTag("Player"))
        {
            anim.SetTrigger("attack");
            other.GetComponent<PlayerController>().GetDamage(damage);
            attackSource.clip = attackClips[Random.Range(0, attackClips.Length)];
            attackSource.Play();
        }
    }

    public void FootStepEvent()
    {
        walkSource.clip = walkClips[Random.Range(0, walkClips.Length)];
        walkSource.Play();
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        hitBox = GetComponent<CapsuleCollider>();
        attackCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SwitchAgent(bool follow)
    {
        agent.isStopped = follow;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0f) attackTimer -= Time.deltaTime;
        agent.SetDestination(target.position);
    }

    public void GetDamage(float damage)
    {
        hitSource.clip = hitClips[Random.Range(0, hitClips.Length)];
        hitSource.Play();
        health -= damage;
        if (health <= 0) Die();
        else GameManager.instance.IncreaseScore(pointsForHit);
    }

    private void Die()
    {
        GameManager.instance.IncreaseScore(pointsForKill);
        GameManager.instance.EnemyKilled();
        agent.isStopped = true;
        hitBox.enabled = false;
        attackCollider.enabled = false;
        anim.SetBool("dead", true);
        particles.Play();
        StartCoroutine(DisableObject());
    }

    private IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(2f);
        particles.Stop();
        gameObject.SetActive(false);
        anim.SetBool("dead", false);
        hitBox.enabled = true;
        attackCollider.enabled = true;
    }
}
