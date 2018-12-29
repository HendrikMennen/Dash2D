
using System;
using TestGame.src.tools;
using System.Runtime.Serialization;

namespace TestGame.src.entities
{
    [DataContract]
    public class Npc : Mob
         
    {
        
        private int range, wait, time;
        public void moveRandom()
        {
            if (range <= 0)
            {
                range = Global.rdm.Next(100);
                MoveTo = new Microsoft.Xna.Framework.Vector2(Position.X + Global.rdm.Next(-1, 2) * range, Position.Y + Global.rdm.Next(-1, 2) * range);
                if (Global.rdm.Next(0, 2) == 1)
                {
                    MoveTo = new Microsoft.Xna.Framework.Vector2(MoveTo.X, Position.Y);
                }
                else MoveTo = new Microsoft.Xna.Framework.Vector2(Position.X, MoveTo.Y);
            }
            if (wait < 0)
            {
                time++;
                walking = false;
                if (time == 60)
                {
                    walking = true;
                    wait = Global.rdm.Next(120);
                    time = 0;
                }
            }
            wait--;
            range--;            
        }              
    }
}
