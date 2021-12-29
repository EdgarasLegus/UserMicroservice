using Microsoft.EntityFrameworkCore.Query;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserApi.Data.DataAccess;
using UserApi.Domain;
using UserApi.Domain.Entities;
using UserApi.Messages;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.ServiceTests
{
    public class UserServiceTests
    {
        private IUnitOfWork _unitOfWorkMock;
        private IRepository<User> _userRepositoryMock;
        private IValidationHelper _validationHelperMock;
        private IMessageBusService _messageBusServiceMock;
        private IUserService _userService;
        private List<User> _users;
        private User _user;
        private User _userToCreate;
        private UserCreatedMessage _userCreatedMessage;
        private OperationResult<User> _operationResultWithValidationError;
        private OperationResult<User> _operationResultWithNotFoundError;

        [SetUp]
        public void Setup()
        {
            _validationHelperMock = Substitute.For<IValidationHelper>();
            _messageBusServiceMock = Substitute.For<IMessageBusService>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _userRepositoryMock = Substitute.For<IRepository<User>>();
            _unitOfWorkMock.GetRepository<User>().Returns(_userRepositoryMock);
            _userService = new UserService(_unitOfWorkMock, _validationHelperMock, _messageBusServiceMock);

            _user = new User()
            {
                UserId = 1,
                FirstName = "Antonio",
                LastName = "Rojas",
                Email = "antonio.rojas@gmail.com"
            };

            _userToCreate = new User
            {
                FirstName = "Shabhaz",
                LastName = "Mubarak",
                Email = "shabhaz.mubarak@gmail.com"
            };

            _operationResultWithValidationError = new OperationResult<User>
            {
                Status = Status.ValidationError,
            };

            _operationResultWithNotFoundError = new OperationResult<User>
            {
                Status = Status.NotFound,
            };
        }

        [Test]
        public void GetUsers_ShouldReturnAllUsers()
        {
            //Arrange
            _users = new List<User> { _user };
            _userRepositoryMock.Get().Returns(_users);

            //Act
            var result = _userService.GetUsers();

            //Assert
            _userRepositoryMock.Received().Get();
            Assert.AreEqual(_users, result);
        }

        [Test]
        public async Task GetUserById_ShouldReturnCorrectUser()
        {
            //Arrange
            _userRepositoryMock.GetFirstOrDefault(Arg.Any<Expression<Func<User, bool>>>(),
                Arg.Any<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()).Returns(_user);

            //Act
            var result = await _userService.GetUserById(_user.UserId);

            //Assert
            Assert.AreEqual(_user, result);
        }

        [Test]
        public async Task GetUserById_UserNotFound()
        {
            //Arrange
            _userRepositoryMock.GetFirstOrDefault(Arg.Any<Expression<Func<User, bool>>>(),
                Arg.Any<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()).ReturnsNull();

            //Act
            var result = await _userService.GetUserById(_user.UserId);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateUser_ShouldReturnSuccessfulOperationResult()
        {
            //Arrange
            _userRepositoryMock.GetFirstOrDefault(Arg.Any<Expression<Func<User, bool>>>(),
                Arg.Any<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()).ReturnsNull();


            //Act
            var result = await _userService.CreateUser(_userToCreate);

            //Assert
            await _userRepositoryMock.Received(1).Insert(_userToCreate);
            await _unitOfWorkMock.Received(1).CommitAsync();
            await _messageBusServiceMock.Received().SendMessage(new UserCreatedMessage
            {
                MessageGuid = new Guid(),
                UserId = _userToCreate.UserId,
                Email = _userToCreate.Email
            });
            Assert.AreEqual(Status.Success, result.Status);
        }

        [Test]
        public async Task CreateUser_ShouldReturnOperationResultWithValidationErrors()
        {
            //Arrange
            _userRepositoryMock.GetFirstOrDefault(Arg.Any<Expression<Func<User, bool>>>(),
                Arg.Any<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()).Returns(_user);
            _validationHelperMock.AddExistingUserValidationError(Arg.Any<OperationResult<User>>()).Returns(_operationResultWithValidationError);

            //Act
            var result = await _userService.CreateUser(_user);

            //Assert
            Assert.AreEqual(Status.ValidationError, result.Status);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnSuccessfulOperationResult()
        {
            //Arrange
            _userRepositoryMock.GetFirstOrDefault(Arg.Any<Expression<Func<User, bool>>>(),
                Arg.Any<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()).Returns(_user);

            //Act
            var result = await _userService.DeleteUser(_user.UserId);

            //Assert
            _userRepositoryMock.Received(1).Delete(_user);
            await _unitOfWorkMock.Received(1).CommitAsync();
            Assert.AreEqual(Status.Success, result.Status);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnOperationResultWithValidationErrors()
        {
            //Arrange
            _userRepositoryMock.GetFirstOrDefault(Arg.Any<Expression<Func<User, bool>>>(),
                Arg.Any<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()).ReturnsNull();
            _validationHelperMock.AddUserNotFoundValidationError(Arg.Any<OperationResult<User>>()).Returns(_operationResultWithNotFoundError);

            //Act
            var result = await _userService.DeleteUser(_user.UserId);

            //Assert
            Assert.AreEqual(Status.NotFound, result.Status);
        }
    }
}