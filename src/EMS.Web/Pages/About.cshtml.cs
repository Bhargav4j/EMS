using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.Web.Pages;

public class AboutModel : PageModel
{
    public string Message { get; set; } = string.Empty;

    public void OnGet()
    {
        Message = "Your application description page.";
    }
}
