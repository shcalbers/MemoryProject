﻿using MemoryProjectFull;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewMemoryGame{
    class TurnManager{

        NetworkCommand nxtTrnCmd;
        private List<Player> players;
        private GamePanel gamepanel;

        public TurnManager(Player[] _players, GamePanel _gamepanel){
            gamepanel = _gamepanel;
            gamepanel.Deactivate();

            players = _players.ToList(); // <-- first i do to array and then to list? might need fix?
            nxtTrnCmd = new NetworkCommand("G:NTURN", Turn, false, true);

            if (NetworkHandler.getInstance().isHost()) { // <-- what if we have dedicated server?
                nxtTrnCmd.send(_players[0].ID.ToString());
            }
        }

        private void Turn(string[] _data){
            Console.WriteLine("NEXT TUNR!!!!");
            if (int.Parse(_data[0]) == NetworkHandler.getInstance().networkID){
                gamepanel.Activate();
                gamepanel.onClickDone += new EventHandler<GamePanel.OnClickDoneArgs>(EndTurn);
                Console.WriteLine("YOU CAN MOVE!");
            } else {
                gamepanel.Deactivate();
                gamepanel.onClickDone -= new EventHandler<GamePanel.OnClickDoneArgs>(EndTurn);
                Console.WriteLine("YOU CAN'T MOVE!");
            } 
        }

        private void EndTurn(Object _sender, GamePanel.OnClickDoneArgs _onClickDoneArgs){
            Console.WriteLine("END TUNR!!!!");
            nxtTrnCmd.send(players.Find(x => x.ID == NetworkHandler.getInstance().networkID).nextID.ToString());
        }

        public List<Player> getPlayers()
        {
            return players;
        }
    }
}