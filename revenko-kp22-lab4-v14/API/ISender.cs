using Microsoft.AspNetCore.Mvc;

namespace API {
    public interface ISender {
        public Task<IActionResult> SendMessage(string message, string rKey);
    }
}
