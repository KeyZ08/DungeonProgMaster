public interface ICommand
{
    void Action(GameController controller, MyCharacterController c, ICommand nextStep = null);
}
