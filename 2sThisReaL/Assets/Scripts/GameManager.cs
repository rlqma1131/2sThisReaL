using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            // 에디터 방어코드
            if (_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    // Player 참조
    private Player _player;
    public Player Player
    {
        get {  return _player; }
        set {  _player = value; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 초기화
    public void Init(Player player)
    {
        this.Player = player;
    }
}
