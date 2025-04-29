// Import necessary namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyWebsite.Areas.Identity.Data;
using MyWebsite.Data;
using MyWebsite.Models;

namespace MyWebsite.Controllers
{
    // Define the controller with authorization attribute
    [Authorize]
    public class CommentController : Controller
    {
        // Dependency injection for the database context and user manager
        private readonly MyCommentsContext _context;
        private readonly UserManager<MyWebsiteUser> _userManager;

        public CommentController(MyCommentsContext context, UserManager<MyWebsiteUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action to display comments (AllowAnonymous for public access)
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            var comments = _context.CommentModel.AsQueryable();

            switch (sortOrder)
            {
                case "name_desc":
                    comments = comments.OrderByDescending(c => c.Name);
                    break;
                case "date":
                    comments = comments.OrderBy(c => c.Posted);
                    break;
                case "name":
                    comments =  comments.OrderBy(c => c.Name);
                    break;
                default:
                    comments = comments.OrderByDescending(c => c.Posted);
                    break;
            }
            return View(comments.ToList());
        }

        // Action to display index after logging in
        public async Task<IActionResult> Create()
        {
            return RedirectToAction(nameof(Index));
        }

        // Action to handle comment creation form submission
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CommentText")] Comment comment)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("Posted");
            ModelState.Remove("Name");
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                comment.UserId = user.Id;
                comment.Name = user.Name;
                comment.Posted = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Action to render the comment editing form
        public async Task<IActionResult> Edit(int? id)
        {
            // Fetch the comment by ID
            // Check if the logged-in user is authorized to edit the comment
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.CommentModel.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorized(comment.UserId))
            {
                return Forbid();
            }
            return View(comment);
        }

        // Action to handle comment editing form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CommentText")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            ModelState.Remove("UserId");
            ModelState.Remove("Posted");
            ModelState.Remove("Name");
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                try
                {
                    comment.UserId = user.Id;
                    comment.Name = user.Name;
                    comment.Posted = DateTime.Now;
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentModelExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        // Action to render the comment deletion confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.CommentModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorized(comment.UserId))
            {
                return Forbid();
            }

            return View(comment);
        }

        // Action to handle comment deletion confirmation submission
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentModel = await _context.CommentModel.FindAsync(id);
            if (commentModel != null)
            {
                _context.CommentModel.Remove(commentModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a comment with a specific ID exists
        private bool CommentModelExists(int id)
        {
            return _context.CommentModel.Any(e => e.Id == id);
        }

        // Helper method to check if the logged-in user is authorized to perform actions on a comment
        private bool IsUserAuthorized(string commentUserId)
        {
            var loggedInUserId = _userManager.GetUserId(User);
            return commentUserId == loggedInUserId;
        }
    }
}
