using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

namespace Sengoku.API.Repositories
{
    public class UserRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<User> _userCollections;

        private const int DefaultUsersPerPage = 20;
        private const int DefaultSortOrder = -1;

        public UserRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
            _ctx = context;
            _userCollections = _ctx.GetCollection<User>("Users");
        }

        public async Task<IReadOnlyList<User>> GetUsers(int usersPerPage = DefaultUsersPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = usersPerPage * page;
            var limit = usersPerPage;

            var users = await _userCollections
                .Find(Builders<User>.Filter.Empty)
                .Limit(limit)
                .Skip(skip)
                .ToListAsync();

            return users;
        }
        public async Task<User> GetUserById(string userId)
        {
            return await _userCollections.Find(Builders<User>.Filter.Eq(x => x.userId, userId)).FirstOrDefaultAsync();
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
                var createdUser = await GetUserById(newUser.userId);
                await _userCollections.InsertOneAsync(newUser);
                return new UserResponse(createdUser);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new UserResponse(false, "A user with the given email already exists.")
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
