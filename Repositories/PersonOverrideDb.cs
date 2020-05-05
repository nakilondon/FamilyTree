using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ReactNet.Models;

namespace ReactNet.Repositories
{
    public class PersonOverrideDb : IPersonOverride
    {
        private readonly string _connectionString;

        public PersonOverrideDb(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddPerson(PersonOverride personOverride)
        {
            if (personOverride.Birth == null)
            {
                personOverride.Birth = "";
            }

            if (personOverride.Death == null)
            {
                personOverride.Death = "";
            }

            if (personOverride.Note == null)
            {
                personOverride.Note = "";
            }

            if (personOverride.Portrait == null)
            {
                personOverride.Portrait = "";
            }

            var db = new MySqlConnection(_connectionString);
            var response = await db.ExecuteAsync("INSERT INTO People(Id, PreferredName, FullName, Birth, Death, Note, Portrait) VALUES (@Id, @PreferredName, @FullName, @Birth, @Death, @Note, @Portrait)", personOverride);
            
        }

        public async Task UpdatePerson(PersonOverride personOverride)
        {
            var db = new MySqlConnection(_connectionString);

            var sqlStatement = @"
UPDATE People
SET PreferredName = @PreferredName, 
FullName = @FullName, 
Birth = @Birth, 
Death = @Death, 
Note = @Note, 
Portrait = @Portrait
WHERE Id = @Id";

            await db.ExecuteAsync(sqlStatement, personOverride);

        }

        public async Task<PersonOverride> GetPerson(string id)
        {
            var db = new MySqlConnection(_connectionString);
         
            return await db.QueryFirstOrDefaultAsync<PersonOverride>("SELECT * FROM People WHERE Id = @Id;", new  {Id = id});

        }
    }
}
