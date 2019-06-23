using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterviewWeb.ApiControllers;
using InterviewWeb.Infrastructure;
using InterviewWeb.Infrastructure.Models;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace InterviewWeb.Tests.Controllers
{
    [TestClass]
    public class ProductsApiControllerTest
    {
        private List<Product> _products;
        [SetUp]
        public void Setup()
        {
            _products = new List<Product>()
            {

                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 1, InternalCode = "CODE_1",
                    Name = "Apple"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 2, InternalCode = "CODE_22",
                    Name = "Pear"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 3, InternalCode = "CODE_12",
                    Name = "Grapes"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = null, Id = 4, InternalCode = "CODE_4",
                    Name = "Banana"
                },
                new Product
                {
                    DateCreated = DateTime.UtcNow, DateDiscontinued = DateTime.UtcNow, Id = 5, InternalCode = "CODE_5",
                    Name = "Mango"
                }
            };
            
        }

        [Test]
        public void Get_ShouldResturnAllProductsButDiscontinued()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.GetAll()).Returns(Task.FromResult(_products as IList<Product>));

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get();

            // Assert
            var res = response.Result as OkNegotiatedContentResult<List<Product>>;
            Assert.AreEqual(res.Content.Count, _products.Count(x => x.DateDiscontinued == null));
        }

        [Test]
        public void Get_ShouldReturnOkResult()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.GetAll()).Returns(Task.FromResult(_products as IList<Product>));

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get();
            // Assert
            var res = response.Result.ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }

        [Test]
        public void GetById_GivenProductId_ShouldReturnProductObject()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(_products.First);

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(1);

            // Assert
            var res = response as OkNegotiatedContentResult<Product>;
            Assert.AreEqual(res.Content.Id, _products.First().Id);
            Assert.AreEqual(res.Content.Name, _products.First().Name);
            Assert.AreEqual(res.Content.DateCreated, _products.First().DateCreated);
            Assert.AreEqual(res.Content.InternalCode, _products.First().InternalCode);
            Assert.AreEqual(res.Content.DateDiscontinued, _products.First().DateDiscontinued);
        }

        [Test]
        public void GetById_GivenProductId_ShouldReturnOKResult()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(_products.First);

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(1);

            // Assert
            var res = response.ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }

        [Test]
        public void GetById_GivenProductId_ShouldReturnNotFound()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Product) null);

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(99);

            // Assert
            var res = response.ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, res.StatusCode);
        }

        [Test]
        public void GetById_GivenProductId_ShouldReturnGone()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(_products.First(x => x.DateDiscontinued != null));

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(99);

            // Assert
            var res = response.ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.Gone, res.StatusCode);
        }

        [Test]
        public void Delete_GivenProductId_ShouldReturnOKResult()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Delete(1);

            // Assert
            var res = response.ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }

        [Test]
        public void Delete_GivenProductId_ShouldReturnNotFound()
        {
            // Arrange
            var inMemoryRepoMock = new Mock<IProductRepository>();
            inMemoryRepoMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(false);

            var controller = new ProductsApiController(inMemoryRepoMock.Object);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Delete(1);

            // Assert
            var res = response.ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.NotFound, res.StatusCode);
        }
    }

    }
    