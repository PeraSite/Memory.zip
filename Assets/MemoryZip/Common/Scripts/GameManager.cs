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
    public GameObject EndImage;

    [Header("Stage")]
    public bool Ending;
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
        
        if (currentStage != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SetPause();
            if (Input.GetKeyDown(KeyCode.C))
            {
                StartCoroutine(Success());
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(Failure());
            }
        }
        else
        {
            if (Ending)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Application.Quit();
                }
            }
        }
    }
    public void StartButton() //����ȭ�� ���� ��ư
    {
        RandomStageSelect();
        GoToNextStage(StageNum[0]);
    }

    public void RandomStageSelect()
    {
        int OrderOfStage = Random.Range(1, 6);
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
            default:
                SceneManager.LoadScene("MainMenu");
                EndImage.SetActive(true);
                Ending = true;
                break;
        }
        Debug.Log(SceneManager.GetActiveScene().name);
        currentStage++;
    }

    public void KeyGuideButton() //����ȭ�� ���̵� ��ư
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

    public void SetPause() //ESC��������
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

    public void Resume() //����ϱ�
    {
        SetPause();
    }
    public void Restart() //�ش� �������� �����
    {
        SetPause();
    }

    public void GotoMain() //��������
    {
        SetPause();
        SceneManager.LoadScene("MainMenu");
        currentStage = 0;
    }

    public void End()
    {

    }

    public IEnumerator Success() //�������� Ŭ���� ����
    {
        SuccessImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        SuccessImage.SetActive(false);
        GoToNextStage(currentStage);
    }

    public IEnumerator Failure() //�������� Ŭ���� ����
    {
        FailureImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        FailureImage.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
