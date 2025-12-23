using System.Linq;

namespace Supercell.Laser.Logic.Home
{
    using System;
    using System.Security.Cryptography;
    using Newtonsoft.Json;
    using System.Numerics;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using System.Text;
    using System.Reflection.Metadata;
    using System.Formats.Asn1;
    using System.Globalization;
    using System.Collections;

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientHome
    {
        public const int DAILYOFFERS_COUNT = 6;

        public static readonly int[] GoldPacksPrice = new int[]
        {
            20, 50, 140, 280
        };

        public static readonly int[] GoldPacksAmount = new int[]
        {
            150, 400, 1200, 2600
        };

        [JsonProperty] public long HomeId;
        [JsonProperty] public int ThumbnailId;
        [JsonProperty] public int NameColorId;
        [JsonProperty] public int[] CharacterIds;
        [JsonProperty] public int FavouriteCharacter;
        public int CharacterId => CharacterIds[0];

        [JsonIgnore] public List<OfferBundle> OfferBundles;

        [JsonProperty] public int TrophiesReward;
        [JsonProperty] public int TokenReward;
        [JsonProperty] public int StarTokenReward;

        [JsonProperty] public BigInteger BrawlPassProgress;
        [JsonProperty] public BigInteger PremiumPassProgress;
        [JsonProperty] public BigInteger BrawlPassPlusProgress;
        [JsonProperty] public int BrawlPassTokens;
        [JsonProperty] public bool HasPremiumPass;
        [JsonProperty] public bool HasPremiumPassPlus;
        [JsonProperty] public List<int> UnlockedEmotes;
        [JsonProperty] public List<int> UnlockedThumbnails;
        [JsonProperty] public List<int> UnlockedSprays;
        [JsonProperty] public List<int> UnlockedTituls;
        [JsonProperty] public List<int> UnlockedFrames;
        [JsonProperty] public int[] UnlockStarRoad;
        [JsonProperty] public NotificationFactory NotificationFactory;
        [JsonProperty] public List<int> UnlockedSkins;

        [JsonProperty] public int TrophyRoadProgress;
        [JsonIgnore] public Quests Quests;
        [JsonProperty] public int EventId;
        [JsonProperty] public List<PlayerMap> PlayerMaps = new List<PlayerMap>();

        [JsonProperty] public BattleCard DefaultBattleCard;
        [JsonIgnore] public EventData[] Events;

        [JsonProperty] public int PreferredThemeId;

        [JsonProperty] public int RecruitTokens;
        [JsonProperty] public int RecruitBrawler;
        [JsonProperty] public int RecruitBrawlerCard;
        [JsonProperty] public int RecruitGemsCost;
        [JsonProperty] public int RecruitCost;
        [JsonProperty] public int ChromaticCoins; // after v52 - shit bcs not uset yet

        [JsonProperty] public List<string> OffersClaimed;

        [JsonProperty] public Dictionary<int, int> PlayerSelectedEmotes = new Dictionary<int, int>();


        [JsonProperty] public List<int> Brawlers;
        [JsonIgnore] public StarrDrop.StarrDrop StarrDrop;

        public PlayerThumbnailData Thumbnail => DataTables.Get(DataType.PlayerThumbnail).GetDataByGlobalId<PlayerThumbnailData>(ThumbnailId);
        public NameColorData NameColor => DataTables.Get(DataType.NameColor).GetDataByGlobalId<NameColorData>(NameColorId);

        public HomeMode HomeMode;

        [JsonProperty] public DateTime LastVisitHomeTime;

        public ClientHome()
        {
            Brawlers = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 34, 36, 37, 40, 42, 43, 45, 47, 48, 50, 52, 58, 61, 63, 64, 67, 69, 71, 73 };
            PlayerSelectedEmotes.Add(1, 28);
            PlayerSelectedEmotes.Add(2, 28);
            PlayerSelectedEmotes.Add(3, 28);
            PlayerSelectedEmotes.Add(4, 28);
            PlayerSelectedEmotes.Add(5, 94 - 3);
            RecruitBrawler = 1;
            RecruitBrawlerCard = 4;
            RecruitCost = 160;
            RecruitGemsCost = 29;
            RecruitTokens = 0;
            OffersClaimed = new List<string>();

            ThumbnailId = GlobalId.CreateGlobalId(28, 0);
            NameColorId = GlobalId.CreateGlobalId(43, 0);
            CharacterIds = new int[] { GlobalId.CreateGlobalId(16, 0), GlobalId.CreateGlobalId(16, 1), GlobalId.CreateGlobalId(16, 2) };
            FavouriteCharacter = GlobalId.CreateGlobalId(16, 0);

            OfferBundles = new List<OfferBundle>();
            UnlockedSkins = new List<int>();
            UnlockedEmotes = new List<int>();
            UnlockedSprays = new List<int>();
            UnlockedFrames = new List<int>();
            UnlockedTituls = new List<int>();
            UnlockedThumbnails = new List<int>();
            LastVisitHomeTime = DateTime.UnixEpoch;

            TrophyRoadProgress = 1;

            BrawlPassProgress = 1;
            PremiumPassProgress = 1;
            EventId = 1;
            UnlockedEmotes = new List<int>();
            DefaultBattleCard = new BattleCard();
            StarrDrop = new StarrDrop.StarrDrop(HomeMode);

            if (NotificationFactory == null) NotificationFactory = new NotificationFactory();

            PreferredThemeId = -1;
        }

        public int TimerMath(DateTime timer_start, DateTime timer_end)
        {
            {
                DateTime timer_now = DateTime.Now;
                if (timer_now > timer_start)
                {
                    if (timer_now < timer_end)
                    {
                        int time_sec = (int)(timer_end - timer_now).TotalSeconds;
                        return time_sec;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        public void HomeVisited()
        {


            LastVisitHomeTime = DateTime.UtcNow;
            UpdateOfferBundles();

            if (Quests == null && TrophyRoadProgress >= 11)
            {
                Quests = new Quests();
                Quests.AddRandomQuests(HomeMode.Avatar.Heroes, 8);
            }
            else if (Quests != null)
            {
                if (Quests.QuestList.Count < 8) // New quests adds at 07:00 AM UTC
                {
                    Quests.AddRandomQuests(HomeMode.Avatar.Heroes, 8 - Quests.QuestList.Count);
                }
            }
        }

        public void Tick()
        {
            LastVisitHomeTime = DateTime.UtcNow;
            TokenReward = 0;
            TrophiesReward = 0;
            StarTokenReward = 0;
        }

        public void PurchaseOffer(int index)
        {
            if (index < 0 || index >= OfferBundles.Count) return;

            OfferBundle bundle = OfferBundles[index];
            bundle.Purchased = true;

            // обработка покупки не готова

            if (bundle.Claim != "debug") { OffersClaimed.Add(bundle.Claim); }

            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            Random rand = new Random();
            foreach (Offer offer in bundle.Items)
            {
                switch (offer.Type)
                {
                    case ShopItem.GuaranteedHero:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(1);
                            reward.DataGlobalId = offer.ItemDataId;
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.Item:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(4);
                            reward.CardGlobalId = GlobalId.CreateGlobalId(23, offer.SkinDataId);
                            reward.Count = 1;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.UpgradeMaterial:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(7);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.Gems:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(8);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.Skin:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(9);
                            reward.SkinGlobalId = GlobalId.CreateGlobalId(29, offer.SkinDataId);
                            reward.Count = 1;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                            NewCommand(DataTables.Get(DataType.Skin).GetDataByGlobalId<SkinData>(offer.SkinDataId), command);
                        }
                        break;

                    case ShopItem.Emote:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(11);
                            reward.VanityGlobalId = GlobalId.CreateGlobalId(52, offer.SkinDataId);
                            reward.Count = 1;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.PlayerThumbnail:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(11);
                            reward.VanityGlobalId = GlobalId.CreateGlobalId(28, offer.SkinDataId);
                            reward.Count = 1;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.Spray:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(11);
                            reward.VanityGlobalId = GlobalId.CreateGlobalId(68, offer.SkinDataId);
                            reward.Count = 1;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.StarPoints:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(12);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.RecruitToken:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(22);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.ChromaticToken:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(23);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.PowerPoints:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(24);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.Bling:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(25);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.ArchetypeItem:
                        {
                            int randomId = rand.Next(779, 800);
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(4);
                            reward.CardGlobalId = GlobalId.CreateGlobalId(23, randomId);
                            reward.Count = 1;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                    case ShopItem.CollabPoints:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(34);
                            reward.Count = offer.Count;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;
                    default:
                        {
                            DeliveryUnit unit = new DeliveryUnit(100);
                            GatchaDrop reward = new GatchaDrop(31);
                            reward.Count = 1;
                            unit.AddDrop(reward);
                            command.DeliveryUnits.Add(unit);
                        }
                        break;

                }

            }
            command.Execute(HomeMode);
            AvailableServerCommandMessage message = new AvailableServerCommandMessage();
            message.Command = command;
            HomeMode.GameListener.SendMessage(message);

            void NewCommand(SkinData skinData, LogicGiveDeliveryItemsCommand command)
            {
                Console.WriteLine($"Processing skin: {skinData?.Name}");
                if (skinData == null) return;

                DeliveryUnit unit = new DeliveryUnit(100);
                bool hasVanityItems = false;
                HashSet<int> addedVanityIds = new HashSet<int>();

                var emoteDatas = DataTables.Get(DataType.Emote).GetDatas();
                if (emoteDatas != null)
                {
                    foreach (EmoteData emoteData in emoteDatas)
                    {
                        if (emoteData != null && emoteData.Skin == skinData.Name)
                        {
                            try
                            {
                                int vanityId = emoteData.GetGlobalId();

                                if (vanityId > 0 && !addedVanityIds.Contains(vanityId))
                                {
                                    hasVanityItems = true;
                                    addedVanityIds.Add(vanityId);

                                    GatchaDrop reward1 = new GatchaDrop(11);
                                    reward1.VanityGlobalId = vanityId;
                                    reward1.Count = 1;
                                    unit.AddDrop(reward1);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error processing emote {emoteData.Name}: {ex.Message}");
                            }
                        }
                    }
                }

                var thumbnailDatas = DataTables.Get(DataType.PlayerThumbnail).GetDatas();
                if (thumbnailDatas != null)
                {
                    foreach (PlayerThumbnailData playerThumbnailData in thumbnailDatas)
                    {
                        if (playerThumbnailData != null && playerThumbnailData.CatalogPreRequirementSkin == skinData.Name)
                        {
                            try
                            {
                                int vanityId = playerThumbnailData.GetGlobalId();

                                if (vanityId > 0 && !addedVanityIds.Contains(vanityId))
                                {
                                    hasVanityItems = true;
                                    addedVanityIds.Add(vanityId);

                                    GatchaDrop reward1 = new GatchaDrop(11);
                                    reward1.VanityGlobalId = vanityId;
                                    reward1.Count = 1;
                                    unit.AddDrop(reward1);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Логирование ошибки
                                Console.WriteLine($"Error processing thumbnail {playerThumbnailData.Name}: {ex.Message}");
                            }
                        }
                    }
                }
                Console.WriteLine($"Found vanity IDs: {string.Join(", ", addedVanityIds)}");
                if (!hasVanityItems) return;

                command.DeliveryUnits.Add(unit);
                command.Execute(HomeMode);

                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);
            }
        }

        public void PurchaseOfferWithCatalog(int DataGlobalId, int Currency)
        {
            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            if (DataGlobalId > 29000000 && DataGlobalId < DataTables.Get(DataType.Skin).Count + 29000000)
            {
                SkinData skinData = DataTables.Get(DataType.Skin).GetDataWithId<SkinData>(DataGlobalId);
                int[] AllowedCurrency = { 0, 1, 2 };
                if (!AllowedCurrency.Contains(Currency)) return;
                if (Currency == 0) // Gold
                {
                    if (!HomeMode.Avatar.UseHeroLvlUpMaterial(skinData.PriceCoins)) return;
                    if (skinData.PriceCoins == 0) return;
                }
                if (Currency == 1) // Gems
                {
                    if (!HomeMode.Avatar.UseDiamonds(skinData.PriceGems)) return;
                    if (skinData.PriceGems == 0) return;
                }
                if (Currency == 2) // Blings
                {
                    if (!HomeMode.Avatar.UseBling(skinData.PriceBling)) return;
                    if (skinData.PriceBling == 0) return;
                }
                if (skinData.ObtainType != 0)
                {
                    return;
                }

                DeliveryUnit unit = new DeliveryUnit(100);
                GatchaDrop reward = new GatchaDrop(9);
                reward.SkinGlobalId = DataGlobalId;
                reward.Count = 1;
                unit.AddDrop(reward);

                command.DeliveryUnits.Add(unit);
                command.Execute(HomeMode);
                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);
                NewCommand(skinData);
            }
            else if (DataGlobalId > 52000000 && DataGlobalId < DataTables.Get(DataType.Emote).Count + 52000000)
            {
                EmoteData emoteData = DataTables.Get(DataType.Emote).GetDataWithId<EmoteData>(DataGlobalId);
                int[] AllowedCurrency = { 1, 2 };
                if (!AllowedCurrency.Contains(Currency)) return;
                if (Currency == 1) // Gems
                {
                    if (!HomeMode.Avatar.UseDiamonds(emoteData.PriceGems)) return;
                    if (emoteData.PriceGems == 0) return;
                }
                if (Currency == 2) // Blings
                {
                    if (!HomeMode.Avatar.UseBling(emoteData.PriceBling)) return;
                    if (emoteData.PriceBling == 0) return;
                }
                if (emoteData.Name.EndsWith("overcharge")) return;

                DeliveryUnit unit = new DeliveryUnit(100);
                GatchaDrop reward = new GatchaDrop(11);
                reward.VanityGlobalId = DataGlobalId;
                reward.Count = 1;
                unit.AddDrop(reward);
                command.DeliveryUnits.Add(unit);
                command.Execute(HomeMode);
                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);
            }
            else if (DataGlobalId > 28000000 && DataGlobalId < DataTables.Get(DataType.PlayerThumbnail).Count + 28000000)
            {
                PlayerThumbnailData playerThumbnail = DataTables.Get(DataType.PlayerThumbnail).GetDataWithId<PlayerThumbnailData>(DataGlobalId);
                int[] AllowedCurrency = { 1, 2 };
                if (!AllowedCurrency.Contains(Currency)) return;
                if (Currency == 1) // Gems
                {
                    if (!HomeMode.Avatar.UseDiamonds(playerThumbnail.PriceGems)) return;
                    if (playerThumbnail.PriceGems == 0) return;
                }
                if (Currency == 2) // Blings
                {
                    if (!HomeMode.Avatar.UseBling(playerThumbnail.PriceBling)) return;
                    if (playerThumbnail.PriceBling == 0) return;
                }
                if (playerThumbnail.LockedForChronos == true) return;
                if (playerThumbnail.IsAvailableForOffers == null) return;

                DeliveryUnit unit = new DeliveryUnit(100);
                GatchaDrop reward = new GatchaDrop(11);
                reward.VanityGlobalId = DataGlobalId;
                reward.Count = 1;
                unit.AddDrop(reward);

                command.DeliveryUnits.Add(unit);
                command.Execute(HomeMode);
                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);
            }

            void NewCommand(SkinData skinData)
            {
                if (skinData == null) return;
                DeliveryUnit unit = new DeliveryUnit(100);
                GatchaDrop reward = new GatchaDrop(9);
                bool hasVanityItems = false;
                foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
                {

                    if (emoteData.Skin == skinData.Name)
                    {
                        hasVanityItems = true;
                        GatchaDrop reward1 = new GatchaDrop(11);
                        reward1.VanityGlobalId = DataTables.Get(DataType.Emote).GetData<EmoteData>(emoteData.Name).GetGlobalId();
                        reward1.Count = 1;
                        unit.AddDrop(reward1);
                    }
                }
                foreach (PlayerThumbnailData playerThumbnailData in DataTables.Get(DataType.PlayerThumbnail).GetDatas())
                {
                    if (playerThumbnailData.CatalogPreRequirementSkin == skinData.Name)
                    {
                        hasVanityItems = true;
                        GatchaDrop reward1 = new GatchaDrop(11);
                        reward1.VanityGlobalId = DataTables.Get(DataType.PlayerThumbnail).GetData<PlayerThumbnailData>(playerThumbnailData.Name).GetGlobalId();
                        reward1.Count = 1;
                        unit.AddDrop(reward1);
                    }

                }
                if (!hasVanityItems) return;
                command.DeliveryUnits.Add(unit);
                command.Execute(HomeMode);

                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);

            }
        }


        public void UpdateOfferBundles()
        {
            OfferBundles.RemoveAll(bundle => bundle.IsTrue);

            if (HomeMode.Avatar.Trophies >= 400)
            {
            }

            GenerateOffer(
                new DateTime(2025, 12, 21, 8, 0, 0), new DateTime(2025, 12, 22, 8, 0, 0),
                20000, 999, 0, ShopItem.RecruitToken,
                0, 0, 0,
                "debug", "1 день", "offer_bgr_xmas"
            );

            GenerateOffer(
                new DateTime(2025, 12, 21, 8, 0, 0), new DateTime(2025, 12, 22, 8, 0, 0),
                20000, 999, 0, ShopItem.RecruitToken,
                0, 0, 0,
                "debug", "1 день", "offer_bgr_xmas"
            );

            GenerateOffer(
                new DateTime(2025, 12, 21, 8, 0, 0), new DateTime(2025, 12, 22, 8, 0, 0),
                20000, 999, 0, ShopItem.RecruitToken,
                0, 0, 0,
                "debug", "1 день", "offer_bgr_xmas"
            );
        }


        public void GenerateOfferPanel(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            //int Count2,
            //int BrawlerID2,
            //int Extra2,
            //ShopItem Item2,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsPanel = true;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;
            bundle.IsTID = Title.Contains("TID_") ? 1 : 0;
            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, GlobalId.CreateGlobalId(16, BrawlerID), Extra);
            bundle.Items.Add(offer);

            //Offer offer2 = new Offer(Item2, Count2, GlobalId.CreateGlobalId(16, BrawlerID2), Extra2);
            //bundle.Items.Add(offer2);

            OfferBundles.Add(bundle);
        }

        public void GenerateOfferPanel2(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Count2,
            int BrawlerID2,
            int Extra2,
            ShopItem Item2,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsPanel = true;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;
            bundle.IsTID = Title.Contains("TID_") ? 1 : 0;
            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, GlobalId.CreateGlobalId(16, BrawlerID), Extra);
            bundle.Items.Add(offer);
            Offer offer2 = new Offer(Item2, Count2, GlobalId.CreateGlobalId(16, BrawlerID2), Extra2);
            bundle.Items.Add(offer2);

            OfferBundles.Add(bundle);
        }

        public void GenerateOffer(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;
            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, GlobalId.CreateGlobalId(16, BrawlerID), Extra);
            bundle.Items.Add(offer);

            OfferBundles.Add(bundle);
        }

        public void GenerateOffer2(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Count2,
            int BrawlerID2,
            int Extra2,
            ShopItem Item2,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;
            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, GlobalId.CreateGlobalId(16, BrawlerID), Extra);
            bundle.Items.Add(offer);
            Offer offer2 = new Offer(Item2, Count2, GlobalId.CreateGlobalId(16, BrawlerID2), Extra2);
            bundle.Items.Add(offer2);

            OfferBundles.Add(bundle);
        }

        public void GenerateOffer3(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Count2,
            int BrawlerID2,
            int Extra2,
            ShopItem Item2,
            int Count3,
            int BrawlerID3,
            int Extra3,
            ShopItem Item3,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;
            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, GlobalId.CreateGlobalId(16, BrawlerID), Extra);
            bundle.Items.Add(offer);
            Offer offer2 = new Offer(Item2, Count2, GlobalId.CreateGlobalId(16, BrawlerID2), Extra2);
            bundle.Items.Add(offer2);
            Offer offer3 = new Offer(Item3, Count3, GlobalId.CreateGlobalId(16, BrawlerID3), Extra3);
            bundle.Items.Add(offer3);

            OfferBundles.Add(bundle);
        }

        public void GenerateOffer4(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Count2,
            int BrawlerID2,
            int Extra2,
            ShopItem Item2,
            int Count3,
            int BrawlerID3,
            int Extra3,
            ShopItem Item3,
            int Count4,
            int BrawlerID4,
            int Extra4,
            ShopItem Item4,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR
            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;
            if (OffersClaimed.Contains(bundle.Claim))
            {
                bundle.Purchased = true;
            }

            Offer offer = new Offer(Item, Count, GlobalId.CreateGlobalId(16, BrawlerID), Extra);
            bundle.Items.Add(offer);
            Offer offer2 = new Offer(Item2, Count2, GlobalId.CreateGlobalId(16, BrawlerID2), Extra2);
            bundle.Items.Add(offer2);
            Offer offer3 = new Offer(Item3, Count3, GlobalId.CreateGlobalId(16, BrawlerID3), Extra3);
            bundle.Items.Add(offer3);
            Offer offer4 = new Offer(Item4, Count4, GlobalId.CreateGlobalId(16, BrawlerID4), Extra4);
            bundle.Items.Add(offer4);

            OfferBundles.Add(bundle);
        }


        public void LogicDailyData(ByteStream encoder, DateTime utcNow)
        {
            // LogicDailyData
            encoder.WriteVInt(1688816070);
            encoder.WriteVInt(1191532375);

            encoder.WriteVInt(utcNow.Year * 1000 + utcNow.DayOfYear); // 0x78d4b8
            encoder.WriteVInt(utcNow.Hour * 3600 + utcNow.Minute * 60 + utcNow.Second); // 0x78d4cc
            encoder.WriteVInt(HomeMode.Avatar.Trophies); // 0x78d4e0
            encoder.WriteVInt(HomeMode.Avatar.HighestTrophies); // 0x78d4f4
            encoder.WriteVInt(HomeMode.Avatar.HighestTrophies); // highest trophy again?
            encoder.WriteVInt(TrophyRoadProgress);
            encoder.WriteVInt(1909); // experience
            ByteStreamHelper.WriteDataReference(encoder, Thumbnail);
            ByteStreamHelper.WriteDataReference(encoder, NameColorId);

            encoder.WriteVInt(0);

            encoder.WriteVInt(0);

            encoder.WriteVInt(0);

            encoder.WriteVInt(0);


            encoder.WriteVInt(UnlockedSkins.Count); // Played game modes
            foreach (int s in UnlockedSkins)
            {
                ByteStreamHelper.WriteDataReference(encoder, s);
            }

            encoder.WriteVInt(0);

            encoder.WriteVInt(0);

            encoder.WriteVInt(0);
            encoder.WriteVInt(HomeMode.Avatar.HighestTrophies);
            encoder.WriteVInt(0);
            encoder.WriteVInt(2);
            encoder.WriteBoolean(true);
            encoder.WriteVInt(115);
            encoder.WriteVInt(335442);
            encoder.WriteVInt(1001442);
            encoder.WriteVInt(5778642);

            encoder.WriteVInt(120);
            encoder.WriteVInt(200);
            encoder.WriteVInt(0);

            encoder.WriteBoolean(true);
            encoder.WriteVInt(2);
            encoder.WriteVInt(2);
            encoder.WriteVInt(2);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);

            encoder.WriteVInt(OfferBundles.Count); // Shop offers at 0x78e0c4
            foreach (OfferBundle offerBundle in OfferBundles)
            {
                offerBundle.Encode(encoder);
            }

            encoder.WriteVInt(20);
            encoder.WriteVInt(1428);

            encoder.WriteVInt(0);

            encoder.WriteVInt(1);
            encoder.WriteVInt(30);

            encoder.WriteByte(1); // count brawlers selected
            ByteStreamHelper.WriteDataReference(encoder, CharacterId);

            encoder.WriteString("RU"); // location
            encoder.WriteString(""); // supported creator

            encoder.WriteVInt(0);
            {

            }



            encoder.WriteVInt(0);

            encoder.WriteVInt(1); // count brawl pass seasons
            {
                encoder.WriteVInt(30); // SeasonID
                encoder.WriteVInt(BrawlPassTokens);
                encoder.WriteBoolean(HasPremiumPass);

                encoder.WriteVInt(131);
                encoder.WriteBoolean(false);

                if (encoder.WriteBoolean(true)) // Track 9
                {
                    encoder.WriteLongLong128(PremiumPassProgress);
                }
                if (encoder.WriteBoolean(true)) // Track 10
                {
                    encoder.WriteLongLong128(BrawlPassProgress);
                }
                encoder.WriteBoolean(HasPremiumPassPlus); // BrawlPassPlus
                if (encoder.WriteBoolean(true)) // Track ?
                {
                    encoder.WriteLongLong128(BrawlPassPlusProgress);
                }
            }




            encoder.WriteBoolean(true);
            encoder.WriteVInt(0);
            encoder.WriteVInt(1);
            encoder.WriteVInt(2);
            encoder.WriteVInt(0);

            encoder.WriteBoolean(true); // Vanity items
            encoder.WriteVInt(UnlockedEmotes.Count + UnlockedTituls.Count + UnlockedThumbnails.Count + UnlockedSprays.Count + UnlockedFrames.Count); // Played game modes
            foreach (int Emote in UnlockedEmotes)
            {
                ByteStreamHelper.WriteDataReference(encoder, Emote);
                encoder.WriteVInt(1);
                encoder.WriteVInt(Emote);
                encoder.WriteVInt(4);
            }
            foreach (int Thumbnail in UnlockedThumbnails)
            {
                ByteStreamHelper.WriteDataReference(encoder, Thumbnail);
                encoder.WriteVInt(0);
            }
            foreach (int Spray in UnlockedSprays)
            {
                ByteStreamHelper.WriteDataReference(encoder, Spray);
                encoder.WriteVInt(0);
            }
            foreach (int Titul in UnlockedTituls)
            {
                ByteStreamHelper.WriteDataReference(encoder, Titul);
                encoder.WriteVInt(0);
            }
            foreach (int Frame in UnlockedFrames)
            {
                ByteStreamHelper.WriteDataReference(encoder, Frame);
                encoder.WriteVInt(0);
            }

            encoder.WriteBoolean(false); // Power league season data

            encoder.WriteInt(2023189);
            encoder.WriteVInt(2023189);
            ByteStreamHelper.WriteDataReference(encoder, FavouriteCharacter);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(2023189);
            encoder.WriteVInt(2023189);
            encoder.WriteVInt(2023189);
            encoder.WriteVInt(2023189);
            // end LogicDailyData
            Console.WriteLine("[DEBUG] [ClientHome::LogicDailyData] Module loaded!");
        }

        public void LogicConfData(ByteStream encoder, DateTime utcNow)
        {

            encoder.WriteVInt(38); // event slot id
            encoder.WriteVInt(1);
            encoder.WriteVInt(2);
            encoder.WriteVInt(3);
            encoder.WriteVInt(4);
            encoder.WriteVInt(5);
            encoder.WriteVInt(6);
            encoder.WriteVInt(7);
            encoder.WriteVInt(8);
            encoder.WriteVInt(9);
            encoder.WriteVInt(10);
            encoder.WriteVInt(11);
            encoder.WriteVInt(12);
            encoder.WriteVInt(13);
            encoder.WriteVInt(14);
            encoder.WriteVInt(15);
            encoder.WriteVInt(16);
            encoder.WriteVInt(17);
            encoder.WriteVInt(18);
            encoder.WriteVInt(19);
            encoder.WriteVInt(20);
            encoder.WriteVInt(21);
            encoder.WriteVInt(22);
            encoder.WriteVInt(23);
            encoder.WriteVInt(24);
            encoder.WriteVInt(25);
            encoder.WriteVInt(26);
            encoder.WriteVInt(27);
            encoder.WriteVInt(28);
            encoder.WriteVInt(29);
            encoder.WriteVInt(30);
            encoder.WriteVInt(31);
            encoder.WriteVInt(32);
            encoder.WriteVInt(33);
            encoder.WriteVInt(34);
            encoder.WriteVInt(35);
            encoder.WriteVInt(36);
            encoder.WriteVInt(37);
            encoder.WriteVInt(38);

            encoder.WriteVInt(9);
            {
                encoder.WriteVInt(-1);
                encoder.WriteVInt(1);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 121)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);


                encoder.WriteVInt(2);
                encoder.WriteVInt(2);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 20)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);


                encoder.WriteVInt(3);
                encoder.WriteVInt(3);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 807)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);


                encoder.WriteVInt(4);
                encoder.WriteVInt(4);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 800)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);


                encoder.WriteVInt(5);
                encoder.WriteVInt(5);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 793)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);


                encoder.WriteVInt(6);
                encoder.WriteVInt(6);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 758)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);

                encoder.WriteVInt(7);
                encoder.WriteVInt(7);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 737)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);

                encoder.WriteVInt(8);
                encoder.WriteVInt(8);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 727)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);

                encoder.WriteVInt(9);
                encoder.WriteVInt(9);
                encoder.WriteVInt(1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(72292);
                encoder.WriteVInt(10);
                ByteStreamHelper.WriteDataReference(encoder, GlobalId.CreateGlobalId(15, 567)); // map id
                encoder.WriteVInt(-1);
                encoder.WriteVInt(2);
                encoder.WriteString("");
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // MapMaker map structure array
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false); // Power League array entry
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteVInt(-1);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteVInt(0);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
                encoder.WriteBoolean(false);
            }

            encoder.WriteVInt(0);

            ByteStreamHelper.WriteIntList(encoder, new List<int> { 20, 35, 75, 140, 290, 480, 800, 1250, 1875, 2800 });
            ByteStreamHelper.WriteIntList(encoder, new List<int> { 10, 25, 50, 99 });
            ByteStreamHelper.WriteIntList(encoder, new List<int> { 150, 400, 1200, 2600 });

            encoder.WriteVInt(0);

            encoder.WriteVInt(3); // 
            {
                encoder.WriteVInt(GlobalId.CreateGlobalId(41, 110));
                encoder.WriteVInt(1);

                encoder.WriteVInt(0);
                encoder.WriteVInt(37);

                encoder.WriteVInt(1);
                encoder.WriteVInt(46);
            }

            encoder.WriteVInt(0); // TimedIntValueEntry::encode

            encoder.WriteVInt(0); // CustomEvent::encode

            encoder.WriteVInt(0); // ShopChainOfferThemeEntry::encode
            {
            }

            encoder.WriteVInt(0);
            encoder.WriteVInt(0);

            encoder.WriteVInt(2);
            encoder.WriteVInt(1);
            encoder.WriteVInt(2);
            encoder.WriteVInt(2);
            encoder.WriteVInt(1);
            encoder.WriteVInt(-1);
            encoder.WriteVInt(2);
            encoder.WriteVInt(1);
            encoder.WriteVInt(4);

            ByteStreamHelper.WriteIntList(encoder, new List<int> { 0, 29, 79, 169, 349, 699 });
            ByteStreamHelper.WriteIntList(encoder, new List<int> { 0, 160, 450, 500, 1250, 2500 });
            // end LogicConfData 
            Console.WriteLine("[DEBUG] [ClientHome::LogicConfData] Module loaded!");
        }

        public void Encode(ByteStream encoder)
        {
            DateTime utcNow = DateTime.UtcNow;
            LogicDailyData(encoder, utcNow);
            LogicConfData(encoder, utcNow);
            // LogicClientHome
            encoder.WriteLong(HomeId); // Player ID
            NotificationFactory.Encode(encoder);
            encoder.WriteVInt(1);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(0);
            encoder.WriteBoolean(true); // Starr Road
            for (int i = 0; i < 7; i++)
            {
                encoder.WriteVInt(0);
            }


            encoder.WriteVInt(0); // Mastery
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);

            encoder.WriteVInt(0); //Brawler's BattleCards

            StarrDrop.Encode(encoder, HomeMode);

            encoder.WriteBoolean(false);

            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(0);
            // end LogicClientHome
            Console.WriteLine("[DEBUG] [ClientHome] All Modules loaded!");
        }


    }

}
