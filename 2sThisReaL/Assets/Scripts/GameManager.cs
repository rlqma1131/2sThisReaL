using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player Player { get; set; }
    public Enemy Enemy { get; set; }

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 이동 시 유지하고 싶으면
    }
}