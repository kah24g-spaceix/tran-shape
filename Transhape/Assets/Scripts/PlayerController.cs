using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [Header("Camera:")]
    [SerializeField] private GameObject cam;

    [Header("Skill")]
    [SerializeField] private Image cooldownSquareImage; // Filled Ÿ���� �̹���
    [SerializeField] private Image cooldownCircleImage;
    private int squareSpeed = 300;
    private float circleSpeed = 10f;
    private float jumpForce = 7f; // ���� ��

    private BulletGenerator bullet;
    private GameObject gameDirector;
    private HitEffect hitEffect;

    private Rigidbody2D rigid2D;
    private BoxCollider2D boxCollider2D;
    private CircleCollider2D circleCollider2D;
    private Animator animator;

    private float maxWalkSpeed = 5f;

    private float cooldownTime_Square = 0.125f; // ��ų ��Ÿ��(��)
    private float currentTime_Square = 0f; // ���� ��Ÿ��
    private float cooldownTime_Circle = 6.5f; // ��ų ��Ÿ��(��)
    private float currentTime_Circle = 0f; // ���� ��Ÿ��

    private float duration = 0.225f;

    private bool isAttack = false;
    private bool isChange = false;
    private bool isGrounded = false;
    private bool isMoving = false;
    private bool isCooldownSquare = false;
    private bool isCooldownCircle = false;
    private bool isDeath = false;

    void Start()
    {
        gameDirector = GameObject.Find("GameDirector");
        rigid2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        bullet = GetComponent<BulletGenerator>();
        hitEffect = GetComponent<HitEffect>();

        cooldownSquareImage.enabled = true;
        cooldownCircleImage.enabled = false;
    }
    void Update()
    {
        Movement();
        Jump();
        ShapeChange();
        CoolDownSquare();
        CoolDownCircle();
    }
    void LateUpdate()
    {
        cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z);
    }
    private void Movement()
    {
        float speedx = Mathf.Abs(rigid2D.velocity.x);
        float nomalSpeed = circleSpeed * Time.timeScale;
        if (Input.GetKey(KeyCode.RightArrow) && !isMoving)
        {
            if (!isChange)
            {
                if (isGrounded)
                {
                    StartCoroutine(Roll(-Vector3.forward)); // ���������� ȸ���ϵ��� �����ϰ� ���� ���͸� ����
                }
                else
                {
                    Vector2 movement = new Vector2(1, 0);
                    if (speedx < maxWalkSpeed / 2)
                        rigid2D.AddForce(movement * nomalSpeed);
                }
            }
            else
            {
                Vector2 movement = new Vector2(1, 0);
                if (speedx < maxWalkSpeed)
                    rigid2D.AddForce(movement * nomalSpeed);
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) && isChange)
        {

            rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !isMoving)
        {
            if (!isChange)
            {
                if (isGrounded)
                {
                    StartCoroutine(Roll(Vector3.forward)); // ���������� ȸ���ϵ��� �����ϰ� ���� ���͸� ����
                }
                else
                {
                    Vector2 movement = new Vector2(-1, 0);
                    if (speedx < maxWalkSpeed / 2)
                        rigid2D.AddForce(movement * nomalSpeed);
                }
            }
            else
            {
                Vector2 movement = new Vector2(-1, 0);
                if (speedx < maxWalkSpeed)
                    rigid2D.AddForce(movement * nomalSpeed);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) && (isChange || (!isGrounded && !isChange)))
        {
            rigid2D.velocity = new Vector2(0, rigid2D.velocity.y);
        }

    }
    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;

        float remainingAngle = 90;
        Vector3 rotationCenter;
        if (direction == Vector3.forward) // ���� Ű�� ������ ��
        {
            rotationCenter = transform.position + Vector3.left / 2 + Vector3.down / 2; // �߽����� �������� �̵�
        }
        else // ������ Ű�� ������ ��
        {
            rotationCenter = transform.position + Vector3.right / 2 + Vector3.down / 2; // �߽����� ���������� �̵�
        }
        Vector3 rotationAxis = direction; // ȸ���� �� ����ϴ� �� ����
        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * squareSpeed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        isMoving = false;
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
            isGrounded = false;
        }
        else if ((Input.GetKeyDown(KeyCode.Space) && !isGrounded))
        {
            ActivateSkill();
        }

    }
    private void ShapeChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isChange)
            {
                isChange = true;
                animator.SetBool("isChange", isChange);

                circleCollider2D.enabled = isChange;
                boxCollider2D.enabled = !isChange;

                cooldownSquareImage.enabled = !isChange;
                cooldownCircleImage.enabled = isChange;

                rigid2D.constraints &= ~RigidbodyConstraints2D.FreezeRotation;

            }
            else
            {
                isChange = false;
                animator.SetBool("isChange", isChange);

                circleCollider2D.enabled = isChange;
                boxCollider2D.enabled = !isChange;

                cooldownSquareImage.enabled = !isChange;
                cooldownCircleImage.enabled = isChange;

                transform.rotation = Quaternion.Euler(0, 0, 0);
                rigid2D.constraints |= RigidbodyConstraints2D.FreezeRotation;

            }

        }
    }
    public void ActivateSkill()
    {
        isAttack = true;

        if (!isChange)
        {
            if (!isCooldownSquare)
            {
                rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0);
                bullet.AttackSquare();
                cooldownSquareImage.fillAmount = 0f;
                currentTime_Square = cooldownTime_Square;
                animator.SetBool("isAttack", isAttack);
                isCooldownSquare = true;
            }

        }
        else
        {
            if (!isCooldownCircle)
            {
                rigid2D.velocity = new Vector2(rigid2D.velocity.x, 0);
                bullet.AttackCircle();
                cooldownCircleImage.fillAmount = 0f;
                currentTime_Circle = cooldownTime_Circle;
                animator.SetBool("isAttack", isAttack);
                isCooldownCircle = true;
            }

        }
    }
    private void CoolDownSquare()
    {
        if (isCooldownSquare)
        {
            currentTime_Square -= Time.deltaTime;
            cooldownSquareImage.fillAmount = 1 - (currentTime_Square / cooldownTime_Square);
            if (currentTime_Square <= 0f)
            {
                isAttack = false;
                isCooldownSquare = false;
                cooldownSquareImage.fillAmount = 1f;

            }
        }
    }
    private void CoolDownCircle()
    {
        if (isCooldownCircle)
        {
            currentTime_Circle -= Time.deltaTime;
            cooldownCircleImage.fillAmount = 1 - (currentTime_Circle / cooldownTime_Circle);
            if (currentTime_Circle <= 0f)
            {
                isAttack = false;
                isCooldownCircle = false;
                cooldownCircleImage.fillAmount = 1f;

            }
        }
    }

    // ���� ��Ҵ��� Ȯ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemy"))
        {
            animator.SetBool("isAttack", false);
            isGrounded = true;

        }
        if (collision.collider.CompareTag("Enemy"))
        {
            StartCoroutine(Hit());
            hitEffect.Flash(duration);
        }
    }
    IEnumerator Hit()
    {
        if (gameDirector.GetComponent<GameDirector>().hpCount == 1)
        {
            gameDirector.GetComponent<GameDirector>().hpCount -= 1;
            Die();
        }
        else
        {
            if(gameDirector.GetComponent<GameDirector>().hpCount >= 1)
                gameDirector.GetComponent<GameDirector>().hpCount -= 1;
        }
        yield return new WaitForSeconds(duration);
    }
    private void Die()
    {
        isDeath = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rigid2D.constraints |= RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("isDeath", isDeath);

        Invoke("GameOver", 1f);
    }
    private void GameOver()
    {
        gameDirector.GetComponent<GameDirector>().isOver = true;
    }
}
