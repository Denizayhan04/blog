using BlogProject.Data;
using BlogProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogProject.Controllers;

[Authorize] // Sadece giriş yapanlar CRUD yapabilir (Read hariç, o Home'da)
public class PostsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Post post)
    {
        post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Aktif kullanıcı ID'si
        post.CreatedAt = DateTime.Now;

        ModelState.Remove("UserId");
        ModelState.Remove("User");

        if (ModelState.IsValid)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        return View(post);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        // Sadece kendi postunu düzenleyebilir
        if (post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Post updatedPost)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        if (post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

        post.Content = updatedPost.Content;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        if (post.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}