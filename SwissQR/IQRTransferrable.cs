namespace SwissQR
{
    public interface IQRTransferrable
    {
        string[] Export();

        void Import(string[] value);

        int GetFieldCount();
    }
}
