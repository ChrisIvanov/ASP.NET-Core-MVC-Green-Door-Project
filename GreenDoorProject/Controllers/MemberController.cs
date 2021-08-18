namespace GreenDoorProject.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GreenDoorProject.Data;
    using GreenDoorProject.Data.Models;
    using GreenDoorProject.Infrastructure;
    using GreenDoorProject.Models.Member;
    using GreenDoorProject.Services.Members;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class MemberController : Controller
    {
        private readonly GreenDoorProjectDbContext data;
        private readonly IMemberService members;

        public MemberController(
            GreenDoorProjectDbContext data,
            IMemberService members)
        {
            this.data = data;
            this.members = members;
        }

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
        public IActionResult BecomeMember(AddMemberFormModel model)
        {
            var userId = this.User.GetId();

            if (members.IsMember(userId))
            {
                ModelState.AddModelError(string.Empty, "You are already a member.");

                return RedirectToAction("Index", "Home");
            }

            var guest = this.data.Users
                .Where(u => u.Id == User.GetId())
                .FirstOrDefault();

            var membership = this.data.Memberships
               .Where(m => m.Id == int.Parse(model.MembershipTypeId))
               .FirstOrDefault();

            var membershipDuration = 0;

            if (membership.Name == "One Month") membershipDuration = 1;
            else if (membership.Name == "Three Months") membershipDuration = 3;
            else if (membership.Name == "Six Months") membershipDuration = 6;
            else if (membership.Name == "Annual") membershipDuration = 12;

            var member = new Member
            {
                MembershipId = membership.Id,
                MembershipStart = DateTime.UtcNow,
                MembershipEnd = DateTime.UtcNow.AddMonths(membershipDuration),
                UserId = guest.Id
            };

            this.data.Members.Add(member);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public bool GuestIsMember(string guestId)
            => this.data.Members
                    .Any(u => u.UserId == guestId);

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