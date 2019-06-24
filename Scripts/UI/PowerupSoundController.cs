using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip plusTenNoise;
    [SerializeField] private AudioClip destroyKillersNoise;
    
    private AudioSource powerupNoiseSource;

    // Start is called before the first frame update
    void Start()
    {
        powerupNoiseSource = GetComponent<AudioSource>();
        Messenger.AddListener(GameEvent.DESTROY_KILLERS, AnnounceDestroyKillers);
        Messenger.AddListener(GameEvent.PLUS_TEN, AnnouncePlusTen);
    }

    public void AnnounceDestroyKillers()
    {
        try
        {
        powerupNoiseSource.clip = destroyKillersNoise;
        powerupNoiseSource.Play();
        }
        catch(MissingReferenceException mre) {}
    }

    public void AnnouncePlusTen()
    {
        try
        {
        powerupNoiseSource.clip = plusTenNoise;
        powerupNoiseSource.Play();
        }
        catch(MissingReferenceException mre) {}
    }
}
