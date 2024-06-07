using C2S.Business.Interfaces;
using C2S.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2S.Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            review.Id = Guid.NewGuid();

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            if (!await _context.Reviews.AnyAsync()) return null;

            return await _context.Reviews
                .Include(r => r.Service)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByServiceIdAsync(Guid serviceId)
        {
            if (!await _context.Reviews.AnyAsync()) return null;

            return await _context.Reviews
                .Where(r => r.ServiceId == serviceId)
                .Include(r => r.Service)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId)
        {
            if (!await _context.Reviews.AnyAsync()) return null;

            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .Include(r => r.Service)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Review> UpdateReviewAsync(Review review)
        {
            var existingReview = await _context.Reviews.FindAsync(review.Id);
            if (existingReview == null) return null;

            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            existingReview.CreatedAt = DateTime.UtcNow;

            _context.Reviews.Update(existingReview);
            await _context.SaveChangesAsync();

            return existingReview;
        }

        public async Task<bool> DeleteReviewAsync(Guid id)
        {
            if (!await _context.Reviews.AnyAsync()) return false;

            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
