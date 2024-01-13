using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DestinationIdentifier
{
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
}

public class LocationPortal : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private DestinationIdentifier destinationIdentifier;
    bool isInsideTrigger;
    public Transform SpawnPoint => spawnPoint;

    private Collider2D _player;

    private void OnTriggerEnter2D(Collider2D player)
    {
        isInsideTrigger = true;
        _player = player;
    }

    private void OnTriggerExit2D(Collider2D player)
    {
        isInsideTrigger = false;
    }

    private void Update()
    {
        if (isInsideTrigger && _player != null)
        {
            var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationIdentifier == this.destinationIdentifier);
            _player.transform.position = destPortal.spawnPoint.position;
        }
    }
}
