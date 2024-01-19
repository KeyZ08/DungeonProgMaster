using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace DPM.App
{
    public class CommandsInstaller : MonoInstaller
    {
        private List<string> allCommands = new List<string>();
        private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public override void InstallBindings()
        {
            //регистрация команд
            BindCommand(new MoveForwardCommand(), "Forward");
            BindCommand(new RotateLeftCommand(), "TurnLeft");
            BindCommand(new RotateRightCommand(), "TurnRight");
            BindCommand(new AttackCommand(), "Attack");
            //BindCommand(new OnComeCommand(), "OnCome");
            BindCommand(new TakeCommand(), "Take");

            Container.Bind<CommandsInstaller>()
                .FromInstance(this).AsSingle();
        }

        public void BindCommand<TCommand>(TCommand command, string name) where TCommand : ICommand
        {
            commands.Add(name, command);
            allCommands.Add(name);
        }

        public List<string> GetAllCommands()
            => allCommands.Select(x => x).ToList();

        public bool TryGetCommand(string commandName, out ICommand command)
            => commands.TryGetValue(commandName, out command);
    }
}