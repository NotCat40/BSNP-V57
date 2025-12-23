namespace Supercell.Laser.Logic.Avatar
{
    using Newtonsoft.Json;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using System.IO;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Battle.Objects;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Security.Principal;

    public enum AllianceRole
    {
        None = 0,
        Member = 1,
        Leader = 2,
        Elder = 3,
        CoLeader = 4
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientAvatar
    {
        [JsonProperty] public long AccountId;
        [JsonProperty] public string PassToken;

        [JsonProperty] public string Name;
        [JsonProperty] public bool NameSetByUser;
        [JsonProperty] public int TutorialsCompletedCount = 2;

        [JsonIgnore] public HomeMode HomeMode;

        [JsonProperty] public int Diamonds;
        [JsonProperty] public int Gold;
        [JsonProperty] public int HeroLvlUpMaterial;
        [JsonProperty] public int LegendaryTrophies;
        [JsonProperty] public int SpraySlot;
        [JsonProperty] public int RecruitTokens;
        [JsonProperty] public int Fame;
        [JsonProperty] public int PowerPoints;
        [JsonProperty] public int Bling;
        [JsonProperty] public int CollabPoints;

        [JsonProperty] public List<Hero> Heroes;

        [JsonProperty] public int TrioWins;
        [JsonProperty] public int DuoWins;
        [JsonProperty] public int SoloWins;

        [JsonProperty] public int Tokens;
        [JsonProperty] public int StarTokens;
        [JsonProperty] public bool IsDev;
        [JsonProperty] public bool IsPremium;

        [JsonProperty] public long AllianceId;
        [JsonProperty] public AllianceRole AllianceRole;

        [JsonProperty] public DateTime LastOnline;

        [JsonProperty] public List<Friend> Friends;
        [JsonProperty] public bool Banned;

        [JsonIgnore] public int PlayerStatus;
        [JsonIgnore] public long TeamId;

        [JsonIgnore] public long BattleId;
        [JsonIgnore] public long UdpSessionId;
        [JsonIgnore] public int TeamIndex;
        [JsonIgnore] public int OwnIndex;

        [JsonProperty] public int HighestTrophies;

        [JsonProperty] public int RollsSinceGoodDrop;

        public int Trophies
        {
            get
            {
                int result = 0;
                foreach (Hero hero in Heroes.ToArray())
                {
                    result += hero.Trophies;
                }
                return result;
            }
        }

        public void AddTrophies(int t)
        {
            HighestTrophies = Math.Max(HighestTrophies, Trophies + t);
        }

        public int GetUnlockedBrawlersCountWithRarity(string rarity)
        {
            return Heroes.Count(x => x.CardData.Rarity == rarity);
        }

        public void ResetTrophies()
        {
            foreach (Hero hero in Heroes.ToArray())
            {
                hero.Trophies = 0;
                hero.HighestTrophies = 0;
            }
        }

        public int GetUnlockedHeroesCount()
        {
            return Heroes.Count;
        }

        public void UnlockHero(int characterId, int cardId)
        {
            Hero heroEntry = new Hero(characterId, cardId);
            Heroes.Add(heroEntry);
        }

        public void UpgradeHero(int characterId)
        {
            Hero heroEntry = GetHero(characterId);
            if (heroEntry.SelectedOverChargeId == 0)
            {
                CardData o = heroEntry.GetDefaultMetaForHero(6);
                if (o != null) heroEntry.SelectedOverChargeId = o.GetInstanceId();
            }
        }

        public void RemoveHero(int characterId)
        {
            Heroes.RemoveAll(x => x.CharacterId == characterId);
        }

        public bool HasHero(int characterId)
        {
            return Heroes.Find(x => x.CharacterId == characterId) != null;
        }

        public void SetEmoteForBrawler(int characterId, int slot, int pin)
        {
            Hero heroEntry = GetHero(characterId);
            heroEntry.emote[slot] = pin;
        }
        public Hero GetHero(int characterId)
        {
            return Heroes.Find(x => x.CharacterId == characterId);
        }
        public Hero GetHeroForCard(CardData cardData)
        {
            //Debugger.Print(DataTables.Get(16).GetData<CharacterData>(cardData.Target).GetInstanceId() + "");
            return GetHero(DataTables.Get(16).GetData<CharacterData>(cardData.Target).GetInstanceId() + 16000000);
        }

        public bool UseTokens(int count)
        {
            return false;
        }

        public void AddTokens(int count)
        {
            HomeMode.Home.BrawlPassTokens += count;
        }

        public void AddDiamonds(int count)
        {
            Diamonds += count;
        }

        public bool UseDiamonds(int count)
        {
            if (count > Diamonds) return false;
            Diamonds -= count;
            return true;
        }

        public bool UseHeroLvlUpMaterial(int count)
        {
            if (count > HeroLvlUpMaterial) return false;
            HeroLvlUpMaterial -= count;
            return true;
        }

        public bool UseLegendaryTrophies(int count)
        {
            if (count > LegendaryTrophies) return false;
            LegendaryTrophies -= count;
            return true;
        }

        public bool UseSpraySlot(int count)
        {
            if (count > SpraySlot) return false;
            SpraySlot -= count;
            return true;
        }

        public bool UseRecruitTokens(int count)
        {
            if (count > RecruitTokens) return false;
            RecruitTokens -= count;
            return true;
        }

        public bool UseFame(int count)
        {
            if (count > Fame) return false;
            Fame -= count;
            return true;
        }

        public bool UsePowerPoints(int count)
        {
            if (count > PowerPoints) return false;
            PowerPoints -= count;
            return true;
        }

        public bool UseBling(int count)
        {
            if (count > Bling) return false;
            Bling -= count;
            return true;
        }

        public bool UseCollabPoints(int count)
        {
            if (count > CollabPoints) return false;
            CollabPoints -= count;
            return true;
        }

        public void AddHeroLvlUpMaterial(int count)
        {
            HeroLvlUpMaterial += count;
        }

        public void AddLegendaryTrophies(int count)
        {
            LegendaryTrophies += count;
        }

        public void AddSpraySlot(int count)
        {
            SpraySlot += count;
        }

        public void AddRecruitTokens(int count)
        {
            RecruitTokens += count;
        }

        public void AddFame(int count)
        {
            Fame += count;
        }

        public void AddPowerPoints(int count)
        {
            PowerPoints += count;
        }

        public void AddBling(int count)
        {
            Bling += count;
        }

        public void AddCollabPoints(int count)
        {
            CollabPoints += count;
        }

        public void RevokeDiamonds(int count)
        {
            Diamonds -= count;
        }

        public void RevokeHeroLvlUpMaterial(int count)
        {
            HeroLvlUpMaterial -= count;
        }

        public void RevokeLegendaryTrophies(int count)
        {
            LegendaryTrophies -= count;
        }

        public void RevokeSpraySlot(int count)
        {
            SpraySlot -= count;
        }

        public void RevokeRecruitTokens(int count)
        {
            RecruitTokens -= count;
        }

        public void RevokeFame(int count)
        {
            Fame -= count;
        }

        public void RevokePowerPoints(int count)
        {
            PowerPoints -= count;
        }

        public void RevokeBling(int count)
        {
            Bling -= count;
        }

        public void RevokeCollabPoints(int count)
        {
            CollabPoints -= count;
        }

        public ClientAvatar()
        {
            Name = "Brawler";

            HeroLvlUpMaterial = 100;
            Diamonds = 0;

            Heroes = new List<Hero>();

            IsDev = false;
            IsPremium = false;

            AllianceRole = AllianceRole.None;
            AllianceId = -1;

            LastOnline = DateTime.UtcNow;
            Friends = new List<Friend>();
        }

        public void SkipTutorial()
        {
            TutorialsCompletedCount = 2;
        }

        public bool IsTutorialState()
        {
            return TutorialsCompletedCount < 2;
        }

        public Friend GetRequestFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id && friend.FriendState != 4);
        }

        public Friend GetAcceptedFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id && friend.FriendState == 4);
        }
        public EmoteData GetDefaultEmoteForCharacter(string Character, string Type)
        {
            foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
            {
                if (emoteData.Character == Character && emoteData.EmoteType == Type) return emoteData;
            }
            return null;
        }
        public Friend GetFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id);
        }
        public void Refresh()
        {

        }
        public int Checksum
        {
            get
            {
                ChecksumEncoder Stream = new ChecksumEncoder();

                return Stream.GetCheckSum();
            }
        }
        //64 vint 36 int 32 boolean
        //124 VInt 108 Int 104 Boolean
        public void Encode(ByteStream Stream)
        {
            Stream.WriteVLong(AccountId);
            Stream.WriteVLong(AccountId);
            Stream.WriteVLong(AccountId);
            Stream.WriteString(Name);
            Stream.WriteBoolean(NameSetByUser);
            Stream.WriteInt(-1);

            Stream.WriteVInt(17);
            {
                Stream.WriteVInt(25 + Heroes.Count);
                {
                    foreach (Hero hero in Heroes)
                    {
                        ByteStreamHelper.WriteDataReference(Stream, hero.CardData);
                        Stream.WriteVInt(-1);
                        Stream.WriteVInt(1);
                    }

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 0));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 1));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 2));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 3));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 4));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 5));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 6));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 7));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 8));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(HeroLvlUpMaterial);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 9));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 10));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(LegendaryTrophies);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 11));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 12));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 13));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 14));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 15));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 16));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 17));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 18));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(SpraySlot + 5);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 19));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(RecruitTokens);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 20));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 21));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(Fame);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 22));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(PowerPoints);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 23));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(Bling);

                    ByteStreamHelper.WriteDataReference(Stream, GlobalId.CreateGlobalId(5, 24));
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(CollabPoints);
                }

                Stream.WriteVInt(Heroes.Count);//2
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(hero.Trophies);
                }

                Stream.WriteVInt(Heroes.Count);//3
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(hero.HighestTrophies);
                }

                Stream.WriteVInt(Heroes.Count);//4
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(RandomNumberGenerator.GetInt32(4));
                }

                Stream.WriteVInt(Heroes.Count);//5
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(hero.PowerPoints);
                }

                Stream.WriteVInt(Heroes.Count);//6
                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(Stream, hero.CharacterData);
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(hero.PowerLevel);
                    //Stream.WriteVInt(1);
                }

                List<int> spgs = new List<int>();
                foreach (CardData carddata in DataTables.Get(DataType.Card).GetDatas())
                {
                    if (carddata.MetaType == 4 || carddata.MetaType == 5 || carddata.MetaType == 6)
                    {
                        spgs.Add(carddata.GetInstanceId());
                    }
                }
                List<int> spgsChosen = new List<int>();
                foreach (Hero hero in Heroes)
                {
                    spgsChosen.Add(hero.SelectedGadgetId);
                    spgsChosen.Add(hero.SelectedStarPowerId);
                    spgsChosen.Add(hero.SelectedOverChargeId);
                }
                Stream.WriteVInt(spgs.Count);//7
                for (int i = 0; i < spgs.Count; i++)
                {
                    ByteStreamHelper.WriteDataReference(Stream, 23000000 + spgs[i]);
                    Stream.WriteVInt(-1);
                    Stream.WriteVInt(0);//0 lock 1 unlock 2 chosen
                }

                Stream.WriteVInt(0); // HeroSeenState

                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array
                Stream.WriteVInt(0); // Array   
            }

            Stream.WriteVInt(Diamonds); // Diamonds
            Stream.WriteVInt(Diamonds); // Free Diamonds
            Stream.WriteVInt(10); // Player Level
            Stream.WriteVInt(100);
            Stream.WriteVInt(0); // CumulativePurchasedDiamonds or Avatar User Level Tier | 10000 < Level Tier = 3 | 1000 < Level Tier = 2 | 0 < Level Tier = 1
            Stream.WriteVInt(100); // Battle Count
            Stream.WriteVInt(10); // WinCount
            Stream.WriteVInt(80); // LoseCount
            Stream.WriteVInt(50); // WinLooseStreak
            Stream.WriteVInt(20); // NpcWinCount
            Stream.WriteVInt(0); // NpcLoseCount
            Stream.WriteVInt(2); // TutorialState | shouldGoToFirstTutorialBattle = State == 0
            Stream.WriteVInt(12);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteString("");
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(1);
            Console.WriteLine("[DEBUG] [ClientAvatar] All Modules loaded!");
        }
    }

}
