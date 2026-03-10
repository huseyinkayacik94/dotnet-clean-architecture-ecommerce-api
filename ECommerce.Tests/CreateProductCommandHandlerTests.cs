using ECommerce.Application.Features.Products.Commands;
using ECommerce.Application.Features.Products.Handlers;
using ECommerce.Application.Interfaces;
using ECommerce.Core.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;

        public CreateProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task Should_Create_Product_And_Return_Id()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Keyboard",
                Price = 100
            };

            var handler = new CreateProductCommandHandler(
                _productRepositoryMock.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBe(Guid.Empty);

            _productRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Once
            );
        }
    }
}
