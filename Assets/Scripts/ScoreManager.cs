using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour {


    public int goal, mode;
    public float freezeDuration, slowDuration, fadeDuration;
    public string menuLevel;
    public GameObject menuPause, scoreObject, healthObject, menuControls, menuGamerules;
    private Dictionary<PlayerController, int> players = new Dictionary<PlayerController, int>();
    private Dictionary<int, int> teams = new Dictionary<int, int>();
    private Dictionary<Rigidbody, Vector3> oldForces = new Dictionary<Rigidbody, Vector3>();
    private Text centralDisplay;
    private bool waitingForA = false, pause = false, launched = false, controlsDisplay = false, gamerulesDisplay = false;
    private List<Rigidbody> ballsRigidbodies = new List<Rigidbody>();


    // Use this for initialization
    void Start() {
        StartCoroutine(LaunchWithoutMenu());
    }

    private IEnumerator LaunchWithoutMenu()
    {
        yield return new WaitForSeconds(1);
        if (!launched)
        {
            initPawns();
            SetScoreMode(mode, goal, 4);
            InitUI();
        }
    }

    public void FixedUpdate()
    {
        if (waitingForA)
        {
            if (OnePlayerButtonControllerDown("0"))
            {
                ChangeLevel();
            }
        }
        if (pause)
        {
            if (Input.GetButtonDown("Pause1") || Input.GetButtonDown("Pause2") || Input.GetButtonDown("Pause3") || Input.GetButtonDown("Pause4"))
            {
                if (controlsDisplay)
                {
                    Controls(false);
                }
                else if (gamerulesDisplay)
                {
                    Gamerules(false);
                }
                else
                {
                    Unpause();
                }
            }
            if (OnePlayerButtonControllerDown("1"))
            {
                if (controlsDisplay)
                {
                    Controls(false);
                }
                else if (gamerulesDisplay)
                {
                    Gamerules(false);
                }
                else
                {
                    Unpause();
                }
            }
            if (OnePlayerButtonControllerDown("0"))
            {
                ChangeLevel();
            }
            if (OnePlayerButtonControllerDown("2"))
            {
                Controls(true);
            }
            if (OnePlayerButtonControllerDown("3"))
            {
                Gamerules(true);
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause1") || Input.GetButtonDown("Pause2") || Input.GetButtonDown("Pause3") || Input.GetButtonDown("Pause4"))
            {
                Pause();
            }
        }


    }

    public void initPawns()
    {
        PlayerController[] instantiatePlayers = Object.FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in instantiatePlayers)
        {
            if (!players.ContainsKey(player))
            {
                players.Add(player, player.health);
            }
            if (!teams.ContainsKey(player.team))
            {
                teams.Add(player.team, 0);
            }
        }
        BallBehavior[] balls = FindObjectsOfType<BallBehavior>();
        foreach (BallBehavior ball in balls)
        {
            ballsRigidbodies.Add(ball.GetComponent<Rigidbody>());
        }
    }

    private bool OnePlayerButtonControllerDown(string button)
    {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKeyDown("joystick " + i + " button " + button))
            {
                return true;
            }
        }
        return false;

    }

    public void SetScoreMode(int mode, int score, int nbplayer)
    {
        launched = true;
        this.mode = mode;
        if (this.mode == 0)
        {
            foreach (PlayerController player in players.Keys)
            {
                player.health = score;
            }
        }
        if (this.mode == 1)
        {
            goal = score;
        }
    }
    private void InitUI()
    {
        SetVisible(menuPause, false);
        centralDisplay = GameObject.Find("CentralDisplay").GetComponent<Text>();
        StartCoroutine(FadeTextToZeroAlpha(fadeDuration, centralDisplay));
        Text currentText;
        if (mode == 0)
        {
            foreach (KeyValuePair<PlayerController, int> player in players)
            {
                currentText = healthObject.GetComponentsInChildren<Text>().ToList().Find(t => t.name == "Health" + player.Key.player);
                if (currentText.name == "Health" + player.Key.player)
                {
                    currentText.GetComponent<Text>().text = "Player " + player.Key.player + " : " + player.Value;
                }
            }
            if (players.Count != 4)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (players.Keys.Where(player => player.player == i).Count() == 0)
                    {
                        healthObject.GetComponentsInChildren<Text>().ToList().Find(t => t.name == "Health" + i).gameObject.SetActive(false);
                    }
                }
            }
            SetVisible(healthObject, true);
            SetVisible(scoreObject, false);
        }
        if (mode == 1)
        {
            string word;
            if (teams.Count == players.Count)
            {
                word = "Player ";
            } else {
                word = "Team ";
            }
            foreach (KeyValuePair<int, int> team in teams)
            {
                currentText = scoreObject.GetComponentsInChildren<Text>().ToList().Find(t => t.name == "Score" + team.Key);
                if (currentText.name == "Score" + team.Key)
                {
                    currentText.GetComponent<Text>().text = word + team.Key + " : " + team.Value;
                }
            }
            if (teams.Count != 4)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (teams.Keys.Where(team => team == i).Count() == 0)
                    {
                        scoreObject.GetComponentsInChildren<Text>().ToList().Find(t => t.name == "Score" + i).gameObject.SetActive(false);
                    }
                }
            }
            SetVisible(healthObject, false);
            SetVisible(scoreObject, true);
        }
    }

    private void SetVisible(GameObject parent, bool visibility)
    {
        for (int i = 0 ; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).gameObject.SetActive(visibility);
        }
    }

    private IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private void Pause()
    {
        Freeze();
        pause = true;
        SetVisible(menuPause, true);

    }

    private void Unpause()
    {
        pause = false;
        SetVisible(menuPause, false);
        StartCoroutine(EndFreeze(0, 0));
    }

    private void Controls(bool active)
    {
        controlsDisplay = active;
        SetVisible(menuControls, active);
    }

    private void Gamerules(bool active)
    {
        gamerulesDisplay = active;
        SetVisible(menuGamerules, active);

    }

    public void UpdateScore(int team, int value)
    {
        if (teams.ContainsKey(team) && mode == 1)
        {
            int oldValue = teams[team];
            teams[team] = oldValue+value;
            Text[] texts = scoreObject.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                if (text.name == "Score" + team)
                {
                    if (teams.Count == players.Count)
                    {
                        text.text = "Player " + team + " : " + teams[team];
                    }
                    else
                    {
                        text.text = "Team " + team + " : " + teams[team];
                    }
                }
            }
            if (teams[team] >= goal)
            {
                Debug.Log(goal);
                Debug.Log(teams[team]);
                if (teams.Count == players.Count)
                {
                    Win("Player " + players.Keys.Where(player => player.team == team).First().player);
                }
                else
                {
                    Win("Team " + team);

                }
            }
        }
    }

    public void UpdateHealth(PlayerController player, int value)
    {
        if (players.ContainsKey(player) && mode == 0)
        {
            int oldValue = players[player];
            players[player] = oldValue + value;
            Text[] texts = healthObject.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                if (text.name == "Health" + player.player)
                {
                    text.text = "Player " + player.player + " : " + players[player];
                }
            }
            if (players[player] <= 0)
            {
                player.Die();
                int survivant=-1;
                foreach(KeyValuePair<PlayerController, int> p in players)
                {
                    if(p.Value>0)
                    {
                        if (survivant == -1)
                        {
                            survivant = p.Key.team;
                        }
                        if (p.Key.team != survivant)
                        {
                            return;
                        }
                    }
                }
                Win("Team " + survivant);
            }
        }
    }

    public bool IsAlive(PlayerController player)
    {
        if (players.ContainsKey(player))
        {
            return players[player] > 0;
        }
        return false;
    }

    public void Win(string winner)
    {
        centralDisplay.text = winner + " wins !\n\n\nScore :\n\n";
        IEnumerable<KeyValuePair<int, int>> query=teams.OrderByDescending(t => t.Value);
        foreach (KeyValuePair<int, int> team in query)
        {
            centralDisplay.text += "Team " + team.Key + " : " + team.Value+"\n\n";
        }
        centralDisplay.text += "\n\n\nNext (A)";
        TestPerfect();
        centralDisplay.color= new Color(centralDisplay.color.r, centralDisplay.color.g, centralDisplay.color.b, 1);
        waitingForA = true;
    }

    private void TestPerfect()
    {
        int nbScoringTeams = 0;
        foreach (int score in teams.Values)
        {
            if (score > 0)
            {
                nbScoringTeams++;
            }
        }
        if (nbScoringTeams == 1)
        {
            if (teams.Count == 2)
            {
                centralDisplay.text = "PERFECT !!\n\n" + centralDisplay.text;
            }
            else
            {
                centralDisplay.text = "ULTRA PERFECT !!\n\n" + centralDisplay.text;
            }
                
        }
    }

    private void ChangeLevel()
    {
        waitingForA = false;
        SceneManager.LoadScene(menuLevel);
    }

    public void TempFreeze()
    {
        Freeze();
        StartCoroutine(EndFreeze(freezeDuration, slowDuration));
    }

    private void Freeze()
    {
        foreach (PlayerController player in players.Keys)
        {
            if (player != null)
            {
                if (!oldForces.ContainsKey(player.GetComponent<Rigidbody>()))
                {
                    oldForces.Add(player.GetComponent<Rigidbody>(), player.GetComponent<Rigidbody>().velocity);
                }
                player.alive = false;
                player.GetComponent<Rigidbody>().Sleep();
            }
        }
        foreach (Rigidbody ball in ballsRigidbodies)
        {
            if (ball != null)
            {
                if (!oldForces.ContainsKey(ball.GetComponent<Rigidbody>()))
                {
                    oldForces.Add(ball.GetComponent<Rigidbody>(), ball.GetComponent<Rigidbody>().velocity);
                    ball.Sleep();
                }
            }
        }
    }

    private IEnumerator EndFreeze(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);


        foreach (KeyValuePair<Rigidbody, Vector3> save in oldForces)
        {
            if (save.Key != null)
            {
                save.Key.GetComponent<Rigidbody>().WakeUp();
            }
        }
        foreach(PlayerController player in players.Keys)
        {
            if (player != null)
            {
                player.alive = true;
            }
        }
        if (duration == 0)
        {
            foreach (KeyValuePair<Rigidbody, Vector3> save in oldForces)
            {
                if (save.Key != null)
                {
                    save.Key.velocity = save.Value;
                }
            }
        }
        for(float timer=0 ; timer<duration ; timer += Time.deltaTime)
        {
            foreach (KeyValuePair<Rigidbody, Vector3> save in oldForces)
            {
                if (save.Key != null)
                {
                    save.Key.velocity=save.Value*timer/duration;
                }
            }
            yield return null;
        }
        oldForces.Clear();

    }
}
