using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  private bool _isGameOver;
  [SerializeField] private GameObject pauseMenu;
  private Animator _pauseMenuAnimator;
  private Vector3 _pos;

  private void Start()
  {
    _pauseMenuAnimator = pauseMenu.GetComponent<Animator>();
    _pauseMenuAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    _pos = pauseMenu.transform.position;
  }

  public void GameOver()
  {
    _isGameOver = true;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
    {
      SceneManager.LoadScene(1); // CURRENT GAME SCENE
    }

    if (Input.GetKey("escape"))
    {
      Application.Quit();
    }

    if (Input.GetKeyDown(KeyCode.P))
    {
      pauseMenu.SetActive(true);
      _pauseMenuAnimator.SetBool("isPaused", true);
      Time.timeScale = 0;
    }
  }
  
  public void ResumePlay()
  {
    pauseMenu.SetActive(false);
    pauseMenu.transform.position = new Vector3(_pos.x, 512, _pos.z);
    Time.timeScale = 1;
  }
}
