using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player Player { get; set; }

    private void Awake()
    {
        // 인스턴스가 아직 없다면 이 오브젝트를 인스턴스로 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Start()
    {
        // 중복 인스턴스 제거는 Start에서 처리 (Destroy 안정적으로 동작함)
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Player player)
    {
        Player = player;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 특정 씬에서만 파괴 (예: "MainMenu", "EndingScene" 등)
        if (scene.name == "MainMenu" || scene.name == "EndingScene")
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        // 씬 로딩 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}