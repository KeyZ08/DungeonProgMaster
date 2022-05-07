using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonProgMaster
{
    class Map
    {
        private Player reservePlayer;
        public Player player;
        public readonly int[,] map;

        public Map(Player player, int[,] map)
        {
            this.player = player;
            reservePlayer = new Player(player.position, player.movement);
            this.map = map;
        }

        public void Reset(Sizer sizer)
        {
            player = new Player(reservePlayer.position, reservePlayer.movement, player);
            player.SetWorldPositionAndSize(sizer);
        }
    }
}
