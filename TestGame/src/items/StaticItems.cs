using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.src.entities;
using TestGame.src.entities.objects.furniture;
using TestGame.src.entities.projectiles;
using TestGame.src.graphics.ui;
using TestGame.src.level;

namespace TestGame.src.items
{
    public class StaticItems
    {
        public static Item transparent = new BasicItem(0, 0, "", "");
        public static Item book1 = new BasicItem(3, 0, "Book - Coming Soon", "");
        public static Item beer = new BasicItem(0, 2, "Beer", "!drunk");

        //CLOTH
        public static Item whitejacket = new ClothItem(4, 2, "White Jacket", "textures/models/player/cloth/jacket", Color.White);
        public static Item yellowjacket = new ClothItem(4, 2, "Yellow Jacket", "textures/models/player/cloth/jacket", Color.Yellow);
        public static Item redjacket = new ClothItem(4, 2, "Red Jacket", "textures/models/player/cloth/jacket", Color.DarkRed);
        public static Item blackjacket = new ClothItem(4, 2, "Gray Jacket", "textures/models/player/cloth/jacket", Color.Gray);

        //Furnitures

        public static Item plant1 = new FurnitureItem<Plant1>(1, 0, "Beautiful Plant");
        public static Item cabinet1 = new FurnitureItem<Cabinet1>(2, 0, "Wooden Cabinet");
        public static Item sofa = new FurnitureItem<Sofa>(2, 0, "Gemütliches Sofa");
        public static Item beachchair = new FurnitureItem<Beachchair>(2, 0, "Beachchair");
        public static Item tv = new FurnitureItem<TV>(2, 2, "TV");
        public static Item lamp1 = new FurnitureItem<Latern>(1, 1, "Laterne");
        public static Item speaker = new FurnitureItem<Speaker1>(3, 1, "Speaker");
        public static Item tree = new FurnitureItem<Tree1>(2, 0, "Tree");
        public static Item palm = new FurnitureItem<Palm>(2, 0, "Palm");
        public static Item fir = new FurnitureItem<Fir>(2, 0, "Fir");


        public static Item grass = new FurnitureItem<Grass>(5, 0, "Grass");
        public static Item torch = new FurnitureItem<Torch>(0, 1, "Torch");
        public static Item magicglass = new FurnitureItem<MagicGlass>(4, 1, "Magic Glass");



        public static Item discolight = new FurnitureItem<DiscoLight>(2, 1, "RGB Licht");
        public static Item cristmasStar = new FurnitureItem<CristmasStar>(2, 1, "Stern");
        public static Item particlespawner = new FurnitureItem<Particelspawner>(3, 2, "Particle Spawner");

        public static Item fence = new FurnitureItem<Fence1>(2, 0, "Fence");
        //Mobs
        public static Item sheep = new SpawnItem<Sheep>(1, 2, "Määäh");
        //public static Item joseph = new SpawnItem<Joseph>(1, 3, "Joseph");
        //Discs
        public static Item disc_Night_Drive = new DiskItem<Song>(3, 0, "Night Drive", "sound/music/Night_Drive");
        public static Item disc_GhostBusters = new DiskItem<Song>(4, 0, "GhostBusters", "sound/music/Ghostbusters");

        //Guns
        public static Item gun1 = new GunItem<Projectile>(0, 3, new Rectangle(16, 0, 16, 16), new Rectangle(16, 16, 16, 16), "Astra 400");
        public static List<Item> Items = new List<Item>();

        public StaticItems()
        {
            Items.Add(transparent);
            Items.Add(book1);
            Items.Add(beer);
            Items.Add(whitejacket);
            Items.Add(yellowjacket);
            Items.Add(redjacket);
            Items.Add(blackjacket);

            for(int i = 0; i < Items.Count; i++)
            {
                Items[i].ID = i;
            }
        }

        public void LoadContent(ContentManager cm)
        {
            foreach(var item in Items)
            {
                item.loadContent(cm);
            }
        }

        public void Init(Level level, Inventory inv)
        {
            foreach (var item in Items)
            {
                item.init(level, inv);
            }
        }
    }
}
