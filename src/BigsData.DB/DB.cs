using BigsData.Database;

namespace BigsData
{
    public static class DB
    {
        public static BigsDatabase Open(string baseFolder)
        {
            return new BigsDatabase(baseFolder);
        }
    }
}