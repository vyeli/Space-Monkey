using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Checkpoint : MonoBehaviour
{
    public GameObject cpON, cpOFF;


    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (GameManager.instance.respawnPosition != transform.position))
        {
            SoundManager.instance.PlaySFX(SoundManager.instance.Pickup);
            UiManager.instance.ShowNotification("Punto de control", 2f);
            
            GameManager.instance.SetSpawnPoint(transform.position);

            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

            foreach (Checkpoint cp in checkpoints)
            {
                cp.cpOFF.SetActive(true);
                cp.cpON.SetActive(false);
            }

            cpOFF.SetActive(false);
            cpON.SetActive(true);
        }
        Debug.Log("Checkpoint reached");
    }
}
