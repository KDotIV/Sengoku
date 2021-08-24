using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace Sengoku.API.Repositories
{
    public class UserRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<User> _userCollections;

        private const int DefaultUsersPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<User> staticExclution = Builders<User>.Projection.Exclude("password").Exclude("_id");

        public UserRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _ctx = context;
            _userCollections = _ctx.GetCollection<User>("Users");
        }

        public async Task<List<User>> GetUsers(int usersPerPage = DefaultUsersPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = usersPerPage * page;
            var limit = usersPerPage;

            var users = await _userCollections
                .Find(user => true)
                .Project<User>(staticExclution)
                .ToListAsync();

            return users;
        }
        public async Task<User> GetUserById(string userId)
        {
            return await _userCollections.Find(Builders<User>.Filter.Eq(x => x.userId, userId))
                .FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByName(string userName)
        {
            return await _userCollections.Find(Builders<User>.Filter.Eq(x => x.Name, userName)).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserEmail(string userId)
        {
            var found = await _userCollections
                .Find(Builders<User>.Filter.Eq(x => x.userId, userId))
                .Project(Builders<User>.Projection.Include("email")).FirstOrDefaultAsync();

            var result = BsonSerializer.Deserialize<User>(found);

            return result;
        }
        public async Task<UserResponse> AddUserAsync(string name, string email, string password)
        {
            string randId = Helpers.MakeRandomID();
            if(CheckUserDetails(randId))
            {
                await AddUserAsync(name, email, password);
            }
            try
            {
                var newUser = new User()
                {
                    userId = randId,
                    Name = name,
                    Email = email,
                    Password = password,
                    CreatedAt = DateTime.Now,
                    IsBlocked = false,
                    LastUpdated = DateTime.Now

                };
                await _userCollections.InsertOneAsync(newUser);
                return new UserResponse(true, "User Created", newUser);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new UserResponse(false, "A user with the given email already exists.")
                    : new UserResponse(false, ex.Message);
            }
        }
        public async Task<UserResponse> DeleteUser(string email)
        {
            try
            {
                var deletedUser = await _userCollections.FindOneAndDeleteAsync(Builders<User>.Filter.Eq(found => found.Email, email));
                if(!_userCollections.AsQueryable<User>().Any(exists => exists.Email == deletedUser.Email))
                    return new UserResponse(true, "User Deleted", deletedUser);
                return new UserResponse(false, "User deletion was unsuccessful", deletedUser);
            }
            catch (Exception ex)
            {
                return new UserResponse(false, ex.ToString());
            }
        }
        public async Task<UserResponse> UpdateUserEmail(string email, string updatedEmail)
        {
            try
            {
                var updatedUser = await _userCollections.FindOneAndUpdateAsync(
                  Builders<User>.Filter.Where(rec => rec.Email == email),
                  Builders<User>.Update
                    .Set(rec => rec.Email, updatedEmail),
                  options: new FindOneAndUpdateOptions<User>
                  {
                      ReturnDocument = ReturnDocument.After
                  });

                return new UserResponse(true, "User Updated", updatedUser);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new UserResponse(false, "User could not be updated.")
                    : new UserResponse(false, ex.Message);
            }
        }
        private bool CheckUserDetails(string userId = null, string email = null)
        {
            if(_userCollections.AsQueryable<User>().Any(exists => exists.userId == userId)) { return true; }

            if(_userCollections.AsQueryable<User>().Any(exists => exists.Email == email)) { return true; }

            return false;
        }
    }
}
