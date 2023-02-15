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
    public GameObject MinecraftRule;
    public GameObject CrazyArcadeRule;
    public GameObject MapleStoryRule;
    [Header("Stage")]
    public bool Ending;
    int currentStage;
    List<int> StageNum = new List<int>();

    [Header("Sound")]
    public AudioSource SoundPlayer;
    public AudioClip MainSound;
    public AudioClip EndingSound;
    public AudioClip MapleSound;
    public AudioClip CrazyArcadeSound;
    public AudioClip MinecraftSound;
    public AudioClip FlirtingGameSound;
    public AudioClip RamyeonGameSound;

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

    private void Start()
    {
        SoundPlayer = GetComponent<AudioSource>();
        SoundPlayer.clip = MainSound;
        SoundPlayer.Play();
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

        if (Ending)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Game End");
            }
        }
    }
    public void StartButton() //메인화면 시작 버튼
    {
        RandomStageSelect();
        GoToNextStage(StageNum[0]);
    }

    public void RandomStageSelect()
    {
        int OrderOfStage = Random.Range(1, 6);
        Debug.Log(OrderOfStage);
        for (int i = 1; i < 6;)
        {
            if (StageNum.Contains(OrderOfStage))
            {
                Debug.Log("Stage Num already has " + OrderOfStage);
                OrderOfStage = Random.Range(1, 6);
            }
            else
            {
                StageNum.Add(OrderOfStage);
                Debug.Log(i +"Stage = " + OrderOfStage);
                i++;
            }
        }
        Debug.Log("Stage Set Done");
        Debug.Log("스테이지 순서" + StageNum[0] + " " + StageNum[1] + " " + StageNum[2] + " " + StageNum[3] + " " + StageNum[4] + " ");
    }

    public void GoToNextStage(int nextStageNum)
    {
        switch (nextStageNum)
        {
            case (1):
                SceneManager.LoadScene("Minecraft");
                MinecraftRule.SetActive(true);
                SoundPlayer.clip = MinecraftSound;
                SoundPlayer.Play();
                break;
            case (2):
                SceneManager.LoadScene("CrazyArcade");
                SoundPlayer.clip = CrazyArcadeSound;
                SoundPlayer.Play();
                break;
            case (3):
                SceneManager.LoadScene("Maplestory");
                SoundPlayer.clip = MapleSound;
                SoundPlayer.Play();
                break;
            case (4):
                SceneManager.LoadScene("FlirtingGame");
                SoundPlayer.clip = FlirtingGameSound;
                SoundPlayer.Play();
                break;
            case (5):
                SceneManager.LoadScene("RamyeonGame");
                SoundPlayer.clip = RamyeonGameSound;
                SoundPlayer.Play();
                break;
            default:
                break;
        }
        Debug.Log(SceneManager.GetActiveScene().name);
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

    public void RuleButton(GameObject RuleObject)
    {
        RuleObject.SetActive(false);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GotoMain() //메인으로
    {
        SetPause();
        SceneManager.LoadScene("MainMenu");
        SoundPlayer.clip = MainSound;
        SoundPlayer.Play();
        currentStage = 0;
    }

    public void GotoEnd()
    {
        SceneManager.LoadScene("Ending");
        SoundPlayer.clip = EndingSound;
        SoundPlayer.Play();
        Ending = true;
    }

    public IEnumerator Success() //스테이지 클리어 성공
    {
        Time.timeScale = 0f;
        SuccessImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        SuccessImage.SetActive(false);
        Time.timeScale = 1f;
        if (currentStage < 5)
        {
            GoToNextStage(StageNum[currentStage]);
        }
        else
        {
            GotoEnd();
        }
        
        
    }

    public IEnumerator Failure() //스테이지 클리어 실패
    {
        Time.timeScale = 0f;
        FailureImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        FailureImage.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
