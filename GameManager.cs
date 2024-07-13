using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isCoopMode = false;
    public bool gameOver = true;
    public GameObject player;

    private UIManager _uiManager;
   
    [SerializeField]
    private bool _isGameOver;


    private void Start()
    {
        _uiManager = GameObject.Find("Spawn_Manager").GetComponent<UIManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); // Current Game Scene
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void GameOver()
    {
        Debug.Log("GameManager::GameOver() Called");
        _isGameOver = true;
    }
}
