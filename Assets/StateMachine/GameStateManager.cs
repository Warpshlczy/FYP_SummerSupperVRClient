using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // 引入新的 Input System 命名空间

public class GameStateManager : MonoBehaviour
{
    public GameState currentGameState;
    public static GameStateManager instance;

    public GameObject fadeOut;

    public int numOnions = 1;
    public int numTomato = 1;

    public enum GameState
    {
        StartingState,
        BeginOnionChop,
        BeginTomatoChopBook,
        BeginTomatoChop,
        BeginBowlStep,
        BeginBrownPorkBook,
        BeginBrownPork,
        BeginSizzlePork,
        BeginWashingBokChoyBook,
        BeginWashingBokChoy,
        EndingState
    }

    private void Start()
    {
        instance = this;
        ChangeGameState(GameState.StartingState);
        fadeOut.SetActive(true);
    }

    private void Update()
    {
        // 使用新的 Input System 检测按键
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Home Kitchen Original");
        }
    }

    public void ChangeGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.StartingState:
                Debug.Log("StartingState reached!");
                BroadcastMessage("StartingState");
                break;
            case GameState.BeginOnionChop:
                Debug.Log("BeginOnionChop reached!");
                BroadcastMessage("BeginOnionChop");
                break;
            case GameState.BeginTomatoChopBook:
                Debug.Log("BeginTomatoChopBook reached!");
                BroadcastMessage("BeginTomatoChopBook");
                break;
            case GameState.BeginTomatoChop:
                Debug.Log("BeginTomatoChop reached");
                BroadcastMessage("BeginTomatoChop");
                break;
            case GameState.BeginBowlStep:
                Debug.Log("BeginBowlStep reached!");
                BroadcastMessage("BeginBowlStep");
                break;
            case GameState.BeginBrownPorkBook:
                Debug.Log("BeginBrownPorkBook reached!");
                BroadcastMessage("BeginBrownPorkBook");
                break;
            case GameState.BeginBrownPork:
                Debug.Log("BeginBrownPork reached!");
                BroadcastMessage("BeginBrownPork");
                break;
            case GameState.BeginSizzlePork:
                Debug.Log("BeginSizzlePork reached.");
                BroadcastMessage("BeginSizzlePork");
                break;
            case GameState.BeginWashingBokChoyBook:
                Debug.Log("BeginWashingBokChoyBook");
                BroadcastMessage("BeginWashingBokChoyBook");
                break;
            case GameState.BeginWashingBokChoy:
                Debug.Log("BeginWashingBokChoy reached!");
                BroadcastMessage("BeginWashingBokChoy");
                break;
            case GameState.EndingState:
                Debug.Log("EndingState reached! Everything should be working");
                fadeOut.GetComponent<FadeInFadeOut>().StartFadeToBlack();
                StartCoroutine(SwitchScenes());
                break;
        }
    }

    private IEnumerator SwitchScenes()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Dorm P1");
    }
}
