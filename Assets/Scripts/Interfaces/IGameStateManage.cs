namespace Com.Capra314Cabra.Project_2048Ex
{
    public interface IGameStateManager
    {
        GameState GameState { get; set; }
        PlayerStatus PlayerStatus { get; set; }
    }
}