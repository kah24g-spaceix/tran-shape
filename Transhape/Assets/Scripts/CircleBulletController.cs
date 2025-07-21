using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CircleBulletController : MonoBehaviour
{
    float destroyTime = 5.0f;
    private GameObject player;
    private CircleCollider2D circleCollider2D;
    void Start()
    {

        circleCollider2D = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, destroyTime);
        circleCollider2D.enabled = false;
        StartCoroutine(ColliderRoutine());
    }

    void Update()
    {
        transform.position = player.transform.position;
    }
    IEnumerator ColliderRoutine()
    {
        while (true)
        {
            circleCollider2D.enabled = true;
            yield return new WaitForSeconds(0.155f);
            circleCollider2D.enabled = false;
        }

    }
}
