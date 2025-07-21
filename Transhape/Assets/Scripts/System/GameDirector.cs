using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameDirector : MonoBehaviour
{
    [Header("PlayerInform UI")]
    [SerializeField] private TextMeshProUGUI scoreCountText;
    [SerializeField] private Slider coinCountSlider;
    [SerializeField] private TextMeshProUGUI hpCountText;
    [HideInInspector] public int score;
    [HideInInspector] public int coinCount;
    [HideInInspector] public int hpCount;

    [Header("GameOver UI")]
    [SerializeField] private GameObject gameOverPopUp;
    [SerializeField] private TextMeshProUGUI endScoreText;

    [SerializeField] private AnimationCurve moveOverTabCurve = AnimationCurve.Linear(0, 0, 1, 1);
    private float startOverTabTime = 0.7f;
    // 시작 위치와 목표 위치
    private Vector2 overTabStartPosition;
    private Vector2 overTabTargetPosition;
    private bool over = false;

    [Header("TabKey")]
    // 이동할 UI 이미지
    [SerializeField] private Image tutorialTab;
    [SerializeField] private AnimationCurve moveTabCurve = AnimationCurve.Linear(0, 0, 1, 1);
    private float startTabTime = 0.7f;
    private float endTabTime = 0.7f;
    // 시작 위치와 목표 위치
    private Vector2 tabStartPosition;
    private Vector2 tabTargetPosition;


    [Header("ESCkey")]
    [SerializeField] private GameObject escapePopUp;

    [Header("Ask")]
    [SerializeField] private GameObject exitPopUp;
    [SerializeField] private GameObject titlePopUp;

    [HideInInspector] public bool isOver = false;
    private bool isTab = false;
    [HideInInspector] public bool isEscape = false;
    void Start()
    {
        // UI 이미지의 RectTransform 컴포넌트 가져오기
        RectTransform tutorialRectTransform = tutorialTab.GetComponent<RectTransform>();
        RectTransform overRectTransform = tutorialTab.GetComponent<RectTransform>();

        tabStartPosition = new Vector2 (-263.65f, -275);
        tabTargetPosition = new Vector2(325f, -275);

        overTabStartPosition = new Vector2(0f, 1154f);
        overTabTargetPosition = new Vector2(0f, 0f);
        // 시작 위치로 이동
        tutorialRectTransform.anchoredPosition = tabStartPosition;
        overRectTransform.anchoredPosition = overTabStartPosition;

        score = 0;
        coinCount = 0;
        hpCount = 10;
    }
    void Update()
    {
        Pause();
        PlayerInformation();
        GameOver();
        TabUI();
        ESCUI();
    }
    private void Pause()
    {
        if (exitPopUp.activeInHierarchy == true || titlePopUp.activeInHierarchy == true || isTab == true || isEscape == true || isOver == true)
        {
            Time.timeScale = 0; // 게임 정지
        }
        else
        {
            Time.timeScale = 1; // 게임 재개
        }
    }
    private void PlayerInformation()
    {
        scoreCountText.SetText("Score: " + score);
        coinCountSlider.value = coinCount;
        hpCountText.SetText(" / " + hpCount);

        if (coinCount >= 50)
        {
            coinCount -= 50;
            hpCount += 1;
        }
    }
    private void GameOver()
    {
        
        if (isOver)
        {
            endScoreText.SetText("Score: " + score);
            StartCoroutine(MoveToGameOverTab());
            
        }
    }
    IEnumerator MoveToGameOverTab()
    {
        float elapsedTime = 0f;
        if (!over)
        {
            while (elapsedTime < startOverTabTime)
            {
                // 보간된 값 계산
                float t = elapsedTime / startOverTabTime;
                float curveT = moveOverTabCurve.Evaluate(t);
                Vector2 newPosition = Vector2.Lerp(overTabStartPosition, overTabTargetPosition, curveT);

                // UI 이미지 이동
                gameOverPopUp.GetComponent<RectTransform>().anchoredPosition = newPosition;

                // 경과 시간 업데이트
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            // 이동이 끝나면 최종 위치로 설정
            gameOverPopUp.GetComponent<RectTransform>().anchoredPosition = overTabTargetPosition;
            over = true;
        }
    }
    private void TabUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isTab)
            {
                StartCoroutine(MoveToTutorialTab());
                isTab = true;
            }
            else
            {
                StartCoroutine(MoveToTutorialTab());
                isTab = false;
            }
        }
    }
    IEnumerator MoveToTutorialTab()
    {
        float elapsedTime = 0f;
        if (!isTab) 
        {
            while (elapsedTime < startTabTime)
            {
                // 보간된 값 계산
                float t = elapsedTime / startTabTime;
                float curveT = moveTabCurve.Evaluate(t);
                Vector2 newPosition = Vector2.Lerp(tabStartPosition, tabTargetPosition, curveT);

                // UI 이미지 이동
                tutorialTab.GetComponent<RectTransform>().anchoredPosition = newPosition;

                // 경과 시간 업데이트
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }

            // 이동이 끝나면 최종 위치로 설정
            tutorialTab.GetComponent<RectTransform>().anchoredPosition = tabTargetPosition;

        }
        else
        {
            while (elapsedTime < endTabTime)
            {

                // 보간된 값 계산
                float t = elapsedTime / endTabTime;
                float curveT = moveTabCurve.Evaluate(t);
                Vector2 newPosition = Vector2.Lerp(tabTargetPosition, tabStartPosition, curveT);

                // UI 이미지 이동
                tutorialTab.GetComponent<RectTransform>().anchoredPosition = newPosition;

                // 경과 시간 업데이트
                elapsedTime += Time.unscaledDeltaTime;

                yield return null;
            }
            // 이동이 끝나면 최종 위치로 설정
            tutorialTab.GetComponent<RectTransform>().anchoredPosition = tabStartPosition;
        }
    }

    private void ESCUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isEscape)
            {
                isEscape = true;
                escapePopUp.SetActive(isEscape);
            }
            else
            {
                isEscape = false;
                escapePopUp.SetActive(isEscape);
            }

        }
    }
}
