using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;

namespace NotikaIdentityEmail.ViewComponents.MessageViewComponents
{
    public class _MessageSidebarComponentPartial : ViewComponent
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;
        public _MessageSidebarComponentPartial(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.sendMessageCount = _context.Messages.Where(x => x.SenderEmail == user.Email).Count();
            ViewBag.receiveMessageCount = _context.Messages.Where(x => x.ReceiverEmail == user.Email).Count();
            return View();
        }
    }
}
