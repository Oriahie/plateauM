using FluentAssertions;
using NSubstitute;
using PlateauMed.Core;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Managers;
using PlateauMed.Infrastructure.Models;
using PlateauMed.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Test.Managers
{
    public class AccountManagerTest
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly AccountManager _accountManager;

        public AccountManagerTest()
        {
            _tokenService = Substitute.For<ITokenService>();
            _userRepository = Substitute.For<IUserRepository>();
            _patientRepository = Substitute.For<IPatientRepository>();
            _providerRepository = Substitute.For<IProviderRepository>();
            _accountManager = new AccountManager(_tokenService, _userRepository, _patientRepository, _providerRepository);
        }


        [Fact]
        public async Task Login_ShouldReturnTokenAndUser_WhenCredentialsAreValid()
        {
            // Arrange
            var loginRequest = new LoginRequestDTO
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var user = new UserModel { UserId = Guid.NewGuid(), Email = loginRequest.Email };
            var passwordHash = Helpers.HashPassword(loginRequest.Password);
            _userRepository.GetUserWithPasswordHash(loginRequest.Email).Returns((user, passwordHash));
            _tokenService.GenerateToken(user).Returns("valid-token");

            // Act
            var result = await _accountManager.Login(loginRequest);

            // Assert
            result.Token.Should().Be("valid-token");
            result.User.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task Login_ShouldThrowValidationException_WhenPasswordIsInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequestDTO
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var user = new UserModel { UserId = Guid.NewGuid(), Email = loginRequest.Email };
            var passwordHash = Helpers.HashPassword("correctpassword");
            _userRepository.GetUserWithPasswordHash(loginRequest.Email).Returns((user, passwordHash));

            // Act
            var act = async () => await _accountManager.Login(loginRequest);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Invalid Username or Password");
        }

        [Fact]
        public async Task Register_ShouldCreatePatient_WhenUserTypeIsPatient()
        {
            // Arrange
            var registerRequest = new RegisterDTO
            {
                Email = "test@example.com",
                Password = "password123",
                UserType = UserType.Patient,
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            var user = new UserModel { UserId = Guid.NewGuid(), Email = registerRequest.Email };
            
            _userRepository.IsUserExists(registerRequest.Email).Returns(false);
            _userRepository.CreateUser(Arg.Any<UserModel>()).Returns(user);

            // Act
            var result = await _accountManager.Register(registerRequest);

            // Assert
            result.Should().BeEquivalentTo(user);
            await _patientRepository.Received(1).AddPatient( Arg.Any<PatientModel>(), user.UserId);
            await _providerRepository.DidNotReceive().AddProvider(new ProviderModel(), user.UserId);
        }

        [Fact]
        public async Task Register_ShouldCreateProvider_WhenUserTypeIsProvider()
        {
            // Arrange
            var registerRequest = new RegisterDTO
            {
                Email = "provider@example.com",
                Password = "password123",
                UserType = UserType.Provider
            };

            var user = new UserModel { UserId = Guid.NewGuid(), Email = registerRequest.Email };
            _userRepository.IsUserExists(registerRequest.Email).Returns(false);
            _userRepository.CreateUser(Arg.Any<UserModel>()).Returns(user);

            // Act
            var result = await _accountManager.Register(registerRequest);

            // Assert
            result.Should().BeEquivalentTo(user);
            await _providerRepository.Received(1).AddProvider(Arg.Any<ProviderModel>(), user.UserId);
            await _patientRepository.DidNotReceive().AddPatient(Arg.Any<PatientModel>(), user.UserId);
        }

        [Fact]
        public async Task Register_ShouldThrowValidationException_WhenUserAlreadyExists()
        {
            // Arrange
            var registerRequest = new RegisterDTO
            {
                Email = "existinguser@example.com",
                Password = "password123",
                UserType = UserType.Patient,
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            _userRepository.IsUserExists(registerRequest.Email).Returns(true);

            // Act
            var act = async () => await _accountManager.Register(registerRequest);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("User already exists");
        }

        [Fact]
        public async Task Register_ShouldThrowValidationException_WhenDateOfBirthIsMissingForPatient()
        {
            // Arrange
            var registerRequest = new RegisterDTO
            {
                Email = "test@example.com",
                Password = "password123",
                UserType = UserType.Patient,
                DateOfBirth = null
            };

            _userRepository.IsUserExists(registerRequest.Email).Returns(false);

            // Act
            var act = async () => await _accountManager.Register(registerRequest);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Date of Birth is required");
        }

    }
}
