using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMReceiver : MonoBehaviour
{
    private AudioSource audioSource;

    /*
     * Inspectorで設定した元の音量
     */
    private float baseVolume;

    private void Awake()
    {
        audioSource =
            GetComponent<AudioSource>();

        baseVolume =
            audioSource.volume;
    }

    private void Update()
    {
        if (SettingManager.Instance == null)
        {
            return;
        }

        audioSource.volume =
            baseVolume *
            SettingManager.Instance.BGMVolume;
    }
}