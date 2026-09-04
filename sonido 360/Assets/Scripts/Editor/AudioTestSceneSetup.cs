using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AudioTestSceneSetup
{
    private const string AudioClipPath = "Assets/assets/override.mp3";

    [MenuItem("Tools/Audio 3D Test/Setup Test Scene")]
    public static void SetupScene()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (mainCamera != null)
        {
            AudioListener existingListener = mainCamera.GetComponent<AudioListener>();
            if (existingListener != null)
            {
                Undo.DestroyObjectImmediate(existingListener);
            }
            Undo.RecordObject(mainCamera, "Disable Main Camera");
            mainCamera.SetActive(false);
        }

        GameObject floor = GameObject.Find("Floor");
        if (floor == null)
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.position = Vector3.zero;
            floor.transform.localScale = new Vector3(5f, 1f, 5f);
            Undo.RegisterCreatedObjectUndo(floor, "Create Floor");
        }

        GameObject capsule = GameObject.Find("AudioCapsule");
        if (capsule == null)
        {
            capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            capsule.name = "AudioCapsule";
            capsule.transform.position = new Vector3(0f, 1f, 8f);
            Undo.RegisterCreatedObjectUndo(capsule, "Create AudioCapsule");
        }

        if (capsule.GetComponent<AudioSource>() == null)
        {
            Undo.AddComponent<AudioSource>(capsule);
        }

        AudioCapsule audioCapsuleComponent = capsule.GetComponent<AudioCapsule>();
        if (audioCapsuleComponent == null)
        {
            audioCapsuleComponent = Undo.AddComponent<AudioCapsule>(capsule);
        }

        SerializedObject serializedCapsule = new SerializedObject(audioCapsuleComponent);
        SerializedProperty clipProperty = serializedCapsule.FindProperty("audioClip");
        if (clipProperty.objectReferenceValue == null)
        {
            clipProperty.objectReferenceValue = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioClipPath);
            serializedCapsule.ApplyModifiedProperties();
        }

        AudioSource capsuleSource = capsule.GetComponent<AudioSource>();
        if (capsuleSource.clip == null)
        {
            capsuleSource.clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AudioClipPath);
        }

        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            player = new GameObject("Player");
            player.transform.position = new Vector3(0f, 1f, 0f);
            Undo.RegisterCreatedObjectUndo(player, "Create Player");
        }

        if (player.GetComponent<CharacterController>() == null)
        {
            Undo.AddComponent<CharacterController>(player);
        }

        Camera playerCamera = player.GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            GameObject cameraObject = new GameObject("PlayerCamera");
            Undo.RegisterCreatedObjectUndo(cameraObject, "Create Player Camera");
            cameraObject.transform.SetParent(player.transform);
            cameraObject.transform.localPosition = new Vector3(0f, 0.6f, 0f);
            cameraObject.transform.localRotation = Quaternion.identity;
            playerCamera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
        }

        if (player.GetComponent<PlayerController>() == null)
        {
            PlayerController controller = Undo.AddComponent<PlayerController>(player);
            SerializedObject serializedController = new SerializedObject(controller);
            serializedController.FindProperty("playerCamera").objectReferenceValue = playerCamera;
            serializedController.ApplyModifiedProperties();
        }

        Selection.activeGameObject = player;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("Audio 3D Test Scene lista: Floor, AudioCapsule y Player configurados y guardados en la escena. Presiona Play para probar.");
    }
}
