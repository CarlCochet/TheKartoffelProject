namespace Stump.Core.IO
{
    public interface ICustomDataOutput : IDataWriter
    {
        new void WriteVarInt(int value);

        void WriteVarShort(int value);

        void WriteVarLong(double value);
    }
}
