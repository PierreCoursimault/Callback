﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour {


    public int goal;
    public float freezeDuration, slowDuration, fadeDuration;
    public string nextlevel;
    private Dictionary<PlayerController, int> players=new Dictionary<PlayerController, int>();
    private Dictionary<int, int> teams = new Dictionary<int, int>();
    private List<Text> scoreTexts = new List<Text>();
    private List<Text> healthTexts = new List<Text>();
    private Dictionary<Rigidbody, Vector3> oldForces = new Dictionary<Rigidbody, Vector3>();
    private Text centralDisplay;
    private bool waitingForA = false, score = false, slowed = false;
    private List<Rigidbody> ballsRigidbodies = new List<Rigidbody>();


    // Use this for initialization
    void Start () {
        PlayerController[] instantiatePlayers = Object.FindObjectsOfType<PlayerController>();
        foreach(PlayerController player in instantiatePlayers)
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
        Text[] UIs=Component.FindObjectsOfType<Text>();
        string name;
        foreach (Text text in UIs)
        {
            name = text.name;
            if (name == "CentralDisplay")
            {
                centralDisplay = text;
                StartCoroutine(FadeTextToZeroAlpha(fadeDuration, centralDisplay));
            }
            foreach (KeyValuePair<PlayerController, int> player in players)
            {
                if(name == "Health" + player.Key.player)
                {
                    text.text = "Player " + player.Key.player + " : "+player.Value;
                    healthTexts.Add(text);
                    break;
                }
            }
            foreach (KeyValuePair<int, int> team in teams)
            {
                if (name == "Score" + team.Key)
                {
                    text.text = "Team " + team.Key + " : " + team.Value;
                    scoreTexts.Add(text);
                    break;
                }
            }
        }
        BallBehavior[] balls = FindObjectsOfType<BallBehavior>();

        foreach (BallBehavior ball in balls)
        {
            ballsRigidbodies.Add(ball.GetComponent<Rigidbody>());
        }
    }

    public void FixedUpdate()
    {
        if (waitingForA)
        {
            if(Input.GetButtonDown("Validate1") || Input.GetButtonDown("Validate2") || Input.GetButtonDown("Validate3") || Input.GetButtonDown("Validate4"))
            {
                ChangeLevel();
            }
        }
    }

    public void SetScoreMode(bool mode, int score)
    {
        this.score = !mode;
        if (this.score)
        {
            goal = score;
        }
        else
        {
            goal = -1;
            foreach(PlayerController player in players.Keys)
            {
                player.health = score;
            }
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

    public void UpdateScore(int team, int value)
    {
        if (teams.ContainsKey(team))
        {
            int oldValue = teams[team];
            teams[team] = oldValue+value;
            foreach (Text text in scoreTexts)
            {
                if (text.name == "Score" + team)
                {
                    text.text = "Team " + team + " : " + teams[team];
                }
            }
            if (goal != -1 && teams[team] >= goal)
            {
                Win("Team " +team);
            }
        }
    }

    public void UpdateHealth(PlayerController player, int value)
    {
        if (players.ContainsKey(player))
        {
            int oldValue = players[player];
            players[player] = oldValue + value;
            foreach (Text text in healthTexts)
            {
                if (text.name == "Health" + player.player)
                {
                    text.text = "Player " + player.player + " : " + players[player];
                }
            }
            if (players[player] <= 0 && !score)
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
        SceneManager.LoadScene(nextlevel);
    }

    public void Freeze()
    {
        foreach (PlayerController player in players.Keys)
        {
            if (player != null) {
                if (!oldForces.ContainsKey(player.GetComponent<Rigidbody>()))
                {
                    oldForces.Add(player.GetComponent<Rigidbody>(), player.GetComponent<Rigidbody>().velocity);
                }
                player.alive = false;
                player.GetComponent<Rigidbody>().Sleep();
            }
        }
        foreach(Rigidbody ball in ballsRigidbodies)
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

        StartCoroutine(EndFreeze());
    }

    private IEnumerator EndFreeze()
    {
        yield return new WaitForSeconds(freezeDuration);


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
        if (slowDuration == 0)
        {
            foreach (KeyValuePair<Rigidbody, Vector3> save in oldForces)
            {
                if (save.Key != null)
                {
                    save.Key.velocity = save.Value;
                }
            }
        }
        for(float timer=0 ; timer<slowDuration ; timer += Time.deltaTime)
        {
            foreach (KeyValuePair<Rigidbody, Vector3> save in oldForces)
            {
                if (save.Key != null)
                {
                    save.Key.velocity=save.Value*timer/slowDuration;
                }
            }
            yield return null;
        }
        oldForces.Clear();

    }
}
