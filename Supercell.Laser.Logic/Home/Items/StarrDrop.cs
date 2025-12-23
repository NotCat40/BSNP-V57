using Supercell.Laser.Logic.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using Supercell.Laser.Titan.DataStream;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Data;
using Supercell.Laser.Logic.Data;
using Supercell.Laser.Logic.Home.Gatcha;
using Supercell.Laser.Logic.Message.Home;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Reflection;
using Supercell.Laser.Logic.Home.Items;

namespace StarrDrop
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Possibility
    {
        public int Type { get; set; }
        public int Ammount { get; set; }
        public int DataGlobalID { get; set; }
        public int SkinGlobalID { get; set; }
    }


    public class StarrDrop
    {
        public Random random = new Random();
        public static int GetRandomSuperRareSkin()
        {
            List<int> globalIds = new List<int>();

            foreach (SkinData emoteData in DataTables.Get(DataType.Skin).GetDatas())
            {
                if (emoteData.PriceGems == 79)
                {
                    globalIds.Add(DataTables.Get(DataType.Skin).GetData<SkinData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];
            }

            return -1;
        }

        public static int GetRandomEpicSkin()
        {
            List<int> globalIds = new List<int>();

            foreach (SkinData emoteData in DataTables.Get(DataType.Skin).GetDatas())
            {
                if (emoteData.PriceGems == 149)
                {
                    globalIds.Add(DataTables.Get(DataType.Skin).GetData<SkinData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];
            }

            return -1;
        }

        public static int GetRandomRareSkin()
        {
            List<int> globalIds = new List<int>();

            foreach (SkinData emoteData in DataTables.Get(DataType.Skin).GetDatas())
            {
                if (emoteData.PriceGems == 29)
                {
                    globalIds.Add(DataTables.Get(DataType.Skin).GetData<SkinData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];
            }

            return -1;
        }

        public static int GetRandomDefPin()
        {
            List<int> globalIds = new List<int>();

            foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
            {
                if (emoteData.Rarity == "COMMON" && emoteData.Skin == null)
                {
                    globalIds.Add(DataTables.Get(DataType.Emote).GetData<EmoteData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];
            }

            return -1;
        }

        public static int GetRandomRarefPin()
        {
            List<int> globalIds = new List<int>();

            foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
            {
                if (emoteData.Rarity == "RARE" && emoteData.Skin == null)
                {
                    globalIds.Add(DataTables.Get(DataType.Emote).GetData<EmoteData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];
            }

            return -1;
        }

        public static int GetRandomEpicPin()
        {
            List<int> globalIds = new List<int>();

            foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
            {
                if (emoteData.Rarity == "EPIC" && emoteData.Skin == null)
                {
                    globalIds.Add(DataTables.Get(DataType.Emote).GetData<EmoteData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];
            }

            return -1;
        }

        public static int GetRandomThumbnail()
        {
            List<int> globalIds = new List<int>();

            foreach (PlayerThumbnailData emoteData in DataTables.Get(DataType.PlayerThumbnail).GetDatas())
            {
                if (emoteData.IsAvailableForOffers && emoteData.LockedForChronos == null)
                {
                    globalIds.Add(DataTables.Get(DataType.PlayerThumbnail).GetData<PlayerThumbnailData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];

            }

            return -1;
        }

        public static int GetRandomBrawlerByRarity(string rare)
        {
            List<int> globalIds = new List<int>();

            foreach (CharacterData emoteData in DataTables.Get(DataType.Character).GetDatas())
            {
                CardData card = DataTables.Get(DataType.Card).GetData<CardData>(emoteData.Name + "_unlock");
                if (card!=null  && card.Rarity == rare && !emoteData.Disabled)
                {
                    globalIds.Add(DataTables.Get(DataType.Character).GetData<CharacterData>(emoteData.Name).GetGlobalId());
                }
            }

            if (globalIds.Count > 0)
            {
                Random random = new Random();
                return globalIds[random.Next(globalIds.Count)];

            }

            return -1;
        }

        public int Rarity;

        /* Explanation of the rarity of stardrops
         * 0 - Rare
         * 1 - SuperRare
         * 2 - Epic
         * 3 - Mythic
         * 4 - Legendary
         * 5 - Hypercharged
        */

        public Possibility data = new Possibility();
        /* Possibility - crate with drop data
         *  меня в дестве били 
        */

        
        public StarrDrop(HomeMode mode)
        {
            if (mode == null) return;
            GenerateDrop(mode);
        }

        public void GenerateDropByRarity(HomeMode init, int rarity)
        {
            ClientHome home = init.Home;
            if (home.StarrDrop == null) home.StarrDrop = new StarrDrop(init);

            if (rarity == 0)
            {
                home.StarrDrop.Rarity = 0;
                GenerateRareDrop(init);
            }
            else if (rarity == 1)
            {
                home.StarrDrop.Rarity = 1;
                GenerateSuperRareDrop(init);
            }
            else if (rarity == 2)
            {
                home.StarrDrop.Rarity = 2;
                GenerateEpicDrop(init);
            }
            else if (rarity == 3)
            {
                home.StarrDrop.Rarity = 3;
                GenerateMythicDrop(init);
            }
            else if (rarity == 4)
            {
                home.StarrDrop.Rarity = 4;
                GenerateLegendaryDrop(init);
            }
        }
        public void GenerateDrop(HomeMode init)
        {
            ClientHome home = init.Home;
            if (home.StarrDrop == null) home.StarrDrop = new StarrDrop(init);

            double chance = random.NextDouble() * 100;

            if (chance < 50)
            {
                home.StarrDrop.Rarity = 0;
                GenerateRareDrop(init);
            }
            else if (chance < 75)
            {
                home.StarrDrop.Rarity = 1;
                GenerateSuperRareDrop(init);
            }
            else if (chance < 90)
            {
                home.StarrDrop.Rarity = 2;
                GenerateEpicDrop(init);
            }
            else if (chance < 96)
            {
                home.StarrDrop.Rarity = 3;
                GenerateMythicDrop(init);
            }
            else
            {
                home.StarrDrop.Rarity = 4;
                GenerateLegendaryDrop(init);
            }
        }
        public void GenerateRareDrop(HomeMode init)
        {
            ClientHome home = init.Home;

            double chance = random.NextDouble() * 100;

            string result;
            if (chance < 40)
                result = "Coins";
            else if (chance < 70)
                result = "PowerPoints";
            else if (chance < 85)
                result = "TokenDoubler";
            else if (chance < 95)
                result = "Credits";
            else
                result = "Bling";

            switch (result)
            {
                case "Coins":
                    home.StarrDrop.data.Type = 7;
                    home.StarrDrop.data.Ammount = 50;
                    break;
                case "PowerPoints":
                    home.StarrDrop.data.Type = 24;
                    home.StarrDrop.data.Ammount = 25;
                    break;
                case "TokenDoubler":
                    home.StarrDrop.data.Type = 2; 
                    home.StarrDrop.data.Ammount = random.Next(100, 150);
                    break;
                case "Credits":
                    home.StarrDrop.data.Type = 22;
                    home.StarrDrop.data.Ammount = 10;
                    break;
                case "Bling":
                    home.StarrDrop.data.Type = 25;
                    home.StarrDrop.data.Ammount = 100;
                    break;
            }


            //home.StarrDrop.data.Type = 8;
            //home.StarrDrop.data.Ammount = 11;
            //home.StarrDrop.data.SkinGlobalID = 0;
            //home.StarrDrop.data.DataGlobalID = 0;
        }

        public void GenerateSuperRareDrop(HomeMode init)
        {
            ClientHome home = init.Home;

            double chance = random.NextDouble() * 100;

            string result;
            if (chance < 40)
                result = "Coins";
            else if (chance < 70)
                result = "PowerPoints";
            else if (chance < 85)
                result = "TokenDoubler";
            else if (chance < 95)
                result = "Credits";
            else
                result = "Bling";

            switch (result)
            {
                case "Coins":
                    home.StarrDrop.data.Type = 7;
                    home.StarrDrop.data.Ammount = 100;
                    break;
                case "PowerPoints":
                    home.StarrDrop.data.Type = 24;
                    home.StarrDrop.data.Ammount = 50;
                    break;
                case "TokenDoubler":
                    home.StarrDrop.data.Type = 2;
                    home.StarrDrop.data.Ammount = random.Next(190, 250);
                    break;
                case "Credits":
                    home.StarrDrop.data.Type = 22;
                    home.StarrDrop.data.Ammount = 30;
                    break;
                case "Bling":
                    home.StarrDrop.data.Type = 25;
                    home.StarrDrop.data.Ammount = 50;
                    break;
            }
        }

        public void GenerateEpicDrop(HomeMode init)
        {
            ClientHome home = init.Home;

            double chance = random.NextDouble() * 100; 

            string result;
            if (chance < 30)
                result = "Coins";        
            else if (chance < 50)
                result = "TokenDoubler"; 
            else if (chance < 70)
                result = "PowerPoints";   
            else if (chance < 75)
                result = "Skin30";      
            else if (chance < 79)
                result = "Pin19";     
            else if (chance < 89)
                result = "Pin9";        
            else if (chance < 91)
                result = "RareBrawler";  
            else
                result = "Skin79";  

            switch (result)
            {
                case "Coins":
                    home.StarrDrop.data.Type = 7;
                    home.StarrDrop.data.Ammount = 200;
                    break;
                case "PowerPoints":
                    home.StarrDrop.data.Type = 24;
                    home.StarrDrop.data.Ammount = 100;
                    break;
                case "TokenDoubler":
                    home.StarrDrop.data.Type = 2;
                    home.StarrDrop.data.Ammount = random.Next(500, 575);
                    break;
                case "Skin30":
                    if (home.UnlockedSkins.Contains(GetRandomRareSkin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 250;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 9;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.SkinGlobalID = GetRandomRareSkin();
                    }
                    break;
                case "Pin19":
                    if (home.UnlockedEmotes.Contains(GetRandomRarefPin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 100;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomRarefPin();
                    }
                    break;
                case "Pin9":
                    if (home.UnlockedEmotes.Contains(GetRandomDefPin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 100;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomDefPin();
                    }
                    break;
                case "RareBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("rare")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 1000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("rare");
                    }
                    break;
                case "Skin79":
                    if (home.UnlockedSkins.Contains(GetRandomSuperRareSkin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 500;
                    }
                    else {
                        home.StarrDrop.data.Type = 9;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.SkinGlobalID = GetRandomSuperRareSkin();
                    }

                    break;
            }
        }

        public void GenerateMythicDrop(HomeMode init)
        {
            ClientHome home = init.Home;

            double chance = random.NextDouble() * 100;

            string result;
            if (chance < 35)
                result = "Coins";
            else if (chance < 45)
                result = "PowerPoints";
            else if (chance < 55)
                result = "Pin9";
            else if (chance < 60)
                result = "Pin19"; 
            else if (chance < 65)
                result = "Pin29";
            else if (chance < 70)
                result = "Thumbnail";
            else if (chance < 75)
                result = "SuperRareBrawler";
            else if (chance < 80)
                result = "Skin79";
            else if (chance < 83)
                result = "RareBrawler";
            else if (chance < 86)
                result = "EpicBrawler";
            else if (chance < 90)
                result = "MythicBrawler";
            else
                result = "Skin30";

            switch (result)
            {
                case "Coins":
                    home.StarrDrop.data.Type = 7;
                    home.StarrDrop.data.Ammount = 500;
                    break;
                case "PowerPoints":
                    home.StarrDrop.data.Type = 24;
                    home.StarrDrop.data.Ammount = 200;
                    break;
                case "Skin30":
                    if (home.UnlockedSkins.Contains(GetRandomRareSkin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 250;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 9;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.SkinGlobalID = GetRandomRareSkin();
                    }
                    break;
                case "Pin19":
                    if (home.UnlockedEmotes.Contains(GetRandomRarefPin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 250;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomRarefPin();
                    }
                    break;
                case "Pin9":
                    if (home.UnlockedEmotes.Contains(GetRandomDefPin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 100;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomDefPin();
                    }
                    break;
                case "Pin29":
                    if (home.UnlockedEmotes.Contains(GetRandomEpicPin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 350;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomEpicPin();
                    }
                    break;
                case "RareBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("rare")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 1000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("rare");
                    }
                    break;
                case "SuperRareBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("super_rare")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 1000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("super_rare");
                    }
                    break;
                case "EpicBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("epic")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 1000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("epic");
                    }
                    break;
                case "MythicBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("mega_epic")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 1000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("mega_epic");
                    }
                    break;
                case "Skin79":
                    if (home.UnlockedSkins.Contains(GetRandomSuperRareSkin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 500;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 9;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.SkinGlobalID = GetRandomSuperRareSkin();
                    }

                    break;
                case "Thumbnail":
                    if (home.UnlockedThumbnails.Contains(GetRandomThumbnail()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 250;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomThumbnail();
                    }

                    break;
            }
        }
        public void GenerateLegendaryDrop(HomeMode init)
        {
            ClientHome home = init.Home;

            double chance = random.NextDouble() * 100;

            string result;
            //if (chance < 35)
                
               // result = "Gadget";TODO
           // else if (chance < 45)
               // result = "Starpower"; TODO
            if (chance < 45)
                result = "Pin29";
            else if (chance < 55)
                result = "Skin79";
            else if (chance < 65)
                result = "Skin149";
            else if (chance < 75)
                result = "EpicBrawler";
            else if (chance < 80)
                result = "MythicBrawler";
            else if (chance < 90)
                result = "LegendaryBrawler";
            else
                result = "Thumbnail";

            switch (result)
            {
                case "Gadget":
                    break;
                case "Starpower":
                    break;
                case "Pin29":
                    if (home.UnlockedEmotes.Contains(GetRandomEpicPin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 350;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomEpicPin();
                    }
                    break;
                case "EpicBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("epic")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 2000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("epic");
                    }
                    break;
                case "MythicBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("mega_epic")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 3000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("mega_epic");
                    }
                    break;
                case "LegendaryBrawler":
                    if (init.Avatar.GetHero(GetRandomBrawlerByRarity("legendary")) != null)
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 4000;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 1;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomBrawlerByRarity("legendary");
                    }
                    break;
                case "Skin79":
                    if (home.UnlockedSkins.Contains(GetRandomSuperRareSkin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 500;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 9;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.SkinGlobalID = GetRandomSuperRareSkin();
                    }

                    break;
                case "Skin149":
                    if (home.UnlockedSkins.Contains(GetRandomEpicSkin()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 2350;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 9;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.SkinGlobalID = GetRandomEpicSkin();
                    }

                    break;
                case "Thumbnail":
                    if (home.UnlockedThumbnails.Contains(GetRandomThumbnail()))
                    {
                        home.StarrDrop.data.Type = 25;
                        home.StarrDrop.data.Ammount = 250;
                    }
                    else
                    {
                        home.StarrDrop.data.Type = 11;
                        home.StarrDrop.data.Ammount = 1;
                        home.StarrDrop.data.DataGlobalID = GetRandomThumbnail();
                    }

                    break;
            }
        }

        public void Encode(ByteStream byteStream, HomeMode player)
        {
            byteStream.WriteVInt(5);
            for (int i = 0; i < 5; i++)
            {
                byteStream.WriteDataReference(80, i);
                byteStream.WriteVInt(-1);
                byteStream.WriteVInt(0);
            }

            byteStream.WriteVInt(0);

            byteStream.WriteInt(-1788180018);
            byteStream.WriteVInt(0); // Progress in Battle
            byteStream.WriteVInt(0);
            byteStream.WriteVInt(0); // timer
            byteStream.WriteVInt(0);

            byteStream.WriteVInt(0);
            byteStream.WriteVInt(0);
            byteStream.WriteVInt(0);
            byteStream.WriteVInt(0);
            byteStream.WriteVInt(0);
            byteStream.WriteBoolean(false);
        }
    }
}
