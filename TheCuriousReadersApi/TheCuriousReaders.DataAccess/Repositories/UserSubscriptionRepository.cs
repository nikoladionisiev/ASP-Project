using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCuriousReaders.DataAccess.Entities;
using TheCuriousReaders.DataAccess.Interfaces;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ServiceModels;

namespace TheCuriousReaders.DataAccess.Repositories
{
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly CuriousReadersContext _curiousReadersContext;
        private readonly IMapper _mapper;
        private const double days = 7;

        public UserSubscriptionRepository(CuriousReadersContext curiousReadersContext, IMapper mapper)
        {
            _curiousReadersContext = curiousReadersContext;
            _mapper = mapper;
        }

        public async Task ApproveSubscriptionAsync(int id, int requestedDays)
        {
            var subscription = await _curiousReadersContext.UserSubscriptions
                .FirstOrDefaultAsync(subscription => subscription.Id == id);

            subscription.RequestedDays = requestedDays;
            subscription.SubscriptionStart = DateTime.Now;
            subscription.SubscriptionEnd = DateTime.Now.AddDays(requestedDays);
            subscription.IsAdminReviewed = true;
            subscription.IsApproved = true;
            subscription.IsRejected = false;

            _curiousReadersContext.UserSubscriptions.Update(subscription);
            await _curiousReadersContext.SaveChangesAsync();
        }

        public async Task CreateAsync(string userId, RequestSubscriptionModel request, TimeSpan duration)
        {
            var entities = new List<UserSubscriptionEntity>();
            var now = DateTime.Now;

            using var transaction = _curiousReadersContext.Database.BeginTransaction();
            try
            {
                for (int i = 0; i < request.Copies; i++)
                {
                    var entity = new UserSubscriptionEntity
                    {
                        UserId = userId,
                        BookId = request.BookId,
                        SubscriptionStart = now,
                        SubscriptionEnd = now.Add(duration),
                        RequestedDays = duration.Days,
                    };

                    entities.Add(entity);
                }

                await _curiousReadersContext.UserSubscriptions.AddRangeAsync(entities);
                await _curiousReadersContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.RollbackToSavepoint("before adding subscriptions");
            }
        }

        public async Task<bool> ExistsAsync(string userId, int bookId)
        {
            return await _curiousReadersContext.UserSubscriptions.AnyAsync(x => x.UserId == userId && x.BookId == bookId);
        }

        public async Task<ICollection<SubscriptionModel>> GetAllNotReviewedSubscriptionsAsync(PaginationParameters paginationParameters)
        {
            var subscriptions = await _curiousReadersContext.UserSubscriptions
                .Where(subscriptions => !subscriptions.IsAdminReviewed)
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .Include(user => user.User)
                .Include(book => book.Book)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<ICollection<SubscriptionModel>>(subscriptions);
        }

        public async Task<ICollection<ApprovedSubscriptionModel>> GetApprovedSubscriptionsForAnUserAsync(PaginationParameters paginationParameters, string userId)
        {
            var subscriptions = await _curiousReadersContext.UserSubscriptions
                .Where(subscriptions => subscriptions.IsAdminReviewed && subscriptions.IsApproved
                && subscriptions.UserId == userId)
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .Include(user => user.User)
                .Include(book => book.Book)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<ICollection<ApprovedSubscriptionModel>>(subscriptions);
        }

        public async Task<SubscriptionModel> GetSubscriptionAsync(int id)
        {
            var subscription = await _curiousReadersContext.UserSubscriptions
                .FirstOrDefaultAsync(subscription => subscription.Id == id);

            return _mapper.Map<SubscriptionModel>(subscription);
        }

        public async Task<int> GetTotalSubscribersOfABookAsync(int bookId)
        {
            return await _curiousReadersContext.UserSubscriptions
                .Where(subscription => subscription.BookId == bookId)
                .GroupBy(x => x.UserId)
                .CountAsync();
        }

        public async Task RejectSubscriptionAsync(int id)
        {
            var subscription = await _curiousReadersContext.UserSubscriptions
            .FirstOrDefaultAsync(subscription => subscription.Id == id);

            subscription.IsAdminReviewed = true;
            subscription.IsRejected = true;
            subscription.IsApproved = false;

            _curiousReadersContext.UserSubscriptions.Update(subscription);
            await _curiousReadersContext.SaveChangesAsync();
        }

        public async Task ExtendSubscriptionAsync(int id)
        {
            var subscription = await _curiousReadersContext.UserSubscriptions
            .FirstOrDefaultAsync(subscription => subscription.Id == id);

            subscription.IsAdminReviewed = false;

            subscription.SubscriptionEnd = subscription.SubscriptionEnd.AddDays(days);

            _curiousReadersContext.UserSubscriptions.Update(subscription);
            await _curiousReadersContext.SaveChangesAsync();
        }
    }
}
