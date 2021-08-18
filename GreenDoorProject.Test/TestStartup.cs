//namespace GreenDoorProject.Test
//{
//    using MyTested.AspNetCore.Mvc;
//    using Microsoft.Extensions.DependencyInjection;
//    using Xunit;
//    using GreenDoorProject.Services.Books;
//    using GreenDoorProject.Services.Movies;
//    using GreenDoorProject.Services.Music;
//    using GreenDoorProject.Services.Members;
//    using GreenDoorProject.Services.Patrons;
//    using Microsoft.Extensions.DependencyInjection.Extensions;

//    public class TestStartup : Startup
//    {
//        public void ConfigurationTestServices(IServiceCollection services)
//        {
//            base.ConfigureServices(services);

//            services.Replace<IBookService, MockedBookService>();
//            services.Replace<IMovieService, MockedMovieService>();
//            services.Replace<IMusicService, MockedMusicService>();
//            services.Replace<IPatronService, MockedPatronService>();
//            services.Replace<IMemberService, MockedMemberService>();

//        }
//    }
//}
