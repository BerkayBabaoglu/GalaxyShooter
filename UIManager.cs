using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    //[SerializeField]
    //private Image _shieldStrengthImg;
    //[SerializeField]
    //private Sprite[] _shieldStrengthSprites;
    [SerializeField]
    private Text _gameOverText;
    //[SerializeField]
    //private Text _restartText;

    private GameManager _gameManager;


    void Start()
    {
        _scoreText.text = "Score: " + 0;

        /*if(_gameOverText != null)
        {
            _gameOverText.gameObject.SetActive(false);
        }
        */
        //_restartText.gameObject.SetActive(false);
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }


    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int curentLives)
    {
        _LivesImg.sprite = _liveSprites[curentLives];

        if (curentLives == 0)
        {
            GameOverSequence();
        }
    }
    /*
    public void UpdateShieldStrength(int shieldStrength)
    {
         Debug.Log(shieldStrength);
        _shieldStrengthImg.sprite = _shieldStrengthSprites[shieldStrength];
    }
    */
    
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        //_restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
