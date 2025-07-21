using System.Collections;
using UnityEngine;

public class EnemyCircleController : MonoBehaviour
{
    private GameObject target;
    private GameObject gameDirector;
    private GameObject enemyGenerator;
    private Rigidbody2D rigid2D;
    private float speed = 10f;
    private float maxWalkSpeed = 1.5f;
    private float stoppingDistance = 0.5f;
    private int hpGauge = 10;
    private float duration = 0.125f;
    [SerializeField] private HitEffect hitEffect;
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector");
        enemyGenerator = GameObject.Find("EnemyGenerator");
        target = GameObject.FindGameObjectWithTag("Player");
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        int direction = 0;
        float distanceX = Mathf.Abs(transform.position.x - target.transform.position.x);

        float speedx = Mathf.Abs(rigid2D.velocity.x);

        if (transform.position.x < target.transform.position.x) direction = 1; //¿À
        if (transform.position.x > target.transform.position.x) direction = -1; // ¿Þ

        Vector2 movement = new Vector2(direction, 0);
        if (speedx < maxWalkSpeed)
        {
            rigid2D.AddForce(movement * speed * Time.timeScale);
        }
        if (distanceX <= stoppingDistance)
        {
            rigid2D.velocity = Vector2.zero;
        }
        IsFall();
    }
    private void IsFall()
    {
        int direction = Random.Range(0, 2);
        if (transform.position.y <= -6.0f)
        {
            Vector2 spawnPos = new Vector2(15, -3.7f);
            if (direction == 0)
                transform.position = new Vector3(target.transform.position.x + spawnPos.x, spawnPos.y, 0);
            else if (direction == 1)
                transform.position = new Vector3(target.transform.position.x - spawnPos.x, spawnPos.y, 0);
        }
    } 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            StartCoroutine(Hit());
            hitEffect.Flash(duration);
        }
    }
    IEnumerator Hit()
    {
        if (hpGauge <= 1)
        {
            Die();
        }
        else
        {
            hpGauge--;
        }
        yield return new WaitForSeconds(duration);
    }
    void Die()
    {
        gameDirector.GetComponent<GameDirector>().score += 50;
        enemyGenerator.GetComponent<EnemyGenerator>().deadEnemy += 1;
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
