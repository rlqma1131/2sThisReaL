using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioClip musicA;
    public AudioClip musicB;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 0.1f);
        audioSource.volume = savedVolume;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "StartScene":
            case "GenderSelectScene":
            case "SettingScene":
                PlayMusic(musicA);
                break;

            case "MainScene":
                PlayMusic(musicB);
                break;

            case "EndingScene":
                StopMusic();
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }
}
