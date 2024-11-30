using Microsoft.AspNetCore.Mvc;
using PlateauMed.Core;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Managers;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Models;
using PlateauMed.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = PlateauMed.Infrastructure.Exceptions.ValidationException;

namespace PlateauMed.Infrastructure.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IProviderRepository _providerRepository;

        public AccountManager(ITokenService tokenService,
                              IUserRepository userRepository,
                              IPatientRepository patientRepository,
                              IProviderRepository providerRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _providerRepository = providerRepository;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            var (user, password) = await _userRepository.GetUserWithPasswordHash(model.Email);

            //verify password hash
            if (!Helpers.VerifyPassword(model.Password, password))
                throw new ValidationException("Invalid Username or Password");

            var token =  _tokenService.GenerateToken(user);
            return new LoginResponseDTO
            {
                Token = token,
                User = user
            };
        }

        public async Task<UserModel> Register(RegisterDTO model)
        {
            if (await _userRepository.IsUserExists(model.Email))
                throw new ValidationException("User already exists");

            if (model.UserType == UserType.Patient && model.DateOfBirth == null)
                throw new ValidationException("Date of Birth is required");

            var passwordHash = Helpers.HashPassword(model.Password);

            var user = await _userRepository.CreateUser(model.Map());

            switch (model.UserType)
            {
                case UserType.Patient:
                    //create as patient
                    await _patientRepository.AddPatient(new PatientModel
                    {
                        CreatedDate = DateTimeOffset.Now,
                        DateOfBirth = model.DateOfBirth.Value
                    }, user.UserId);
                    break;
                case UserType.Provider:
                    //create as provider
                    await _providerRepository.AddProvider(new ProviderModel
                    {
                        CreatedDate = DateTimeOffset.Now
                    }, user.UserId);
                    break;
                default:
                    break;
            }
            return user;
        }
    }
}
