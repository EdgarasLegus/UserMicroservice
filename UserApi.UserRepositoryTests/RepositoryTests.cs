using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UserApi.Data;
using UserApi.Data.DataAccess;
using UserApi.Domain.Entities;

namespace UserApi.UserRepositoryTests
{
    public class RepositoryTests
    {
        private UserContext _userContextMock;
        private Microsoft.EntityFrameworkCore.DbSet<User> _realdbSetMock;
        private IDbSet<User> _dbSetMock;
        //private IUserRepository _userRepository;
        private User _testObject;
        private IQueryable<User> _users;

        [SetUp]
        public void Setup()
        {
            _userContextMock = Substitute.For<UserContext>();

            _users = new List<User>
            {
                new User { UserId = 1, FirstName = "Andres", LastName = "Iniesta", Email = "andres.iniesta@gmail.com" },
                new User { UserId = 2, FirstName = "Xavi", LastName = "Hernandez", Email = "xavi.erno@gmail.com" }
            }.AsQueryable();

            _realdbSetMock = Substitute.For<Microsoft.EntityFrameworkCore.DbSet<User>>();

            _dbSetMock = Substitute.For<IDbSet<User>>();
            _dbSetMock.Provider.Returns(_users.Provider);
            _dbSetMock.Expression.Returns(_users.Expression);
            _dbSetMock.ElementType.Returns(_users.ElementType);
            _dbSetMock.GetEnumerator().Returns(_users.GetEnumerator());
            //_userRepository = new UserRepository(_userContextMock);

            _testObject = new User() { UserId = 1 };

            
        }

        [Test]
        public void GetUsers_ShouldBeCalledCorrectly()
        {
            //Arrange
            var testList = new List<User>() { _testObject };
            //_userContextMock.Users.Returns(testList);

            //Act
            //var result = _userRepository.Get();

            //Assert
            //Assert.Equals(testList, result.ToList());
        }
    }
}