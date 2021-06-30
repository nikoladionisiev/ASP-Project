using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ResponseModels;
using TheCuriousReaders.Services.Interfaces;

namespace TheCuriousReaders.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionsService _subscriptionService;
        private readonly IMapper _mapper;
        public SubscriptionsController(ISubscriptionsService subscriptionService, IMapper mapper)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestSubscriptionModel request)
        {
            var userId = User.Claims.SingleOrDefault(c => c.Type == "id").Value;

            if (!await _subscriptionService.SubscribeAsync(userId, request))
            {
                return NotFound("Book could not be found in the database.");
            }

            return NoContent();
        }

        [HttpGet("books/{bookId}/total")]
        public async Task<IActionResult> Get(int bookId)
        {
            return Ok(new { totalSubscribers = await _subscriptionService.GetTotalSubscribersOfABookAsync(bookId) });
        }

        [Authorize(Roles = "Librarian")]
        [HttpGet("non-reviewed")]
        public async Task<IActionResult> GetNonReviewedSubscriptions([FromQuery] PaginationParameters paginationParameters)
        {
            var nonReviewedSubscription = await _subscriptionService.GetNonReviewedSubscriptionsAsync(paginationParameters);

            return Ok(_mapper.Map<ICollection<SubscriptionResponse>>(nonReviewedSubscription));
        }

        [Authorize]
        [HttpGet("approved/{userId}")]
        public async Task<IActionResult> GetApprovedSubscriptions(string userId, [FromQuery] PaginationParameters paginationParameters)
        {
            var approvedSubscriptions = await _subscriptionService.GetApprovedSubscriptionsForAnUserAsync(paginationParameters, userId);

            return Ok(_mapper.Map<ICollection<ApprovedSubscriptionResponse>>(approvedSubscriptions));
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]RequestSubscriptionApprovalModel requestSubscriptionModel)
        {
            if(await _subscriptionService.GetSubscriptionAsync(id) is null)
            {
                return NotFound($"Subscription with id {id} has not been found.");
            }

            if (requestSubscriptionModel.IsApproved)
            {
                await _subscriptionService.ApproveSubscriptionAsync(id, requestSubscriptionModel.SubscriptionDaysAccepted);
                return NoContent();
            }

            await _subscriptionService.RejectSubscriptionAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ExtendSubscriptionAsync(int id)
        {
            await _subscriptionService.ExtendSubscriptionAsync(id);
            return NoContent();
        }

    }
}