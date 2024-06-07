using C2S.Data.Models;

namespace C2S.Business.Interfaces
{
    public interface IReviewService
    {
        Task<Review> CreateReviewAsync(Review review);
        Task<Review> GetReviewByIdAsync(Guid id);
        Task<IEnumerable<Review>> GetReviewsByServiceIdAsync(Guid serviceId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId);
        Task<Review> UpdateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(Guid id);
    }
}
