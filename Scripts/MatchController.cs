using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MatchController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public int MAX_LIVES, currentLives;
    [SerializeField] GameObject ballPrefab, targetZonePrefab;
    [SerializeField] Transform background;
    [SerializeField] float ballScaleUnits = 2f;
    [SerializeField] float zoneScaleUnits = 3f;
    [SerializeField] Vector2 scoreSpeedPercent = new Vector2(); 
    float ballScale;
    Vector3 ballSpawnPosition;
    Scaler scaler;
    bool levelReady = false;
    BallController currentBallController;
    TargetZone currentTargetZone;
    int currentSetID =0;
    // Start is called before the first frame update
    void Start()
    {
        SetupLevel();
    }
    private void SetupLevel()
    {
        Application.targetFrameRate = 60;
        scaler = new Scaler(scoreSpeedPercent);
        background.position = new Vector3(scaler.WorldCenter.x, scaler.WorldCenter.y, background.position.z);
        background.localScale = new Vector3(scaler.WorldWidth, scaler.WorldHeight, 1);
        ballSpawnPosition = new Vector3(scaler.WorldCenter.x,
                (float)(scaler.WorldBotLeft.y + scaler.UnitWorldHeight * 5), -2);

        ballScale = (float)(ballScaleUnits * scaler.UnitWorldHeight);
        Debug.Log("UWH" + scaler.UnitWorldHeight + " | Ball scale:" + ballScale);
        score = 0;
        currentLives = MAX_LIVES;
        currentSetID = 0;
        SpawnSet();
        scoreText.text = "" + score;
        levelReady = true;
    }
    void FixedUpdate()
    {
        if (!levelReady)
            return;

        if (currentBallController.flicked)
        {
            SpawnSet();
        }
    }

    void SpawnSet()
    {
        currentSetID++;
        SpawnTargetZone();
        SpawnBall();
    }

    void SpawnTargetZone()
    {
        int rand = Random.Range(20, 90);
        Vector3 zonePos = new Vector3(scaler.WorldCenter.x, (float)scaler.UnitWorldHeight * rand +
                                scaler.WorldBotLeft.y, -1.5f);
        currentTargetZone = Instantiate(targetZonePrefab, zonePos, Quaternion.identity).GetComponent<TargetZone>();
        currentTargetZone.transform.localScale = new Vector3(
            scaler.WorldWidth, zoneScaleUnits * (float)scaler.UnitWorldHeight, 2f
        );
        currentTargetZone.id = currentSetID;
    }

    void SpawnBall()
    {
        currentBallController = Instantiate(ballPrefab, ballSpawnPosition, Quaternion.identity)
                                .GetComponent<BallController>();
        currentBallController.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
        currentBallController.scaler = scaler;
        currentBallController.targetZone = currentTargetZone;
        currentBallController.mc = this;
    }

    public void BallScored(){
        score++;
        scoreText.text = "" + score;
    }

    public void BallFailed(){
        currentLives--;
        if (currentLives == 0){
            ResetGame();
        }
    } 

    public void ResetGame(){
        SceneManager.LoadScene(0);
        // currentBallController.DespawnBall(false);
        // currentTargetZone.DespawnZone();
        // SetupLevel();
        // Debug.Log("GAME RESET");
    }

}
