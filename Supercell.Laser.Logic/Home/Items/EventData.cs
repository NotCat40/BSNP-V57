namespace Supercell.Laser.Logic.Home.Items
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json.Converters;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class EventData
    {
        public int Slot;
        public int LocationId;
        public DateTime EndTime;
        public LocationData Location => DataTables.Get(DataType.Location).GetDataByGlobalId<LocationData>(LocationId);

        public void Encode(ByteStream encoder)
        {
            encoder.WriteVInt(-1);
            encoder.WriteVInt(Slot);
            encoder.WriteVInt(1);
            encoder.WriteVInt(0);
            encoder.WriteVInt((int)(EndTime - DateTime.Now).TotalSeconds);
            encoder.WriteVInt(10);
            if (Slot == 12 || Slot == 13) ByteStreamHelper.WriteDataReference(encoder, null);
            else ByteStreamHelper.WriteDataReference(encoder, Location);
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
    }
}
