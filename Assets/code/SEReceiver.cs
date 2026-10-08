using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEReceiver : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource =
            GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (SettingManager.Instance == null)
        {
            return;
        }

        audioSource.volume =
            SettingManager.Instance.SEVolume;
    }
}