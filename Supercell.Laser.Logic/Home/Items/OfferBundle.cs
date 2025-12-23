// not final code, he-he :3
namespace Supercell.Laser.Logic.Home.Items
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;

    public class OfferBundle
    {
        public List<Offer> Items;
        public int Currency;
        public int Cost;
        public bool IsDailyDeals;
        public bool Purchased;
        public bool IsTrue;
        public DateTime EndTime;

        public int OldCost;

        public string Title;
        public string BackgroundExportName;
        public string Claim;

        public int State;

        public int IsTID;
        public bool IsPanel;
        public int ShopPanelLayoutId;

        public bool IsChain;
        public int ShopChainId;


        public OfferBundle()
        {
            Items = new List<Offer>();
            State = 0;
        }

        public void Encode(ByteStream Stream)
        {
            Stream.WriteVInt(Items.Count);  // RewardCount
            foreach (Offer gemOffer in Items)
            {
                gemOffer.Encode(Stream);
            }
            Stream.WriteVInt(Currency); // currency
            Stream.WriteVInt(Cost); // cost
            Stream.WriteVInt((int)(EndTime - DateTime.UtcNow).TotalSeconds); // Seconds left
            Stream.WriteVInt(State); // State
            Stream.WriteVInt(0); // ??
            Stream.WriteVInt(0); // ??
            Stream.WriteBoolean(Purchased); // already bought

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(IsDailyDeals); // is daily deals
            Stream.WriteVInt(OldCost); // Old cost???
            Stream.WriteString(Title); // Name
            Stream.WriteVInt(IsTID); // is tid
            Stream.WriteBoolean(false); // LoadOnStartup
            Stream.WriteString(BackgroundExportName);  // background
            Stream.WriteVInt(-1);
            Stream.WriteBoolean(false); // processed
            Stream.WriteVInt(0); // type benefit
            Stream.WriteVInt(0); // benefit
            Stream.WriteString("");
            Stream.WriteBoolean(false); // one-time-offer text 
            Stream.WriteBoolean(false);
            if (IsChain) Stream.WriteDataReference(69, ShopChainId);
            else Stream.WriteDataReference(0);
            if (IsPanel) Stream.WriteDataReference(70, ShopPanelLayoutId);
            else Stream.WriteDataReference(0);

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteVInt(0);
            Stream.WriteVInt(-1);
            Stream.WriteVInt(Cost);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteVInt((int)(EndTime - DateTime.UtcNow).TotalSeconds);
            Stream.WriteVInt((int)(EndTime - DateTime.UtcNow).TotalSeconds);
            Stream.WriteBoolean(false);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false); 
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
        }
    }
}
