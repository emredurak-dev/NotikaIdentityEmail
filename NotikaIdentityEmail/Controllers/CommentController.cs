using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotikaIdentityEmail.Context;
using NotikaIdentityEmail.Entities;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotikaIdentityEmail.Controllers
{
    public class CommentController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult UserComments()
        {
            var values = _context.Comments.Include(x => x.AppUser).ToList();
            return View(values);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult UserCommentList()
        {
            var values = _context.Comments.Include(x => x.AppUser).ToList();
            return View(values);
        }

        [HttpGet]
        public PartialViewResult CreateComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            comment.AppUserId = user.Id;
            comment.CommentDate = DateTime.Now;

            //toxic bert
            using (var client = new HttpClient())
            {
                var apiKey = "YOUR-TOXIC-BERT-API-KEY";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                try
                {
                    var translateRequestBody = new
                    {
                        inputs = comment.CommentDetail
                    };
                    var translateJson = JsonSerializer.Serialize(translateRequestBody);
                    var translateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");
                    var translateResponse = await client.PostAsync("YOUR-TOXIC-BERT-API-URL", translateContent);
                    var translateResponseString = await translateResponse.Content.ReadAsStringAsync();
                    string englishText = comment.CommentDetail;
                    if (translateResponseString.TrimStart().StartsWith("["))
                    {
                        var translateDoc = JsonDocument.Parse(translateResponseString);
                        englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
                    }
                    var toxicRequestBody = new
                    {
                        inputs = englishText
                    };

                    var toxicJson = JsonSerializer.Serialize(toxicRequestBody);
                    var toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");
                    var toxicResponse = await client.PostAsync("YOUR-TOXIC-BERT-API-URL", toxicContent);
                    var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

                    if (toxicResponseString.TrimStart().StartsWith("["))
                    {
                        var toxicDoc = JsonDocument.Parse(toxicResponseString);
                        foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
                        {
                            string label = item.GetProperty("label").GetString();
                            double score = item.GetProperty("score").GetDouble();
                            if (score > 0.5)
                            {
                                comment.CommentStatus = "Toxic Comment";
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(comment.CommentStatus))
                    {
                        comment.CommentStatus = "Pending";
                    }
                }
                catch (Exception ex)
                {
                    comment.CommentStatus = "Pending";
                }

                _context.Comments.Add(comment);
                _context.SaveChanges();
                return RedirectToAction("UserCommentList");
            }
        }
        
        public IActionResult DeleteComment(int id)
        {
            var value = _context.Comments.Find(id);
            _context.Comments.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("UserCommentList");
        }

        public IActionResult CommentStatusChangeToToxic(int id)
        {
            var value = _context.Comments.Find(id);
            value.CommentStatus = "Toxic Comment";
            _context.SaveChanges();
            return RedirectToAction("UserCommentList");
        }

        public IActionResult CommentStatusChangeToPassive(int id)
        {
            var value = _context.Comments.Find(id);
            value.CommentStatus = "Passive";
            _context.SaveChanges();
            return RedirectToAction("UserCommentList");
        }

        public IActionResult CommentStatusChangeToActive(int id)
        {
            var value = _context.Comments.Find(id);
            value.CommentStatus = "Approved";
            _context.SaveChanges();
            return RedirectToAction("UserCommentList");
        }
    }
}
