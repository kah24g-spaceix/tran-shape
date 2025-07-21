using System.Collections;
using UnityEngine;

public class LootController : MonoBehaviour
{
    private Rigidbody2D lootRigidbody2D;
    private Renderer lootRenderer;
    private GameObject gameDirector;
    private float speed = 5f; // 초기 속도
    private float upwardAngle = 30f; // 위쪽으로 발사할 각도
    private float minHorizontalAngle = 45f; // 왼쪽으로 발사할 최소 각도
    private float maxHorizontalAngle = 135f; // 오른쪽으로 발사할 최대 각도

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

        // 랜덤한 값 생성
        float randomValue = Random.value;

        // 위로 발사
        if (randomValue == 0)
        {
            angle = upwardAngle;
            direction = 0; // 위쪽으로만 발사하므로 방향은 의미 없음
        }
        // 오른쪽으로 발사
        else if (randomValue >= 1f)
        {
            angle = Random.Range(minHorizontalAngle, maxHorizontalAngle);
            direction = 1;
        }
        // 왼쪽으로 발사
        else
        {
            angle = Random.Range(minHorizontalAngle, maxHorizontalAngle);
            direction = -1;
        }

        // 각도를 라디안으로 변환
        float radians = angle * Mathf.Deg2Rad;
        // 초기 속도를 x와 y의 성분으로 분해
        float vx = speed * Mathf.Cos(radians) * direction;
        float vy = speed * Mathf.Sin(radians);
        // 포물선 운동을 위해 힘을 가함
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
