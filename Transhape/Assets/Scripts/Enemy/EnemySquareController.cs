using System.Collections;
using UnityEngine;

public class EnemySquareController : MonoBehaviour
{
    private int speed = 300;
    private bool isMoving = false;
    private GameObject target;
    private GameObject gameDirector;
    private GameObject enemyGenerator;
    private int hpGauge = 15;
    private float duration = 0.125f;
    [SerializeField] private HitEffect hitEffect;
    void Start()
    {
        gameDirector = GameObject.Find("GameDirector");
        enemyGenerator = GameObject.Find("EnemyGenerator");
        target = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        while (true)
        {
            if (transform.position.x < target.transform.position.x && !isMoving) // 오
            {
                StartCoroutine(Roll(-Vector3.forward));
            }
            else if (transform.position.x > target.transform.position.x && !isMoving) // 왼
            {
                StartCoroutine(Roll(Vector3.forward));
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;

        float remainingAngle = 90;
        Vector3 rotationCenter;
        if (direction == Vector3.forward) // 왼
        {
            rotationCenter = transform.position + Vector3.left / 2 + Vector3.down / 2; // 중심점을 왼쪽으로 이동
        }
        else // 오
        {
            rotationCenter = transform.position + Vector3.right / 2 + Vector3.down / 2; // 중심점을 오른쪽으로 이동
        }
        Vector3 rotationAxis = direction; // 회전할 때 사용하는 축 변경
        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        isMoving = false;
    }
    void Update()
    {
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
        gameDirector.GetComponent<GameDirector>().score += 100;
        enemyGenerator.GetComponent<EnemyGenerator>().deadEnemy += 1;
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
