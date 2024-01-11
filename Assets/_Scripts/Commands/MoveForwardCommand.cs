public class MoveForwardCommand : ICommand
{
    public void Action(GameController controller, MyCharacterController c, ICommand nextStep)
    {
        var character = c.Character;
        character.CurrentPosition += character.Forward;
        var isNextMoveFree = nextStep is MoveForwardCommand && c.IsNextMoveFree(character.CurrentPosition, character.CurrentDirection);
        c.Visualizer.MoveTo(controller.GetPosByMap(character.CurrentPosition), isNextMoveFree);
    }
}
