using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    AudioSource _mermaidShow; // This is audio component attached to the teleport LOC 3
    public AudioClip _trackToPlay; // This the mermaid song we are wanting to play
    public bool _alreadyPlayed = false; // this bool logs whether or not the song has been play already
    public bool _playerInLocation; // This logs the player's postion. 
    public float _volume; // this is the volume setting available in the inspector window.
    public GameObject _spotLight;



    // Start is called before the first frame update
    void Start()
    {
        _mermaidShow = GetComponent<AudioSource>(); // This line of code grabs and stores the audiosouce on start up.
        _spotLight.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) // This function is to trigger the audio clip to start when the player is in position
    {
        if (other.gameObject.CompareTag ("Player")) // this makes sure that only objects with the player tag can trigger the function
        {
            _playerInLocation = true; // this sets players location to in position for the mermaid show. 
            if (!_alreadyPlayed) // this is saying... if the track has not already been played then continue to the next line in the script.
            {
                // this section that actually initiates playing the audio clip.
                _mermaidShow.Play();
                _spotLight.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit(Collider other) // This function is to stop the audio clip when the player leaves the position
    {
        if (other.gameObject.CompareTag("Player")) // this makes sure that only objects with the player tag can control the trigger the function
        {
            _playerInLocation = false; // this logs that the player has left the mermaid show position
            {
                _mermaidShow.Stop(); // Stops playing the audio clip.
                _spotLight.SetActive(false);
            }

        }



    }

}
