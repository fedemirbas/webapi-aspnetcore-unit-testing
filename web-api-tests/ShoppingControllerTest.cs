using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using web_api.Controllers;
using Xunit;

namespace web_api_tests
{
    public class ShoppingControllerTest
    {
        ShoppingController _controller;
        DataContext _context;

        public ShoppingControllerTest()
        {
            InitContext();
            _controller = new ShoppingController(_context);
        }

        public void InitContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase();

            var context = new DataContext(builder.Options);
            _context = context;
        }

        #region Get Tests

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.Get().Result as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<ShoppingItem>>(okResult.Value);
            Assert.Equal(10, items.Count);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.Get(11);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            int testId = 1;

            // Act
            var okResult = _controller.Get(testId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            int testId = 1;

            // Act
            var okResult = _controller.Get(testId).Result as OkObjectResult;

            // Assert
            Assert.IsType<ShoppingItem>(okResult.Value);
            Assert.Equal(testId, (okResult.Value as ShoppingItem).Id);
        }

        #endregion

        #region Add Tests

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new ShoppingItem()
            {
                Manufacturer = "Guinness",
                Price = 12.00M
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = _controller.Post(nameMissingItem);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            ShoppingItem testItem = new ShoppingItem()
            {
                Id = 12,
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var createdResponse = _controller.Post(testItem);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse);
        }


        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new ShoppingItem()
            {
                Id = 13,
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var createdResponse = _controller.Post(testItem) as CreatedAtRouteResult;
            var item = createdResponse.Value as ShoppingItem;

            // Assert
            Assert.IsType<ShoppingItem>(item);
            Assert.Equal("Guinness Original 6 Pack", item.Name);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new ShoppingItem()
            {
                Id = 2,
                Manufacturer = "Guinness",
                Price = 12.00M
            };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = _controller.Put(2, nameMissingItem);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse);
        }

        [Fact]
        public void Update_NotMatchedIdPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new ShoppingItem()
            {
                Id = 1,
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var badResponse = _controller.Put(2, nameMissingItem);

            // Assert
            Assert.IsType<BadRequestResult>(badResponse);
        }


        [Fact]
        public void Update_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            ShoppingItem testItem = new ShoppingItem()
            {
                Id = 4,
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var updatedResponse = _controller.Put(4, testItem);

            // Assert
            Assert.IsType<OkObjectResult>(updatedResponse);
        }


        [Fact]
        public void Update_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new ShoppingItem()
            {
                Id = 1,
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var updatedResponse = _controller.Put(1, testItem) as OkObjectResult;
            var item = updatedResponse.Value as ShoppingItem;

            // Assert
            Assert.IsType<ShoppingItem>(item);
            Assert.Equal("Guinness Original 6 Pack", item.Name);
        }

        [Fact]
        public void Update_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            int notExistingID = 11;

            // Act
            var badResponse = _controller.Delete(notExistingID);

            // Assert
            Assert.IsType<NotFoundResult>(badResponse);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Remove_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var testItem = new ShoppingItem()
            {
                Id = 1,
                Name = "Guinness Original 6 Pack",
                Manufacturer = "Guinness",
                Price = 12.00M
            };

            // Act
            var updatedResponse = _controller.Put(1, testItem) as OkObjectResult;

            // Assert
            Assert.IsType<NotFoundResult>(updatedResponse);
        }

        [Fact]
        public void Remove_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            int existingId = 1;

            // Act
            var okResponse = _controller.Delete(existingId);

            // Assert
            Assert.IsType<NoContentResult>(okResponse);
        }
        [Fact]
        public void Remove_ExistingIdPassed_RemovesOneItem()
        {
            // Arrange
            int existingId = 1;

            // Act
            var okResponse = _controller.Delete(existingId);
            var okResult = _controller.Get().Result as OkObjectResult;
            List<ShoppingItem> value = (List<ShoppingItem>)okResult.Value;

            // Assert
            Assert.Equal(9, value.Count);
        }

        #endregion

    }
}
