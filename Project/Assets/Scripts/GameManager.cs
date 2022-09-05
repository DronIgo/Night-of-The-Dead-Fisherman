using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// GameManager contains information and logic behind current difficulty level, gamestate and score
/// </summary>
public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int numberOfDice = 15;
    public float timeBetweenTasks = 9;
    public float defTimeBetweenTasks = 9;
    public bool hintActive = false;

    public GameObject hintUI = null;

    public AudioSource gameOver = null;

    private float timer = 4f;

    //Whats the amount of task before each difficulty transition?
    public int diffciultyTransition1;
    public int diffciultyTransition2;
    public int diffciultyTransition3;
    public int diffciultyTransition4;
    public int diffciultyTransition5;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        BowlManager.instance.GenerateDice(numberOfDice);
    }

    int numOfTasks = 0;
    int difficulty = 0;
    public bool gameActive = true;
    HintQuad lastHint = null;
    void Update()
    {
        if (gameActive)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenTasks)
            {
                timer = 0;
                numOfTasks++;
                ProcessDifficulty();
                TaskManager.instance.CreateNewTask(difficulty);
            }
            
        }
        if (Input.GetKey(KeyCode.H))
        {
            hintActive = true;
            hintUI.SetActive(true);
            RaycastHit hit = CastRay(LayerMask.GetMask("Condition"));
            if (hit.collider != null)
            {
                lastHint = hit.collider.gameObject.GetComponent<HintQuad>();
                lastHint.Activate();
            }
            else
            {
                if (lastHint != null)
                {
                    lastHint.DeActivate();
                    lastHint = null;
                }
                if (hintUI.transform.Find("HintUI").gameObject.TryGetComponent<TMP_Text>(out TMP_Text t))
                {
                    t.text = "";
                }
            }
        }
        else
        {
            hintActive = false;
            hintUI.SetActive(false);
        }
    }

    int difficultyLevel = 0;
    int count = 0;
    void ProcessDifficulty()
    {
        if (numOfTasks > diffciultyTransition1)
            difficultyLevel = 1;
        if (numOfTasks > diffciultyTransition2)
            difficultyLevel = 2;
        if (numOfTasks > diffciultyTransition3)
            difficultyLevel = 3;

        switch (difficultyLevel) // lots of magic numbers needs refactoring
        {
            case 0:
                difficulty = 0;
                timeBetweenTasks = defTimeBetweenTasks;
                break;
            case 1:
                count += Random.Range(0, 15);
                if (count > 10)
                {
                    difficulty = 1;
                    count = 0;
                } else
                {
                    difficulty = 0;
                    count -= 5;
                }
                break;
            case 2:
                count += Random.Range(0, 25);
                if (count > 20)
                {
                    difficulty = 2;
                    count = 0;
                } else if (count > 15) {
                    difficulty = 1;
                    count -= 5;
                } else {
                    difficulty = 0;
                }
                break;
            case 3:
                count += Random.Range(0, 30);
                if (count > 20)
                {
                    difficulty = 3;
                    count = 0;
                }
                else if (count > 15)
                {
                    difficulty = 2;
                    count -= 5;
                }
                else if (count > 5)
                {
                    difficulty = 1;
                } else
                {
                    difficulty = 0;
                }
                if (difficulty < 3)
                    timeBetweenTasks = defTimeBetweenTasks - 4 + difficulty;
                break;
            case 4:
                count += Random.Range(0, 40);
                if (count > 20)
                {
                    difficulty = 3;
                    count -= 25;
                }
                else if (count > 15)
                {
                    difficulty = 2;
                    count -= 5;
                }
                else if (count > 5)
                {
                    difficulty = 1;
                }
                else
                {
                    difficulty = 0;
                }
                timeBetweenTasks = defTimeBetweenTasks - 5 + difficulty;
                break;
        }
    }

    public int score = 0;
    public void IncrementScore(int s)
    {
        score += s;
    }

    public void LoadLevel(string LevelName)
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName(LevelName).buildIndex);
    }

    public GameObject InGameUI = null;
    public GameObject GameOverUI = null;
    public GameObject DialogUI = null;
    public void GameOver()
    {
        gameActive = false;
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
        GameOverUI.transform.Find("Final Score").GetComponent<TextMeshProUGUI>().text = 
            "You managed to feed " + score + " dead sailors";
        gameOver.Play();
    }

    private RaycastHit CastRay(LayerMask mask)
    {
        //Vectors describe the position relative to the camera.
        Vector3 screeenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screeenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        //Get world position from camera-relative coordinated
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screeenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screeenMousePosNear);

        //Cast a ray to locate an object the cursor is hovering over
        RaycastHit hit;
        //LayerMask mask = LayerMask.GetMask("Default") + LayerMask.GetMask("Card") + LayerMask.GetMask("CubeOutOfBox");
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, Camera.main.farClipPlane, mask);

        return hit;
    }
}
