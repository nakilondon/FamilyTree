using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using MySql.Data.MySqlClient;

namespace ReactNet.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _connectionString;

        public PersonRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task AddPerson(PersonDb personDb)
        {

            if (personDb.NickName == null)
                personDb.NickName = "";

            if (personDb.PlaceOfBirth == null)
                personDb.PlaceOfBirth = "";

            if (personDb.PlaceOfDeath == null)
                personDb.PlaceOfDeath = "";

            if (personDb.Note == null)
                personDb.Note = "";

            if (personDb.Portrait == null)
                personDb.Portrait = "";


            var db = new MySqlConnection(_connectionString);
            try
            {
                var response = await db.ExecuteAsync(@"
INSERT INTO People (
GedcomID, 
Gender,
PreferredName, 
GivenNames,
Surname,
BirthRangeStart,
BirthRangeEnd, 
PlaceOfBirth,
Dead,
DeathRangeStart,
DeathRangeEnd, 
PlaceOfDeath
) VALUES (
@GedcomID, 
@Gender,
@PreferredName, 
@GivenNames,
@Surname,
@BirthRangeStart,
@BirthRangeEnd, 
@PlaceOfBirth,
@Dead,
@DeathRangeStart,
@DeathRangeEnd, 
@PlaceOfDeath)", personDb);
            } catch (Exception e)
            {}
        }

        public async Task<PersonDb> FindPerson(string gedcomId)
        {
            var db = new MySqlConnection(_connectionString);

            return await db.QueryFirstOrDefaultAsync<PersonDb>("SELECT * FROM People WHERE GedcomId = @GedcomId;", new { GedcomId = gedcomId });

        }

        public async Task AddRelationship(RelationshipDb relationshipDb)
        {
            var db = new MySqlConnection(_connectionString);
            try
            {
                var response = await db.ExecuteAsync("INSERT INTO Relationship (Person1, Relationship, Person2) VALUES ( @Person1, @Relationship, @Person2)", relationshipDb);
            }
            catch (Exception e)
            { }

        }
    }
}
