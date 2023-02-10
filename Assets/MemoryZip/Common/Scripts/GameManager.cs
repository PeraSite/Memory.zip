using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if(instance == null)
                {
                    Debug.Log("No GameManager");
                }
                
            }
            return instance;
        }
        
    }

    [Header("UI")]
    public GameObject keyGuide;
    public bool isPause;
    public GameObject pause;
    public GameObject SuccessImage;
    public GameObject FailureImage;

    [Header("Stage")]
    int currentStage;
    List<int> StageNum = new List<int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if(currentStage != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SetPause();
        }
        
    }
    public void StartButton() //메인화면 시작 버튼
    {
        RandomStageSelect();
        GoToNextStage(StageNum[0]);
    }

    public void RandomStageSelect()
    {
        int OrderOfStage = Random.Range(1, 7);
        Debug.Log(OrderOfStage);
        for (int i = 0; i < 5;)
        {
            if (StageNum.Contains(OrderOfStage))
            {
                Debug.Log("Stage Num already has " + OrderOfStage);
                OrderOfStage = Random.Range(1, 7);
            }
            else
            {
                StageNum.Add(OrderOfStage);
                Debug.Log(i +"Stage = " + OrderOfStage);
                i++;
            }
        }
        Debug.Log("Stage Set Done");
    }

    public void GoToNextStage(int nextStageNum)
    {
        switch (nextStageNum)
        {
            case (1):
                SceneManager.LoadScene("Minecraft");
                break;
            case (2):
                SceneManager.LoadScene("CrazyArcade");
                break;
            case (3):
                SceneManager.LoadScene("Maplestory");
                break;
            case (4):
                SceneManager.LoadScene("FlirtingGame");
                break;
            case (5):
                SceneManager.LoadScene("RamyeonGame");
                break;
            case (6):
                SceneManager.LoadScene("TalesRunner");
                break;
            default:
                break;
        }
        currentStage++;
    }

    public void KeyGuideButton() //메인화면 가이드 버튼
    {
        if (keyGuide.activeSelf == true)
        {
            keyGuide.SetActive(false);
        }
        else
        {
            keyGuide.SetActive(true);
        }
    }

    public void SetPause() //ESC눌렀을때
    {
        if (!isPause)
        {
            pause.SetActive(true);
            isPause = true;
            Time.timeScale = 0;
        }
        else
        {
            pause.SetActive(false);
            isPause = false;
            Time.timeScale = 1;
        }
        
    }

    public void Resume() //계속하기
    {
        SetPause();
    }
    public void Restart() //해당 스테이지 재시작
    {
        SetPause();
    }

    public void GotoMain() //메인으로
    {
        SetPause();
        SceneManager.LoadScene("MainMenu");
        currentStage = 0;
    }

    public void Success() //스테이지 클리어 성공
    {
        SuccessImage.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SuccessImage.SetActive(false);
            GoToNextStage(currentStage);
        }
            
    }

    public void Failure() //스테이지 클리어 실패
    {
        FailureImage.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FailureImage.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
            
    }
}
