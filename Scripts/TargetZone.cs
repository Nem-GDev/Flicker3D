using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public int id;

    public void DespawnZone(){
        Destroy(gameObject);
    }
}
