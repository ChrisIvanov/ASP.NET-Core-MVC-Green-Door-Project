namespace GreenDoorProject.Test.AdminControllers
{
    using GreenDoorProject.Areas.Admin.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using MyTested.AspNetCore.Mvc;
    using System;
    using Xunit;

    using GreenDoorProject.Test.Mocks;
    using FakeItEasy;
    using GreenDoorProject.Data;

    public class BooksController
    {
        [Fact]
        public void AllUsersMethodShouldReturnListWithUsers()
        {
            //Arrange
            var data = A.Fake<GreenDoorProjectDbContext>();
            var controller = new BooksController();


        }
    }
}
