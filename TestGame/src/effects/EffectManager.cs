using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.input;

namespace TestGame.src.effects
{
    public class EffectManager
    {
        public PlayerEffect alcohol = new AlcoholPlayerEffect();
        public PlayerEffect portal = new PortalEffect();
        private List<PlayerEffect> playereffects = new List<PlayerEffect>();

        public EffectManager()
        {
            playereffects.Add(alcohol);
            playereffects.Add(portal);
        }
        public void Update(Input input)
        {
            for(int i = 0; i < playereffects.Count; i++)
            {
                if(playereffects[i].duration > 0) playereffects[i].update(input);
            }          
        }

        public void removeEffects()
        {
            foreach(var effect in playereffects)
            {
                effect.duration = 0;
            }
        }
       
        public void addEffect(int id, int duration)
        {
            playereffects[id].duration = duration;                      
        }

        public void addEffect(PlayerEffect effect, int duration)
        {
            effect.duration = duration;
        }
    }
}
