namespace GreenDoorProject.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using GreenDoorProject.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using static Areas.Guest.GuestConstants;
    using static Data.DataConstants.Guest;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Guest> signInManager;
        private readonly UserManager<Guest> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterModel(
            UserManager<Guest> userManager,
            SignInManager<Guest> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "Full Name")]
            [StringLength(GuestnameMaxLength, MinimumLength = GuestnameMinLength)]
            public string FullName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (!await roleManager.RoleExistsAsync(GuestRoleName))
                {
                    var role = new IdentityRole { Name = GuestRoleName };

                    await roleManager.CreateAsync(role);
                }

                var guest = new Guest 
                { 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    FullName = Input.FullName
                };

                var result = await this.userManager.CreateAsync(guest, Input.Password);

                await userManager.AddToRoleAsync(guest, GuestRoleName);

                if (result.Succeeded)
                {
                    await this.signInManager.SignInAsync(guest, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
