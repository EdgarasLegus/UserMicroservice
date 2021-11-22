using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UserApi.Models;

namespace UserApi.ModelTests
{
    public class UserModelTests
    {
        private UserModel _userWithLongFirstName;
        private UserModel _userWithNoFirstName;
        private UserModel _userWithShortLastName;
        private UserModel _userWithBadEmail;

        [SetUp]
        public void Setup()
        {
            _userWithLongFirstName = new UserModel
            {
                FirstName = "sdfrfrsgrgrsgrgrsgsgsgsgsgrsgsgrsgrsgsgsgsgrsgr",
                LastName = "Linux",
                Email = "t.linux@gmail.com"
            };

            _userWithNoFirstName = new UserModel
            {
                FirstName = "",
                LastName = "Linux",
                Email = "t.linux@gmail.com"
            };

            _userWithShortLastName = new UserModel
            {
                FirstName = "Teo",
                LastName = "L",
                Email = "t.linux@gmail.com"
            };

            _userWithBadEmail = new UserModel
            {
                FirstName = "Theodor",
                LastName = "Linux",
                Email = "t.linuxgmail.com"
            };
        }

        [Test]
        public void FirstName_ShouldNotBeTooLong()
        {
            //Arrange,Act
            var result = ValidateModel(_userWithLongFirstName);

            //Assert
            Assert.IsTrue(result.Any(r => r.MemberNames.Contains("FirstName")));
        }

        [Test]
        public void FirstName_ShouldNotBeNull()
        {
            //Arrange,Act
            var result = ValidateModel(_userWithNoFirstName);

            //Assert
            Assert.IsTrue(result.Any(r => r.ErrorMessage.Contains("required")));
        }

        [Test]
        public void LastName_ShouldNotBeTooShort()
        {
            //Arrange,Act
            var result = ValidateModel(_userWithShortLastName);

            //Assert
            Assert.IsTrue(result.Any(r => r.MemberNames.Contains("LastName")));
        }

        [Test]
        public void Email_ShouldCorresponToStandards()
        {
            //Arrange,Act
            var result = ValidateModel(_userWithBadEmail);

            //Assert
            Assert.IsTrue(result.Any(r => r.MemberNames.Contains("Email")));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);

            Validator.TryValidateObject(model, validationContext, validationResults, true);

            return validationResults;
        }
    }
}