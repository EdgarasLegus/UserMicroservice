using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UserApi.BusinessLogic.Services;
using UserApi.Controllers;
using UserApi.Domain;
using UserApi.Domain.Entities;
using UserApi.Models;

namespace UserApi.ControllerTests
{
    public class UserApiControllerTests
    {
        private IUserService _userServiceMock;
        private UserApiController _userApiController;
        private IMapper _mapperMock;
        private User _user, _createdUser; 
        private UserModel _userModel, _userPostModel;
        private List<User> _users;
        private List<UserModel> _userModels;
        private OperationResult<User> _operationResultWithSuccess;
        private OperationResult<User> _operationResultNotFound;
        private OperationResult<User> _operationResultWithValidationError;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = Substitute.For<IUserService>();
            _mapperMock = Substitute.For<IMapper>();
            _userApiController = new UserApiController(_userServiceMock, _mapperMock);

            _user = new User()
            {
                UserId = 1,
                FirstName = "Antonio",
                LastName = "Rojas",
                Email = "antonio.rojas@gmail.com"
            };

            _userModel = new UserModel
            {
                UserId = _user.UserId,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Email = _user.Email
            };

            _userPostModel = new UserModel
            {
                FirstName = "Shabhaz",
                LastName = "Mubarak",
                Email = "shabhaz.mubarak@gmail.com"
            };

            _createdUser = new User
            {
                FirstName = _userPostModel.FirstName,
                LastName = _userPostModel.LastName,
                Email = _userPostModel.Email
            };

            _operationResultWithSuccess = new OperationResult<User>
            {
                Status = Status.Success,
            };

            _operationResultNotFound = new OperationResult<User>
            {
                Status = Status.NotFound,
            };

            _operationResultWithValidationError = new OperationResult<User>
            {
                Status = Status.ValidationError,
            };
        }

        [Test]
        public void GetAll_ShouldReturnCorrectStatusCode()
        {
            //Arrange
            _users = new List<User> { _user };
            _userModels = new List<UserModel> { _userModel };

            _userServiceMock.GetUsers().Returns(_users);
            _mapperMock.Map<IEnumerable<UserModel>>(_users).Returns(_userModels);

            //Act
            var result = _userApiController.Get() as OkObjectResult;

            //Assert
            _userServiceMock.Received().GetUsers();
            _mapperMock.Received(1).Map<IEnumerable<UserModel>>(_users);

            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [TestCase(1)]
        public async Task GetById_ShouldReturnOkStatus(int expectedUserId)
        {
            //Arrange
            _userServiceMock.GetUserById(_user.UserId).Returns(_user);
            _mapperMock.Map<UserModel>(_user).Returns(_userModel);

            //Act
            var result = await _userApiController.Get(_user.UserId) as OkObjectResult;

            //Assert
            await _userServiceMock.Received().GetUserById(_user.UserId);
            _mapperMock.Received(1).Map<UserModel>(_user);

            Assert.AreEqual(expectedUserId, (result.Value as UserModel).UserId);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task GetById_ShouldReturnNotFoundStatus()
        {
            //Arrange
            _userServiceMock.GetUserById(_user.UserId).ReturnsNull();

            //Act
            var result = await _userApiController.Get(_user.UserId) as NotFoundResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task Create_ShouldCreateNewUser()
        {
            //Arrange
            _mapperMock.Map<User>(_userPostModel).Returns(_createdUser);
            _userServiceMock.CreateUser(_createdUser).Returns(_operationResultWithSuccess);

            //Act
            var result = await _userApiController.Create(_userPostModel) as CreatedAtActionResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
        }

        [Test]
        public async Task Create_ShouldReturnBadRequest()
        {
            //Arrange
            _mapperMock.Map<User>(_userPostModel).Returns(_createdUser);
            _userServiceMock.CreateUser(_createdUser).Returns(_operationResultWithValidationError);

            //Act
            var result = await _userApiController.Create(_userPostModel) as BadRequestObjectResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldDeleteExistingUser()
        {
            //Arrange
            _userServiceMock.DeleteUser(_user.UserId).Returns(_operationResultWithSuccess);

            //Act
            var result = await _userApiController.Delete(_user.UserId) as OkResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task Delete_UserToDeleteIsNotFound()
        {
            //Arrange
            _userServiceMock.DeleteUser(_user.UserId).Returns(_operationResultNotFound);

            //Act
            var result = await _userApiController.Delete(_user.UserId) as NotFoundObjectResult;

            //Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}