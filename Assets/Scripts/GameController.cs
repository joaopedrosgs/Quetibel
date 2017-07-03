using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public enum State
    {
        Playing,
        Menu,
        End
    };

    private static State _gameState;

    //Element numbers
    public static int Cats = 0;

    public static float Score;
    private float _timeLeft;
    public static int DeadCats;
    public static int SavedCats;


    //Text UI
    public static Text ScoreText;
    public static Text TimeText;

    //Positions
    public Transform Portal;
    public GameObject PlayerPrefab;
    public static GameObject Player;

    //Other UI
    public static GameObject Menu;
    public static GameObject[] NormalUi;
    public static GameObject EndScreen;

    //Statistics
    public GameObject StatPoint;
    public GameObject StatSaved;
    public GameObject StatDead;

    public static State GetGameState()
    {
        return _gameState;
    }

    public static bool OnMenu()
    {
        return _gameState == State.Menu;
    }

    void Awake()
    {
        Random.InitState((int) DateTime.Now.Ticks);
        _timeLeft = Random.Range(20, 40);

        //Player Spawn
        Vector3 position = Portal.transform.position;
        position.y = Terrain.activeTerrain.SampleHeight(position);
        Player = Instantiate(PlayerPrefab, position, Quaternion.identity);

        //Inicializando membros estáticos
        EndScreen = GameObject.Find("EndGameScreen");
        ScoreText = GameObject.Find("Score").GetComponent<Text>();
        Menu = GameObject.Find("Menu").gameObject;
        NormalUi = GameObject.FindGameObjectsWithTag("PlayerUI");
        TimeText = GameObject.Find("Tempo").GetComponent<Text>();
    }

    // Update is called once per frame
    void Start()
    {
        Menu.SetActive(false);
        EndScreen.SetActive(false);
    }

    void Update()
    {

        switch(_gameState)
        {
            case State.End:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SetGame();
                    SceneManager.LoadScene("StartMenu");
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetGame();
                }
            }
                break;
            case State.Playing:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                        EnterMenu();

                if (_timeLeft <= 0)
                {
                    EndGameReached();
                }
                _timeLeft -= Time.deltaTime;
                TimeText.text = _timeLeft.ToString("0") + " Segundos";
            }
                break;
            case State.Menu:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                        LeaveMenu();
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EndGameReached()
    {
        _gameState = State.End;
        EndScreen.SetActive(true);
        Menu.SetActive(false);
        foreach (GameObject ui in NormalUi)
            ui.SetActive(false);
        StatPoint.GetComponent<Text>().text = Score.ToString("0");
        StatDead.GetComponent<Text>().text = DeadCats.ToString();
        StatSaved.GetComponent<Text>().text = SavedCats.ToString();
        Time.timeScale = 0.02f;

        AudioListener.volume = 0.1f;
    }

    private void SetGame()
    {
        _gameState = State.Playing;
        EndScreen.SetActive(false);
        Menu.SetActive(false);
        foreach (GameObject ui in NormalUi)
            ui.SetActive(true);
        _timeLeft = Random.Range(20, 40);
        DeadCats = 0;
        SavedCats = 0;
        Score = 0;
        foreach (var cat in FindObjectsOfType<Cat>())
        {
            DestroyImmediate(cat.gameObject);
        }
        foreach (var skeleton in FindObjectsOfType<Skeleton>())
        {
            DestroyImmediate(skeleton.gameObject);
        }
        ScoreText.text = "0";
        Time.timeScale = 1;
        AudioListener.volume = 1;
        Player.transform.position = Portal.position;
    }

    public static void LeaveMenu()
    {
        Menu.SetActive(false);

        foreach (GameObject ui in NormalUi)
        {
            ui.SetActive(true);
        }
        _gameState = State.Playing;
        Time.timeScale = 1;
    }

    public static void EnterMenu()
    {
        foreach (GameObject ui in NormalUi)
        {
            ui.SetActive(false);
        }
        Menu.SetActive(true);
        _gameState = State.Menu;
        Time.timeScale = 0.05f;
    }

    public static IEnumerator IncrementScore(float quantity)
    {
        float newScore = Score + quantity;
        for (float timer = 0; timer < 0.5f; timer += Time.deltaTime)
        {
            float progress = timer / 0.5f;
            Score = (int) Mathf.Lerp(Score, newScore, progress);
            ScoreText.text = Score.ToString("0");
            yield return null;
        }
        ScoreText.text = newScore.ToString("0");
    }
}