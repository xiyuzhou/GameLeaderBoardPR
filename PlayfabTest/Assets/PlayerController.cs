using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject playButton;

    public float speed;
    private Rigidbody rig;
    private float startTime;
    private float timeTaken;

    private int collectablesPicked = 0;
    public int maxCollectables = 10;
    private bool isPlaying;
    public TextMeshProUGUI curTimeText;
    private bool isPlayed = false;
    public TextMeshProUGUI buttonText;
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (!isPlaying)
            return;
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.AddForce(new Vector3(x, rig.velocity.y, z) * speed);

        curTimeText.text = (Time.time - startTime).ToString("F2");

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            Destroy(other.gameObject);
            if (collectablesPicked == maxCollectables)
                End();
        }
    }
    public void Begin()
    {
        if (isPlayed)
            SceneManager.LoadScene("Game1");
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);
    }
    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        LeaderBoard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);
        buttonText.text = "Replay";
        isPlayed = true;
        Invoke("UpdateLeaderBoard", 1.5f);
    }

    private void UpdateLeaderBoard()
    {
        LeaderBoard.instance.DisplayLeaderboard();
    }
}
