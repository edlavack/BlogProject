﻿using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IBlogPostService _blogPostService;
        private readonly IEmailSender _emailService;
        private readonly UserManager<BlogUser> _userManager;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context, IBlogPostService blogPostService, IEmailSender emailService, UserManager<BlogUser> userManager )
        {
            _logger = logger;
            _context = context;
            _blogPostService = blogPostService;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<IActionResult> AuthorPage( int? pageNum)
        {
            int pageSize = 4;
            int page = pageNum ?? 1;

            IEnumerable<BlogPost> posts = (await _blogPostService.GetAllBlogPostsAsync()).Where(b=> b.IsPublished == true);
            IPagedList<BlogPost> blogPosts = await posts.ToPagedListAsync(page, pageSize);

            return View(posts);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult>ContactMe(string? id)
        {
            string blogUserId = _userManager.GetUserId(User);
            string blogUserEmail = (await _context.Users.FirstOrDefaultAsync(b => b.Id == blogUserId))!.Email;

            if(blogUserId == null)
            {
                return NotFound();
            }

            EmailData emailData = new EmailData()
            {
                EmailAddress = blogUserEmail
                
            };



            return View(emailData);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult>ContactMe(EmailData emailData)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _emailService.SendEmailAsync(emailData.EmailAddress!, emailData.Subject!, emailData.Body!);
                    return RedirectToAction("AuthorPage", "Home", new { swalMessage = "Success: Email Sent" });
                }
                catch
                {
                    return RedirectToAction("ContactMe", "Home", new { swalMessage = "Error: Email Send Failed" });
                    throw;
                }
            }
            return View(emailData);
        }
    }
}