using System;

namespace DungeonProgMaster
{
    class Script
    {
        public Command Move { get; private set; }

        public string Sketch { get; private set; }

        public string Declaration { get; private set; }

        private readonly Action<Player> Doing;

        public Script(Command move)
        {
            Move = move;
            Sketch = Sketches.data[move].sketch;
            Declaration = Sketches.data[move].declaration;
            Doing = Commands.commands[move];
        }

        public void Play(Player player)
        {
            Doing.Invoke(player);
        }
    }
}
