namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Titan.DataStream;
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    public class LogicRefreshRandomRewardsCommand : Command
    {
        public bool guaranteed;
        public int Count = 0;
        public int Rarity = 0;
        public int NextStep = 4;
        public bool Disable;

        public override void Encode(ByteStream stream)
        {
            
            stream.WriteVInt(1);
            stream.WriteVInt(-1);
            stream.WriteVInt(-1);
            stream.WriteVInt(0);
            stream.WriteVInt(1);
            stream.WriteVInt(1);


            stream.WriteVInt(5);
            {
                stream.WriteDataReference(80, 1);
                stream.WriteVInt(-1);
                stream.WriteVInt(0);
                stream.WriteDataReference(80, 2);
                stream.WriteVInt(-1);
                stream.WriteVInt(0);
                stream.WriteDataReference(80, 3);
                stream.WriteVInt(-1);
                stream.WriteVInt(0);
                stream.WriteDataReference(80, 4);
                stream.WriteVInt(-1);
                stream.WriteVInt(0);
                stream.WriteDataReference(80, 5);
                stream.WriteVInt(-1);
                stream.WriteVInt(0);
            }

            if (!Disable)
            {
                stream.WriteVInt(Count);
                stream.WriteDataReference(80, Rarity);
                stream.WriteVInt(1);
                {
                    stream.WriteVInt(16);
                    stream.WriteVInt(11);
                    stream.WriteDataReference(0, 0);
                    stream.WriteVInt(0);
                }
                stream.WriteVInt(0);
                stream.WriteVInt(0);
            } else
            {
                stream.WriteVInt(0);
            }

                stream.WriteInt(0);
                stream.WriteVInt(-1); // Battle Progression Step
                stream.WriteVInt(0);
                stream.WriteVInt(11111111); // timer until refresh
                stream.WriteVInt(0);
                stream.WriteVInt(0);

                stream.WriteVInt(0);
                stream.WriteVInt(0);
                stream.WriteVInt(0);
                stream.WriteVInt(0);
                stream.WriteBoolean(false);
            
            base.Encode(stream);
        }

        public override int Execute(HomeMode homeMode)
        {

            Rarity = homeMode.Home.StarrDrop.Rarity;
            return 0;
        }

        public override int GetCommandType()
        {
            return 228;
        }
    }
}
