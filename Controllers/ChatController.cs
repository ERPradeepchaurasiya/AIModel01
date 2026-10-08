using JJMModel.Models;
using JJMModel.Services;
using Microsoft.AspNetCore.Mvc;

namespace JJMModel.Controllers
{
    public class ChatController : Controller
    {
        private readonly JJMChatService _chatService;

        public ChatController(JJMChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Chat(ChatRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Message))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Message cannot be empty."
                });
            }

            try
            {
                string response =
                    _chatService.Predict(request.Message);

                response = FormatResponse(response);

                return Json(new
                {
                    Success = true,
                    Response = response
                });
            }
            catch (System.Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        private string FormatResponse(string raw)
        {
            if (string.IsNullOrEmpty(raw))
                return string.Empty;

            var text =
                System.Net.WebUtility.HtmlEncode(raw);

            // Convert URLs to clickable links
            text = System.Text.RegularExpressions.Regex.Replace(
                text,
                @"((http|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?)",
                "<a href=\"$1\" target=\"_blank\" rel=\"noopener noreferrer\">$1</a>"
            );

            var parts = text.Split(
                new[] { " | " },
                System.StringSplitOptions.None);

            if (parts.Length > 1)
            {
                var html =
                    "<div class='formatted-response'>";

                foreach (var part in parts)
                {
                    if (part.StartsWith("Steps:"))
                    {
                        html +=
                            "<div class='step-item'>" +
                            "<strong>Steps:</strong> " +
                            part.Substring(6).Trim() +
                            "</div>";
                    }
                    else if (part.StartsWith("Official link:"))
                    {
                        html +=
                            "<div class='link-item'>" +
                            "<strong>Official Link:</strong> " +
                            part.Substring(14).Trim() +
                            "</div>";
                    }
                    else
                    {
                        html +=
                            "<div class='info-item'>• " +
                            part.Trim() +
                            "</div>";
                    }
                }

                html += "</div>";

                return html;
            }

            return text;
        }
    }
}