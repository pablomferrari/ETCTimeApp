namespace ETCTimeApp.BL.Contracts
{
    public abstract class VersionBase : BusinessEntityBase
    {
        public int Version { get; set; }

        public VersionBase()
        {
            Version = 1;
        }
    }
}
