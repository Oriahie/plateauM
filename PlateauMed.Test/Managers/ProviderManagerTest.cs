using FluentAssertions;
using NSubstitute;
using PlateauMed.Infrastructure.DTO;
using PlateauMed.Infrastructure.Exceptions;
using PlateauMed.Infrastructure.Interfaces.Repositories;
using PlateauMed.Infrastructure.Interfaces.Services;
using PlateauMed.Infrastructure.Managers;
using PlateauMed.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Test.Managers
{
    public class ProviderManagerTest
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IUtilityService _utilityService;
        private readonly ProviderManager _providerManager;

        public ProviderManagerTest()
        {
            _providerRepository = Substitute.For<IProviderRepository>();
            _utilityService = Substitute.For<IUtilityService>();
            _providerManager = new ProviderManager(_providerRepository, _utilityService);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfProviders()
        {
            // Arrange
            var providers = new List<ProviderModel>
            {
                new() { ProviderId = Guid.NewGuid(), ProviderName = "Provider 1" },
                new() { ProviderId = Guid.NewGuid(), ProviderName = "Provider 2" }
            };

            _providerRepository.GetProviders().Returns(providers);

            // Act
            var result = await _providerManager.GetAll();

            // Assert
            result.Should().BeEquivalentTo(providers);
            await _providerRepository.Received(1).GetProviders();
        }

        [Fact]
        public async Task SetAvailability_ShouldSetAvailabilityWhenValidModel()
        {
            // Arrange
            var model = new ProviderAvailabilityDTO
            {
                StartTime = TimeSpan.FromHours(9),
                CloseTime = TimeSpan.FromHours(17),
                BreakStartTime = TimeSpan.FromHours(12),
                BreakDuration = 1
            };

            var provider = new ProviderModel { ProviderId = Guid.NewGuid(), ProviderName = "Provider Test" };
            var updatedProvider = new ProviderModel { ProviderId = provider.ProviderId, ProviderName = "Provider Test" };

            _utilityService.UserId().Returns(Guid.NewGuid().ToString());
            _providerRepository.GetProviderByUserId(Arg.Any<Guid>()).Returns(provider);
            _providerRepository.SetProviderAvailability(model, provider.ProviderId).Returns(updatedProvider);

            // Act
            var result = await _providerManager.SetAvailability(model);

            // Assert
            result.Should().BeEquivalentTo(updatedProvider);
            await _providerRepository.Received(1).GetProviderByUserId(Arg.Any<Guid>());
            await _providerRepository.Received(1).SetProviderAvailability(model, provider.ProviderId);
        }

        [Fact]
        public async Task SetAvailability_ShouldThrowValidationException_WhenStartTimeGreaterThanCloseTime()
        {
            // Arrange
            var model = new ProviderAvailabilityDTO
            {
                StartTime = TimeSpan.FromHours(18),
                CloseTime = TimeSpan.FromHours(17),
                BreakStartTime = TimeSpan.FromHours(12),
                BreakDuration = 1
            };

            // Act
            var act = async () => await _providerManager.SetAvailability(model);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("Ensure Start time and Close time are correct");
            await _providerRepository.DidNotReceiveWithAnyArgs().SetProviderAvailability(default, default);
        }

        [Fact]
        public async Task SetAvailability_ShouldThrowValidationException_WhenBreakTimeExceedsClosingTime()
        {
            // Arrange
            var model = new ProviderAvailabilityDTO
            {
                StartTime = TimeSpan.FromHours(9),
                CloseTime = TimeSpan.FromHours(17),
                BreakStartTime = TimeSpan.FromHours(16),
                BreakDuration = 2
            };

            // Act
            var act = async () => await _providerManager.SetAvailability(model);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("Ensure break time does not exceeed closing time");
            await _providerRepository.DidNotReceiveWithAnyArgs().SetProviderAvailability(default!, default);
        }

    }
}
