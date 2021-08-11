namespace GreenDoorProject.Controllers
{
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Models.Member;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MemberController : Controller
    {
        private readonly GreenDoorProjectDbContext data;

        public MemberController(GreenDoorProjectDbContext data) 
            => this.data = data;

        [Authorize]
        public IActionResult BecomeMember()
        {
            return View(new AddMemberFormModel
            {
                MembershipTypes = this.GetMembershipTypes()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult BecomeMember(string userId, string membershipType)
        {
            var user = this.data.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            var price = 0.00m;
            var membershipDuration = 0;

            if (membershipType == "OneMonth")
            {
                price = 9.99m;
                membershipDuration = 1;
            }
            else if (membershipType == "ThreeMonth")
            {
                price = 26.99m;
                membershipDuration = 3;
            }
            else if (membershipType == "SixMonth")
            {
                price = 50.99m;
                membershipDuration = 6;
            }
            else if (membershipType == "Annual")
            {
                price = 89.99m;
                membershipDuration = 12;
            }

            var membership = new Membership
            {
                Name = membershipType,
                Price = price
            };

            var memeber = new Member
            {
                MembershipId = membership.Id,
                MembershipStart = DateTime.UtcNow,
                MembershipEnd = DateTime.UtcNow.AddMonths(membershipDuration),
                UserId = userId
            };

            return View();
        }

        private IEnumerable<MembershipTypesViewModel> GetMembershipTypes()
            => this.data.Memberships
                .Select(m => new MembershipTypesViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price
                })
                .ToList();
    }
}
