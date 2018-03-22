namespace WorldEditor.Helpers.Sql
{
    public struct RawData
    {
        public object Data;

        public RawData(object data)
        {
            this.Data = data;
        }

        public static RawData Raw(object obj)
        {
            return new RawData(obj);
        }
    }
}