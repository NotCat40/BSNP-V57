namespace Supercell.Laser.Logic.Home.Items
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;

    public class Offer
    {
        public ShopItem Type;
        public int Count;

        public int ItemDataId;
        public int SkinDataId;

        public Offer()
        {
            // Json.
        }

        public Offer(ShopItem type, int count)
        {
            Type = type;
            Count = count;
        }

        public Offer(ShopItem type, int count, int itemGlobalId) : this(type, count)
        {
            ItemDataId = itemGlobalId;
        }

        public Offer(ShopItem type, int count, int itemGlobalId, int skinGlobalId) : this(type, count, itemGlobalId)
        {
            SkinDataId = skinGlobalId;
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt((int)Type);
            stream.WriteVInt(Count);
            ByteStreamHelper.WriteDataReference(stream, ItemDataId);
            stream.WriteVInt(SkinDataId);
        }
    }

    public enum ShopItem
    {
        FreeBox = 0,
        UpgradeMaterial = 1,
        GuaranteedBox = 2,
        GuaranteedHero = 3,
        Skin = 4,
        Item = 5,
        Ticket = 7,
        CoinDoubler = 9,
        Keys = 11,
        WildcardPower = 12,
        EventSlot = 13,
        AdBox = 15,
        Gems = 16,
        StarPoints = 17,
        QuestsFeature = 18,
        Emote = 19,
        EmoteBundle = 20,
        RandomEmotes = 21,
        RandomEmotesForBrawler = 22,
        RandomEmoteOfRarity = 23,
        SkinAndHero = 24,
        PlayerThumbnail = 25,
        PurchaseOptionSkin = 26,
        RandomEmotesPackOfRarity = 27,
        BrawlPassTokens = 28,
        ClubFeature = 29,
        GuaranteedHeroWithLevel = 30,
        GuaranteedBoxWithLevel = 31,
        GearToken = 32,
        GearScrap = 33,
        Spray = 35,
        SprayBundle = 36,
        SpraySlot = 37,
        RecruitToken = 38,
        ChromaticToken = 39,
        PowerPoints = 41,
        BrawlPassTailReward = 42,
        PlayerTitle = 43,
        DailyQuests = 44,
        Bling = 45,
        BrawlPassBundle = 46,
        OverchargeStarrDrop = 47,
        BrawlerUpgradeFromLevelToLevel = 48,
        StarrDropContainer = 49,
        RandomStarrDrop = 50,
        BrawlPassUnlockBrawler = 51,
        BrawlPassPlusBundle = 52,
        RandomCollabDrop = 53,
        SpecialEventTokens = 54,
        MegaBox = 55,
        ArchetypeItem = 56,
        CollabPoints = 57
    }
}
