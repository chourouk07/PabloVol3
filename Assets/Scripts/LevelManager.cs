using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Transform player;
    public int score;
    public int levelItems;
    public Transform[] particles;
    public AudioClip[] levelSounds;

    private void Awake()
    {
        instance = this;
    }




    public void PlaySound(AudioClip sound, Vector3 ownerPos)
    {
        GameObject obj = SoundFXPooler.current.GetPooledObject();
        AudioSource audio = obj.GetComponent<AudioSource>();
        obj.transform.position = ownerPos;
        obj.SetActive(true);
        audio.PlayOneShot(sound);
        StartCoroutine(DisableSound(audio));
    }


     IEnumerator DisableSound(AudioSource audio)
    {
        while (audio.isPlaying)        
            yield return new WaitForSeconds(0.5f);      
        audio.gameObject.SetActive(false);
    }
}   
