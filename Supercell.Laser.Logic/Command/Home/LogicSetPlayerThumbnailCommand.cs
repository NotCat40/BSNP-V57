namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSetPlayerThumbnailCommand : Command
    {
        public int ThumbnailInstanceId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            ThumbnailInstanceId = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            if (ThumbnailInstanceId < 0) return 1;
            if (ThumbnailInstanceId > DataTables.Get(DataType.PlayerThumbnail).Count) return 2;

            PlayerThumbnailData thumbnailData = DataTables.Get(DataType.PlayerThumbnail).GetData<PlayerThumbnailData>(ThumbnailInstanceId);

            Console.WriteLine(thumbnailData.Name);

            if (thumbnailData.RequiredHero != null) { 
                int characterId = DataTables.Get(16).GetData<CharacterData>(thumbnailData.RequiredHero).GetInstanceId() + 16000000;
                if (!homeMode.Avatar.HasHero(characterId)) { return 3; }
            }

            if (thumbnailData.RequiredTotalTrophies != 0)
            {
                if (thumbnailData.RequiredTotalTrophies > homeMode.Avatar.HighestTrophies) { return 4; }
            }

            homeMode.Home.ThumbnailId = GlobalId.CreateGlobalId(28, ThumbnailInstanceId);

            if (homeMode.Avatar.Friends.Count > 0)
            {
                foreach (var friend in homeMode.Avatar.Friends)
                {
                    if (friend.Avatar.Friends.Find(x => x.AccountId == homeMode.Avatar.AccountId) == null) continue;
                    friend.Avatar.Friends.Find(x => x.AccountId == homeMode.Avatar.AccountId).DisplayData = new Logic.Avatar.Structures.PlayerDisplayData(homeMode.Home.ThumbnailId, homeMode.Home.NameColorId, homeMode.Avatar.Name);
                }
            }
            return 0;
        }

        public override int GetCommandType()
        {
            return 505;
        }
    }
}
