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
        public JsonResult Chat(string Message1)
        {
            ChatRequest request = new ChatRequest();
            request.Message = Message1;
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return Json(new
                {
                    Success = false,
                    Message = "Message cannot be empty."
                });
            }
            try
            {
                string response = _chatService.Predict(request.Message);

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

            // Line breaks ko " | " me convert kar deta hai taaki multi-line text bhi format ho sake
            raw = raw.Replace("\r\n", " | ").Replace("\n", " | ");

            var text = System.Net.WebUtility.HtmlEncode(raw);

            // Convert URLs to clickable links
            text = System.Text.RegularExpressions.Regex.Replace(
                text,
                @"((http|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?)",
                "<a href=\"$1\" target=\"_blank\" rel=\"noopener noreferrer\">$1</a>"
            );

            var parts = text.Split(
                new[] { " | " },
                System.StringSplitOptions.RemoveEmptyEntries);

            var html = "<div class='formatted-response'>";

            foreach (var part in parts)
            {
                string trimmedPart = part.Trim();
                if (string.IsNullOrEmpty(trimmedPart)) continue;

                if (trimmedPart.StartsWith("Steps:", System.StringComparison.OrdinalIgnoreCase))
                {
                    html += "<div class='step-item'><strong>Steps:</strong> " + trimmedPart.Substring(6).Trim() + "</div>";
                }
                else if (trimmedPart.StartsWith("Official link:", System.StringComparison.OrdinalIgnoreCase) ||
                         trimmedPart.StartsWith("Official Link:", System.StringComparison.OrdinalIgnoreCase))
                {
                    int colonIndex = trimmedPart.IndexOf(':');
                    string linkContent = colonIndex >= 0 ? trimmedPart.Substring(colonIndex + 1).Trim() : trimmedPart;
                    html += "<div class='link-item'><strong>Official Link:</strong> " + linkContent + "</div>";
                }
                else
                {
                    html += "<div class='info-item'>• " + trimmedPart + "</div>";
                }
            }

            html += "</div>";
            return html;
        }
    }
}