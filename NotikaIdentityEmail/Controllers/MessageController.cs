using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using NotikaIdentityEmail.Models.MessageViewModels;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.SenderEmail equals u.Email into userGroup
                          from sender in userGroup.DefaultIfEmpty()

                          join c in _context.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.ReceiverEmail == user.Email
                          select new MessageWithSenderInfoViewModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              SenderEmail = m.SenderEmail,
                              SenderName = sender != null ? sender.Name : "Unknown",
                              SenderSurname = sender != null ? sender.Surname : "User",
                              CategoryName = category != null ? category.CategoryName : "No category"
                          }).ToList();

            return View(values);
        }

        public async Task<IActionResult> Sendbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.ReceiverEmail equals u.Email into userGroup
                          from receiver in userGroup.DefaultIfEmpty()

                          join c in _context.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.SenderEmail == user.Email
                          select new MessageWithReceiverInfoViewModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              ReceiverEmail = m.ReceiverEmail,
                              ReceiverName = receiver != null ? receiver.Name : "Unknown",
                              ReceiverSurname = receiver != null ? receiver.Surname : "User",
                              CategoryName = category != null ? category.CategoryName : "No category"
                          }).ToList();

            return View(values);
        }

        public IActionResult MessageDetail(int id)
        {
            var value = _context.Messages.Where(x => x.MessageId == id).FirstOrDefault();
            return View(value);
        }

        [HttpGet]
        public IActionResult ComposeMessage()
        {
            var categories = _context.Categories.ToList();
            ViewBag.v = categories.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.CategoryId.ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ComposeMessage(Message message)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            message.SenderEmail = user.Email;
            message.SendDate = DateTime.Now;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Sendbox");
        }

        public async Task<IActionResult> GetMessageListByCategory(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var values = (from m in _context.Messages
                          join u in _context.Users
                          on m.SenderEmail equals u.Email into userGroup
                          from sender in userGroup.DefaultIfEmpty()

                          join c in _context.Categories
                          on m.CategoryId equals c.CategoryId into categoryGroup
                          from category in categoryGroup.DefaultIfEmpty()

                          where m.ReceiverEmail == user.Email && m.CategoryId == id
                          select new MessageWithSenderInfoViewModel
                          {
                              MessageId = m.MessageId,
                              MessageDetail = m.MessageDetail,
                              Subject = m.Subject,
                              SendDate = m.SendDate,
                              SenderEmail = m.SenderEmail,
                              SenderName = sender != null ? sender.Name : "Unknown",
                              SenderSurname = sender != null ? sender.Surname : "User",
                              CategoryName = category != null ? category.CategoryName : "No category"
                          }).ToList();

            return View(values);
        }
    }
}
