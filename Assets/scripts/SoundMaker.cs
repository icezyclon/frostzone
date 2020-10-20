using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static SoundMaker Instance;

    public AudioClip shotSound;
    public AudioClip hitSound;
    public AudioClip killSound;
    public AudioClip iceShotSound;
    public AudioClip iceHitSound;
    public AudioClip jumpSound;

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("More than 1 instance of SoundMaker!");
        }
        Instance = this;
    }

    public void Shot()
    {
        MakeSound(shotSound);
    }

    public void Hit()
    {
        MakeSound(hitSound);
    }
    public void Kill()
    {
        MakeSound(killSound);
    }
    public void IceShot()
    {
        MakeSound(iceShotSound);
    }
    public void IceHit()
    {
        MakeSound(iceHitSound);
    }
    public void Jump()
    {
        MakeSound(jumpSound);
    }


    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }
}
