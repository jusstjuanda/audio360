using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class AudioCapsule : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        ConfigureAudioSource();
    }

    private void Reset()
    {
        ConfigureAudioSource();

#if UNITY_EDITOR
        if (audioClip == null)
        {
            audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/assets/override.mp3");
            GetComponent<AudioSource>().clip = audioClip;
        }
#endif
    }

    private void ConfigureAudioSource()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 2f;
        audioSource.maxDistance = 20f;
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        if (audioClip != null && audioSource.clip == null)
        {
            audioSource.clip = audioClip;
        }
    }

    private void OnDrawGizmosSelected()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, audioSource.minDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, audioSource.maxDistance);
    }
}
