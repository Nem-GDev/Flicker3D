using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float flickForceModifier = 20f;
    public bool flicked = false;
    public TargetZone targetZone;
    public Scaler scaler;
    public MatchController mc;
    Rigidbody ballrb;
    FlickInfo flick;

    void Start()
    {
        ballrb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (flicked)
            return;

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                flick = new FlickInfo(t);
            }
            else if (t.phase == TouchPhase.Ended)
            {
                flick.EndFlick(t);
            }
        }
    }

    void FixedUpdate()
    {

        if (flicked)
        {
            if (ballrb.velocity.y < 0f)
            {
                if (targetZone != null)
                {
                    targetZone.DespawnZone();
                    StartCoroutine(DespawnBall(3f));
                }
            }
            return;
        }

        if (flick != null)
            if (flick.readyToProcess)
            {
                double flickForce = flick.verticalFlickDistance / scaler.UnitScreenHeight * flickForceModifier;
                ballrb.isKinematic = false;
                ballrb.AddForce(new Vector3(0f, (float)flickForce, 0f));
                Debug.Log($"Added {flickForce} to ball");
                flick.readyToProcess = false;
                flicked = true;
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        TargetZone hitZone = other.transform.GetComponent<TargetZone>();
        if (hitZone == null)
            return;

        if (hitZone.id == targetZone.id)
        {
            Debug.Log(ballrb.velocity);
            if (ballrb.velocity.y < scaler.UpperSpeed && ballrb.velocity.y > scaler.LowerSpeed)
            {
                Debug.Log("SCORED: " + ballrb.velocity.y);
                targetZone.DespawnZone();
                this.DespawnBall(true);
            }
            else
            {
                Debug.Log("Hit Speed not good: " + ballrb.velocity.y);
                targetZone.DespawnZone();
                StartCoroutine(DespawnBall(3f));
                //this.DespawnBall();
            }
        }
    }

    public void DespawnBall(bool scored)
    {
        if(scored)
            mc.BallScored();
        else    
            mc.BallFailed();
        Destroy(gameObject);
    }

    public IEnumerator DespawnBall(float timer)
    {
        yield return new WaitForSeconds(timer);
        DespawnBall(false);
    }
}
