public class GameManager
{
    private static GameManager instance;
    public static GameManager Instance => instance ??= new GameManager();

    public Player Player { get; set; }
    public Enemy Enemy { get; set; }

    private GameManager() { }
}