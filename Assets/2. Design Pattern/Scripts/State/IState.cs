
using MyProject.Skill;

namespace MyProject.State{

    public interface IState
    {
        public void Initialize(Player player);

        public void Enter();
        public void Update();

        public void Exit();
    }
}
