using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WikiTechWebApp.Areas.Identity.Data;

namespace WikiTechWebApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IbanModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IbanModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        public string Iban { get; set; }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "New Iban")]
            public string NewIban { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            //var Iban = await _userManager.GetIbanAsync(user);
            Iban = Iban;

            Input = new InputModel
            {
                NewIban = Iban,
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeIbanAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var Iban = await _userManager.GetEmailAsync(user);
            if (Input.NewIban != Iban)
            {
                var userId = await _userManager.GetUserIdAsync(user);

                StatusMessage = "Confirmation link to change Iban sent. Please check your Iban.";
                return RedirectToPage();
            }

            StatusMessage = "Your Iban is unchanged.";
            return RedirectToPage();
        }

    }
}
