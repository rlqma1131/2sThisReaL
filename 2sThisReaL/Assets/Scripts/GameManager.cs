using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

<<<<<<< HEAD
    public Player Player { get; private set; }


=======
    public Player Player { get; set; }
>>>>>>> Dev_05

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Init(Player player)
    {
        this.Player = player;
    }

    public void Init(Player player)
    {
        Player = player;
    }
}