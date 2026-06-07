using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services
{
    // Handles all business logic related to gym classes
    public class GymClassService : IGymClassService
    {
        private readonly IGymClassRepository _gymClassRepository;
        private readonly ITrainerRepository _trainerRepository;
        private readonly ILogger<GymClassService> _logger;

        public GymClassService(
            IGymClassRepository gymClassRepository,
            ITrainerRepository trainerRepository,
            ILogger<GymClassService> logger)
        {
            _gymClassRepository = gymClassRepository;
            _trainerRepository = trainerRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<GymClass>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all gym classes");
            return await _gymClassRepository.GetAllWithDetailsAsync();
        }

        public async Task<GymClass?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving gym class with ID: {ClassId}", id);
            return await _gymClassRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<IEnumerable<GymClass>> GetByTrainerAsync(int trainerId)
        {
            // Make sure the trainer exists first
            var trainerExists = await _trainerRepository.ExistsAsync(trainerId);
            if (!trainerExists)
                throw new KeyNotFoundException($"Trainer with ID {trainerId} not found.");

            return await _gymClassRepository.GetByTrainerAsync(trainerId);
        }

        public async Task<GymClass> CreateAsync(GymClass gymClass)
        {
            // A class must have a valid trainer assigned
            var trainerExists = await _trainerRepository.ExistsAsync(gymClass.TrainerId);
            if (!trainerExists)
                throw new KeyNotFoundException(
                    $"Trainer with ID {gymClass.TrainerId} not found.");

            gymClass.Status = ClassStatus.Scheduled;

            _logger.LogInformation("Creating gym class: {ClassName}", gymClass.Name);
            return await _gymClassRepository.CreateAsync(gymClass);
        }

        public async Task UpdateAsync(int id, GymClass gymClass)
        {
            var existing = await _gymClassRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Gym class with ID {id} not found.");

            existing.Name = gymClass.Name;
            existing.Description = gymClass.Description;
            existing.Capacity = gymClass.Capacity;
            existing.ScheduledAt = gymClass.ScheduledAt;
            existing.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation("Updating gym class with ID: {ClassId}", id);
            await _gymClassRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _gymClassRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Gym class with ID {id} not found.");

            // Don't delete a class that already has enrollments
            var enrollmentCount = await _gymClassRepository.GetEnrollmentCountAsync(id);
            if (enrollmentCount > 0)
                throw new InvalidOperationException(
                    "Cannot delete a class that has active enrollments.");

            _logger.LogInformation("Deleting gym class with ID: {ClassId}", id);
            await _gymClassRepository.DeleteAsync(id);
        }

        public async Task UpdateStatusAsync(int id, ClassStatus newStatus)
        {
            var gymClass = await _gymClassRepository.GetByIdAsync(id);
            if (gymClass == null)
                throw new KeyNotFoundException($"Gym class with ID {id} not found.");

            // Only allow logical status transitions
            var validTransition = (gymClass.Status, newStatus) switch
            {
                (ClassStatus.Scheduled, ClassStatus.InProgress) => true,
                (ClassStatus.InProgress, ClassStatus.Finished) => true,
                (ClassStatus.Scheduled, ClassStatus.Cancelled) => true,
                _ => false
            };

            if (!validTransition)
                throw new InvalidOperationException(
                    $"Cannot change status from {gymClass.Status} to {newStatus}.");

            gymClass.Status = newStatus;
            gymClass.UpdatedAt = DateTime.UtcNow;

            _logger.LogInformation(
                "Updating gym class {ClassId} status to {NewStatus}", id, newStatus);
            await _gymClassRepository.UpdateAsync(gymClass);
        }
    }
}