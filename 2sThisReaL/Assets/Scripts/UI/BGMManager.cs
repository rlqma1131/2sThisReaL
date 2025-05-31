using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioClip musicA;
    public AudioClip musicB;
    public AudioClip musicC;

    private bool isInMusicZone = false;

    private AudioSource audioSource;
    private Transform player;

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
            case "HappyEndingScene":
                StopMusic();
                break;
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainScene") return;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            return;
        }

        Vector3 pos = player.position;

        // 특정 위치 범위 (예: x:5~10, z:10~15)
        bool isInZone = pos.x >= -206 && pos.x <= 138 && pos.z >= 187 && pos.z <= 600;

        if (isInZone && !isInMusicZone)
        {
            PlayMusic(musicC);
            isInMusicZone = true;
        }
        else if (!isInZone && isInMusicZone)
        {
            PlayMusic(musicB);
            isInMusicZone = false;
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
