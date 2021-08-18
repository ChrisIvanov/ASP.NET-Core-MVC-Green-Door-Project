namespace GreenDoorProject.Test.Mocks
{
    using GreenDoorProject.Data;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;

    public class DatabaseMock
    {
        public static GreenDoorProjectDbContext Instance
        {
            get
            {
                var options = new DbContextOptionsBuilder<GreenDoorProjectDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new GreenDoorProjectDbContext(options);
            }
        }

        
    }
}
