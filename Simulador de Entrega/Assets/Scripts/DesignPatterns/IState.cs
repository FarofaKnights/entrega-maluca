public interface IState {
    public void Enter();
    public void Execute(float dt);
    public void Exit();
}
