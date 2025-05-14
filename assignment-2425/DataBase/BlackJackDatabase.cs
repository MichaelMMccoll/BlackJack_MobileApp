using SQLite;

namespace assignment_2425.DataBase
{
    public class BlackJackDatabase
    {
        private readonly SQLiteAsyncConnection _database;
        private readonly string _dbPath;

        public BlackJackDatabase()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "BlackJack.db");
            _database = new SQLiteAsyncConnection(_dbPath);
            Task.Run(async () => await InitializeDatabase()).Wait();
        }

        private async Task InitializeDatabase()
        {
            // Create the table if it doesn't exist
            await _database.CreateTableAsync<Models.Settings>();

            // If table is empty adds default data
            var count = await _database.Table<Models.Settings>().CountAsync();
            if (count == 0)
            {
                await _database.InsertAsync(new Models.Settings { TextToSpeech = false, Vibration = false });
            }
        }
        //Gets data
        public async Task<List<Models.Settings>> GetItemsAsync()
        {
            return await _database.Table<Models.Settings>().ToListAsync();
        }
        //Saves Data
        public async Task<int> SaveItemAsync(Models.Settings item)
        {
            return await _database.UpdateAsync(item);
        }
    }
}
