using CarShop.Data;
using CarShop.Models.Accounts;
using CarShop.Models.Chat;
using CarShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using CarShop.Hubs;
using Microsoft.AspNetCore.Authorization;

namespace CarShop.Controllers;


[Authorize]
public class ChatController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHubContext<ChatHub> _chatHub;

    public ChatController(ApplicationDbContext context, UserManager<AppUser> userManager, IHubContext<ChatHub> chatHub)
    {
        _context = context;
        _userManager = userManager;
        _chatHub = chatHub;
    }

    public async Task<IActionResult> CheckForConverstaion(string? id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser.Id == id)
            {
                throw new Exception("You cant chat with yourself!");
            }

            ViewData["CurrentUserId"] = currentUser.Id;

            var channel = await _context.ChatChannels
                .AsNoTracking()
                .FirstOrDefaultAsync(c => (c.User1Id == currentUser.Id && c.User2Id == id) ||
                                          (c.User1Id == id && c.User2Id == currentUser.Id));


            if (channel == null)
            {
                channel = new ChatChannel
                {
                    User1Id = currentUser.Id,
                    User2Id = id
                };

                _context.Add(channel);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return RedirectToAction("Index", new
            {
                activeChannelId = channel.Id
            });
        }
        else
        {
            throw new Exception($"Something went wrong on chat loading for {id} user");
        }
    }

    public async Task<IActionResult> Index(int? activeChannelId)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        ViewData["CurrentUserId"] = currentUser.Id;
        var chatViewModel = new ChatViewModel();


        var chatChannels = _context.ChatChannels
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Include(c => c.Messages)
            .Where(c => c.User1Id == currentUser.Id || c.User2Id == currentUser.Id)
            .ToList();

        if (activeChannelId.HasValue)
        {
            var activeChatChannel = chatChannels.FirstOrDefault(c => c.Id == activeChannelId);

            

            chatViewModel.ActiveChannel = activeChatChannel;
        }

        chatViewModel.ChatChannels = chatChannels;

        return View(chatViewModel);
    }

    //public async Task<IActionResult> SendMessage([Bind("Content,ReceiverId,SenderId,ChatChannelId")] Message msg)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        await _context.Messages.AddAsync(msg);
    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (Exception e)
    //        {
    //            throw new Exception(e.Message);
    //        }
    //    }

    //    await _chatHub.Clients.Group(msg.ChatChannelId.ToString()).SendAsync("ReceiveMessage", msg);

    //    return Ok();
    //}
}
