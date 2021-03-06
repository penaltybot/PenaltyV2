﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PenaltyV2.Data;
using PenaltyV2.Models;

namespace PenaltyV2.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _dbContext = dbContext;
        }
        [Display(Name = "Utilizador")]
        public string Username { get; set; }

        public SelectList FavoriteTeam { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public Byte[] UserImg { get; set; }

        public string Message { get; set; }

        public bool IsValid { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "E-Mail")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Equipa Favorita")]
            public string FavoriteTeam { get; set; }
            [NotMapped]
            [Display(Name = "Personalizar Avatar")]
            [AllowFileSize(FileSize = 1 * 1024 * 1024, ErrorMessage = "Máximo de ficheiros permitidos é 1 MB.")]
            public IFormFile ImageFile { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            ViewData["Ligas"] = Database.LoadUserLeagues(User.Identity.Name, _dbContext);

            List<Teams> teams = Database.GetTeams(_dbContext);
            FavoriteTeam = new SelectList(teams, "Name", "Name");
            

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Usersinfo usersinfo = Database.GetUserInfo(user.UserName, _dbContext);
            //UserImg = File(usersinfo.UserImg, "image/png");
            UserImg = usersinfo.UserImg;
            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                FavoriteTeam = usersinfo.Favoriteteam,            
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (ModelState.IsValid)
            {


                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }               
                if (Input.Email != email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                    }
                }           
                if (Input.PhoneNumber != phoneNumber)
                {
                    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                    if (!setPhoneResult.Succeeded)
                    {
                        var userId = await _userManager.GetUserIdAsync(user);
                        throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                    }
                }

                Byte[] fileBytes = null;
                if (Input.ImageFile != null)
                {
                    if (Input.ImageFile.Length > 0)
                    {
                        IFormFile files = Input.ImageFile;
                        using (var target = new MemoryStream())
                        {
                            files.CopyTo(target);
                            fileBytes = target.ToArray();
                        }

                    }
                }


                //Alterar dados tabela Usersinfo
                UpdateUserInfo(new Usersinfo
                {
                    Username = user.UserName,
                    Favoriteteam = Input.FavoriteTeam,
                    UserImg = fileBytes
                });

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "O teu perfil foi atualizado!";
            }else
            {

                await OnGetAsync();
                return Page();
            }

            
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
        public void UpdateUserInfo(Usersinfo userupdatedinfo)
        {
            Usersinfo userinfo = Database.GetUserInfo(userupdatedinfo.Username, _dbContext);
            if(userinfo.Favoriteteam != userupdatedinfo.Favoriteteam)
            {
                userinfo.Favoriteteam = userupdatedinfo.Favoriteteam;
            }

            if(userupdatedinfo.UserImg != null)
            {
                userinfo.UserImg = userupdatedinfo.UserImg;
            }
           
            _dbContext.Update(userinfo);
            _dbContext.SaveChanges();

        }

    }
}
