
public abstract class FlowState : IFlowState
{
    protected MonsterGenerator _generator;
    protected LevelConfig.LevelDef _levelDef;

    public enum State
    {
        Monster,
        Start,
        End,
        BetweenWaveState
    }

    public FlowState(MonsterGenerator generator,LevelConfig.LevelDef levelDef)
    {
        _generator = generator;
        _levelDef = levelDef;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
       
    }

    public virtual void OnUpdate()
    {
       
    }

    public virtual void OnMonsterDestroyed()
    {

    }

    public virtual void OnGameOver()
    {
        _generator.gameWon = false;
        _generator.SetFlowState(State.End);
    }
}
