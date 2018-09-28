using Data;
using Microsoft.EntityFrameworkCore;
using System;
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





    }
}
