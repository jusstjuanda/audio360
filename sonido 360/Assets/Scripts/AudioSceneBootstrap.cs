using UnityEngine;

public static class AudioSceneBootstrap
{
    private const string PlayerName = "Player";
    private const string CapsuleName = "AudioCapsule";
    private const string FloorName = "Floor";
    private const string AudioResourcePath = "Audio/override";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        DisableDefaultMainCameraListener();
        CreateFloor();
        CreateAudioCapsule();
        CreatePlayer();
    }

    private static void DisableDefaultMainCameraListener()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera == null) return;

        AudioListener existingListener = mainCamera.GetComponent<AudioListener>();
        if (existingListener != null)
        {
            Object.Destroy(existingListener);
        }

        mainCamera.SetActive(false);
    }

    private static void CreateFloor()
    {
        if (GameObject.Find(FloorName) != null) return;

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = FloorName;
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(5f, 1f, 5f);
    }

    private static void CreateAudioCapsule()
    {
        if (GameObject.Find(CapsuleName) != null) return;

        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsule.name = CapsuleName;
        capsule.transform.position = new Vector3(0f, 1f, 8f);

        AudioSource source = capsule.AddComponent<AudioSource>();
        capsule.AddComponent<AudioCapsule>();

        source.clip = Resources.Load<AudioClip>(AudioResourcePath);
        if (source.clip != null)
        {
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSceneBootstrap: no se encontro el AudioClip en Resources/{AudioResourcePath}.");
        }
    }

    private static void CreatePlayer()
    {
        if (GameObject.Find(PlayerName) != null) return;

        GameObject player = new GameObject(PlayerName);
        player.transform.position = new Vector3(0f, 1f, 0f);
        player.AddComponent<CharacterController>();

        GameObject cameraObject = new GameObject("PlayerCamera");
        cameraObject.transform.SetParent(player.transform);
        cameraObject.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        cameraObject.transform.localRotation = Quaternion.identity;
        cameraObject.AddComponent<Camera>();
        cameraObject.AddComponent<AudioListener>();

        player.AddComponent<PlayerController>();
    }
}
