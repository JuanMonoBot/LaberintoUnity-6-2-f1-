using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyVision : MonoBehaviour
{
    public float detectionDistance = 8f;

    public Transform player;
    public GameObject alertText;

    bool hasSeenPlayer = false;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionDistance && !hasSeenPlayer)
        {
            StartCoroutine(ShowAlert());
        }
    }

    IEnumerator ShowAlert()
    {
        hasSeenPlayer = true;

        alertText.SetActive(true);

        yield return new WaitForSeconds(3f);

        alertText.SetActive(false);
    }
}