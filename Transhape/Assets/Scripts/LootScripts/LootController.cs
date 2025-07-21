using System.Collections;
using UnityEngine;

public class LootController : MonoBehaviour
{
    private Rigidbody2D lootRigidbody2D;
    private Renderer lootRenderer;
    private GameObject gameDirector;
    private float speed = 5f; // �ʱ� �ӵ�
    private float upwardAngle = 30f; // �������� �߻��� ����
    private float minHorizontalAngle = 45f; // �������� �߻��� �ּ� ����
    private float maxHorizontalAngle = 135f; // ���������� �߻��� �ִ� ����

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector");
        lootRigidbody2D = GetComponent<Rigidbody2D>();
        lootRenderer = GetComponent<Renderer>();
        LootSpreading();
        StartCoroutine(StartDisappearing());
    }
    private void LootSpreading()
    {
        float angle;
        int direction;

        // ������ �� ����
        float randomValue = Random.value;

        // ���� �߻�
        if (randomValue == 0)
        {
            angle = upwardAngle;
            direction = 0; // �������θ� �߻��ϹǷ� ������ �ǹ� ����
        }
        // ���������� �߻�
        else if (randomValue >= 1f)
        {
            angle = Random.Range(minHorizontalAngle, maxHorizontalAngle);
            direction = 1;
        }
        // �������� �߻�
        else
        {
            angle = Random.Range(minHorizontalAngle, maxHorizontalAngle);
            direction = -1;
        }

        // ������ �������� ��ȯ
        float radians = angle * Mathf.Deg2Rad;
        // �ʱ� �ӵ��� x�� y�� �������� ����
        float vx = speed * Mathf.Cos(radians) * direction;
        float vy = speed * Mathf.Sin(radians);
        // ������ ��� ���� ���� ����
        lootRigidbody2D.velocity = new Vector2(vx, vy);

    }
    IEnumerator StartDisappearing()
    {
        int currentTime = 17;
        while (currentTime > 0)
        {
            if (currentTime == 3)
            {
                CancelInvoke("Blink");
                InvokeRepeating("Blink", 0f, 0.125f);
            }
            yield return new WaitForSeconds(1.0f);
            --currentTime;
        }
        Destroy(gameObject);
    }
    private void Blink()
    {
        if (lootRenderer.material.color.a == 0)
            lootRenderer.material.color = new Color(1f, 1f, 1f, 1f);
        else
            lootRenderer.material.color = new Color(1f, 1f, 1f, 0f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            lootRigidbody2D.velocity = Vector3.zero;
            lootRigidbody2D.constraints |= RigidbodyConstraints2D.FreezePosition;
        }
        if (collision.CompareTag("Player"))
        {
            LootType();
            Destroy(gameObject);
        }
    }
    public void LootType()
    {
        if (gameObject.CompareTag("Coin"))
        {
            gameDirector.GetComponent<GameDirector>().score += 10;
            gameDirector.GetComponent<GameDirector>().coinCount += 1;
        }
        if (gameObject.CompareTag("Heart"))
        {
            gameDirector.GetComponent<GameDirector>().score += 20;
            gameDirector.GetComponent<GameDirector>().hpCount += 1;
        }
    }

}
