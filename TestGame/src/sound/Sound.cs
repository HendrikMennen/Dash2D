using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.level;

namespace TestGame.src.sound
{
    public class Sound
    {
        private List<SoundEffectInstance> sounds = new List<SoundEffectInstance>();
        private Level level;

        public float MasterVolume
        {
            get
            {
                return SoundEffect.MasterVolume;
            }
            set
            {
                SoundEffect.MasterVolume = value;
            }
        }

        public void Init(Level level)
        {
            this.level = level;

            SoundEffect.DistanceScale = 2.0f;
        }

        public void Play(SoundEffect se)
        {
            if(se != null) se.Play();
        }

        public void Play(SoundEffect se, float volume)
        {
            if (se != null) se.Play(volume, 0f, 0f);
        }

        public void update()
        {           
            sounds.RemoveAll(x => x.State == SoundState.Stopped);
        }

        public void Play(SoundEffect se, Vector2 position)
        {
            if (se == null) return;
            SoundEffectInstance inst = se.CreateInstance();

            AudioEmitter ae = new AudioEmitter();
            ae.Position = new Vector3(position.X, position.Y, 0);
            
            AudioListener al = new AudioListener();
            al.Position = new Vector3(level.getClientPlayer().Position.X, level.getClientPlayer().Position.Y, 0);

            inst.Apply3D(al, ae);
            inst.Play();

            sounds.Add(inst);
        }
      
    }
}
