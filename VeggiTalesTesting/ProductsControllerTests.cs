using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeggiTales.Controllers;
using VeggiTales.Data;
using VeggiTales.Models;

namespace VeggiTalesTesting
{
    [TestClass]
    public class ProductsControllerTests
    {
        ApplicationDbContext _context;
        ProductsController _controller;

        [TestInitialize] 
        public void Init() {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            //it took me a very long time to figure out I needed to also add a parent category for the unit testing
            //I feel rather stupid
            //I hope you're happy >:C
            Category category = new Category { CategoryId = 1, Name = "some category" };
            _context.Categories.Add(category);

            Product product = new Product { ProductId = 3, Name = "Some Product", CategoryId = 1 };
            _context.Products.Add(product);
            _context.SaveChanges();

            _controller = new ProductsController(_context);
        }

        #region "Delete"

        [TestMethod]
        public void DeleteGetValidIdReturnsView()
        {
            var result = (ViewResult)_controller.Delete(3).Result;

            Assert.AreEqual(result.ViewName, "Delete");
        }

        [TestMethod]
        public void DeleteConfirmedValidIdDeletesProduct()
        {
            Product product = _context.Products.Find(3);
            Assert.IsNotNull(product);
            _controller.DeleteConfirmed(3);
            product = _context.Products.Find(3);
            Assert.IsNull(product);
        }

        #endregion
    }
}
