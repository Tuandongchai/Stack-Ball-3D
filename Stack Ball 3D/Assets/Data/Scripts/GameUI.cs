using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private static GameUI instance;
    public static GameUI Instance { get => instance; }

    private bool inGame;
    public bool InGame { get => inGame; }

    [SerializeField] protected GameObject homeUI, inGameUI, finishUI, gameOverUI;
    [SerializeField] protected GameObject allbuttons;

    private bool buttons;

    [Header("PreGame")]
    [SerializeField] protected Button soundButton;
    [SerializeField] protected Sprite soundOnS, soundOffS;

    [Header("InGame")]
    [SerializeField] protected Image levelSlider;
    [SerializeField] protected Image currentLevelImg;
    [SerializeField] protected Image nextLevelImg;
    [SerializeField] protected Text currentLevelText, nextLevelText;

    [Header("Finish")]
    [SerializeField] protected Text finishLevelText;

    [Header("GameOver")]
    [SerializeField] protected Text gameOverScoreText;
    [SerializeField] protected Text gameOverBestText;

    private Material ballMat;
    /*private Ball ball;*/

    private void Awake()
    {
        GameUI.instance = this;

        ballMat = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;
        /*ball = FindObjectOfType<Ball>();*/

        levelSlider.transform.parent.GetComponent<Image>().color = ballMat.color + Color.gray;
        levelSlider.color = ballMat.color;
        currentLevelImg.color = ballMat.color;
        nextLevelImg.color = ballMat.color;

        soundButton.onClick.AddListener(() => SoundManager.Instance.SoundOnOff());
    }

    private void Start()
    {
        currentLevelText.text = FindObjectOfType<LevelSpawner>().Level.ToString();
        nextLevelText.text = FindObjectOfType<LevelSpawner>().Level+1+"";
    }
    private void Update()
    {
        if(Ball.Instance.BallStateCurrent == Ball.BallState.Prepare)
        {
            if(SoundManager.Instance.Sound && soundButton.GetComponent<Image>().sprite != soundOnS)
            {
                soundButton.GetComponent<Image>().sprite = soundOnS;
            }
            else if(!SoundManager.Instance.Sound && soundButton.GetComponent <Image>().sprite != soundOffS) 
            {
                soundButton.GetComponent<Image>().sprite=soundOffS;
            }
        }
        if(InputManager.Instance.MouseButtonDown && !IgnoreUI() && Ball.Instance.BallStateCurrent == Ball.BallState.Prepare)
        {
            /*Ball.Instance.BallStateCurrent = Ball.BallState.Playing;*/

            homeUI.SetActive(false);
            inGameUI.SetActive(true);
            finishUI.SetActive(false);
            gameOverUI.SetActive(false);
            
        }
        if(Ball.Instance.BallStateCurrent == Ball.BallState.Finish)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(true);
            gameOverUI.SetActive(false);

            finishLevelText.text = "Level" + FindObjectOfType<LevelSpawner>().Level;
        }
        if(Ball.Instance.BallStateCurrent == Ball.BallState.Died)
        {
            homeUI.SetActive(false);
            inGameUI.SetActive(false);
            finishUI.SetActive(false);
            gameOverUI.SetActive(true);

            gameOverScoreText.text = ScoreManager.Instance.Score.ToString();
            gameOverBestText.text=PlayerPrefs.GetInt("HighScore").ToString();

            if (InputManager.Instance.MouseButtonDown)
            {
                ScoreManager.Instance.ResetScore();
                SceneManager.LoadScene(0);
            }
        }
        if (inGameUI.activeSelf)
        {
            this.inGame = true;
            
        }
        else
        {
            this.inGame= false;
        }

        
    }
    protected virtual bool IgnoreUI()
    {
        PointerEventData pointerEvenData = new PointerEventData(EventSystem.current);
        pointerEvenData.position = Input.mousePosition;

        List<RaycastResult> raycasResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEvenData, raycasResultList);
        for(int i =0; i<raycasResultList.Count; i++)
        {
            if (raycasResultList[i].gameObject.GetComponent<Ignore>() != null)
            {
                raycasResultList.RemoveAt(i);
                i--;
            }
        }
        return raycasResultList.Count >0;
    }
    public virtual void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    protected virtual void Settings()
    {
        buttons = !buttons;
        allbuttons.SetActive(buttons);
    }
}
