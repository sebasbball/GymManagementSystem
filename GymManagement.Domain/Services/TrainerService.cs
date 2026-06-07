using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services
{
    // Handles all business logic related to trainers
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly ILogger<TrainerService> _logger;

        public TrainerService(
            ITrainerRepository trainerRepository,
            ILogger<TrainerService> logger)
        {
            _trainerRepository = trainerRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Trainer>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all trainers");
            return await _trainerRepository.GetAllAsync();
        }

        public async Task<Trainer?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving trainer with ID: {TrainerId}", id);
            return await _trainerRepository.GetByIdAsync(id);
        }

        public async Task<Trainer> CreateAsync(Trainer trainer)
        {
            // No duplicate emails allowed for trainers
            var existing = await _trainerRepository.GetByEmailAsync(trainer.Email);
            if (existing != null)
                throw new InvalidOperationException(
                    $"A trainer with email '{trainer.Email}' already exists.");

            _logger.LogInformation("Creating trainer: {FirstName} {LastName}",
                trainer.FirstName, trainer.LastName);

            return await _trainerRepository.CreateAsync(trainer);
        }

        public async Task UpdateAsync(int id, Trainer trainer)
        {
            var existing = await _trainerRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Trainer with ID {id} not found.");

            existing.FirstName = trainer.FirstName;
            existing.LastName = trainer.LastName;
            existing.Phone = trainer.Phone;
            existing.Specialty = trainer.Specialty;
            existing.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating trainer with ID: {TrainerId}", id);
            await _trainerRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _trainerRepository.ExistsAsync(id);
            if (!exists)
                throw new KeyNotFoundException($"Trainer with ID {id} not found.");

            _logger.LogInformation("Deleting trainer with ID: {TrainerId}", id);
            await _trainerRepository.DeleteAsync(id);
        }
    }
}