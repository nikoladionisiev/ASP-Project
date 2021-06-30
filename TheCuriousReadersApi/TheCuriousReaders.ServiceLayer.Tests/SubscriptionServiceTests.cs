using Moq;
using System.Threading.Tasks;
using TheCuriousReaders.Models.RequestModels;
using TheCuriousReaders.Models.ServiceModels;
using TheCuriousReaders.ServiceLayer.Tests.Fixtures;
using TheCuriousReaders.Services.CustomExceptions;
using Xunit;

namespace TheCuriousReaders.ServiceLayer.Tests
{
    public class SubscriptionServiceTests : IClassFixture<SubscriptionServiceFixture>
    {
        private readonly SubscriptionServiceFixture _subscriptionsServiceFixture;

        public SubscriptionServiceTests(SubscriptionServiceFixture subscriptionServiceFixture)
        {
            _subscriptionsServiceFixture = subscriptionServiceFixture;
        }

        [Fact]
        public async Task When_InvalidRequestedDaysNumber_Expect_ValidationException()
        {
            //Arrange
            //Act
            var exception = await Record.ExceptionAsync(async () => await _subscriptionsServiceFixture.SubscriptionsService.ApproveSubscriptionAsync(2, -1));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact]
        public async Task When_BookDoesNotExist_Expect_False()
        {
            //Arrange
            BookModel bookModel = null;
            RequestSubscriptionModel requestSubscriptionModel = new RequestSubscriptionModel();
            requestSubscriptionModel.BookId = 23;
            requestSubscriptionModel.Copies = 24;

            _subscriptionsServiceFixture.BookRepository
                .Setup(bookRepository => bookRepository.GetABookAsync(23)).ReturnsAsync(bookModel);

            //Act
            var result = await _subscriptionsServiceFixture.SubscriptionsService
                 .SubscribeAsync("993dsao", requestSubscriptionModel);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task When_UserAlreadySubscribed_Expect_DuplicateResourceException()
        {
            //Arrange
            BookModel bookModel = new BookModel
            {
                Title = "Game of Thrones",
                Description = "A book about fire and ice",
                Quantity = 22
            };

            RequestSubscriptionModel requestSubscriptionModel = new RequestSubscriptionModel();
            requestSubscriptionModel.BookId = 23;
            requestSubscriptionModel.Copies = 24;

            _subscriptionsServiceFixture.BookRepository
                .Setup(bookRepository => bookRepository.GetABookAsync(23)).ReturnsAsync(bookModel);
            _subscriptionsServiceFixture.UserSubscriptionRepository
                .Setup(userSubscriptionRepository => userSubscriptionRepository.ExistsAsync("993dsao", 23)).ReturnsAsync(true);

            //Act
            var exception = await Record.ExceptionAsync(async () => await _subscriptionsServiceFixture.SubscriptionsService.SubscribeAsync("993dsao", requestSubscriptionModel));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<DuplicateResourceException>(exception);
        }

        [Fact]
        public async Task When_BookQuantityIsZero_Expect_ValidationException()
        {
            //Arrange
            BookModel bookModel = new BookModel
            {
                Title = "Game of Thrones",
                Description = "A book about fire and ice",
                Quantity = 0
            };

            RequestSubscriptionModel requestSubscriptionModel = new RequestSubscriptionModel();
            requestSubscriptionModel.BookId = 23;
            requestSubscriptionModel.Copies = 24;

            _subscriptionsServiceFixture.BookRepository
                .Setup(bookRepository => bookRepository.GetABookAsync(23)).ReturnsAsync(bookModel);
            _subscriptionsServiceFixture.UserSubscriptionRepository
                .Setup(userSubscriptionRepository => userSubscriptionRepository.ExistsAsync("993dsao", 23)).ReturnsAsync(false);

            //Act
            var exception = await Record.ExceptionAsync(async () => await _subscriptionsServiceFixture.SubscriptionsService.SubscribeAsync("993dsao", requestSubscriptionModel));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact]
        public async Task When_NotEnoughQuantityForRequestedCopies_Expect_ValidationException()
        {
            //Arrange
            BookModel bookModel = new BookModel
            {
                Title = "Game of Thrones",
                Description = "A book about fire and ice",
                Quantity = 3
            };

            RequestSubscriptionModel requestSubscriptionModel = new RequestSubscriptionModel();
            requestSubscriptionModel.BookId = 23;
            requestSubscriptionModel.Copies = 23;

            _subscriptionsServiceFixture.BookRepository
                .Setup(bookRepository => bookRepository.GetABookAsync(23)).ReturnsAsync(bookModel);
            _subscriptionsServiceFixture.UserSubscriptionRepository
                .Setup(userSubscriptionRepository => userSubscriptionRepository.ExistsAsync("993dsao", 23)).ReturnsAsync(false);

            //Act
            var exception = await Record.ExceptionAsync(async () => await _subscriptionsServiceFixture.SubscriptionsService.SubscribeAsync("993dsao", requestSubscriptionModel));

            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ValidationException>(exception);
        }

        [Fact]
        public async Task When_SubscribingIsSuccessful_Expect_True()
        {
            //Arrange
            BookModel bookModel = new BookModel
            {
                Title = "Game of Thrones",
                Description = "A book about fire and ice",
                Quantity = 555
            };

            RequestSubscriptionModel requestSubscriptionModel = new RequestSubscriptionModel();
            requestSubscriptionModel.BookId = 23;
            requestSubscriptionModel.Copies = 23;

            _subscriptionsServiceFixture.BookRepository
                .Setup(bookRepository => bookRepository.GetABookAsync(23)).ReturnsAsync(bookModel);
            _subscriptionsServiceFixture.UserSubscriptionRepository
                .Setup(userSubscriptionRepository => userSubscriptionRepository.ExistsAsync("993dsao", 23)).ReturnsAsync(false);

            //Act
            var result = await _subscriptionsServiceFixture.SubscriptionsService.SubscribeAsync("993dsao", requestSubscriptionModel);

            //Assert
            Assert.True(result);
        }
    }
}
