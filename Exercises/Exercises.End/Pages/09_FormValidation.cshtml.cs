using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exercises.Pages
{
    public class FormValidation : PageModel
    {
        [BindProperty, Required]
        public string? Name { get; init; } = string.Empty;

        [BindProperty, Required]
        public int? Age { get; init; } = null!;

        public async Task<IActionResult> OnPost()
        {
            // see the loading spinner (remove for production)
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            // handle Htmx request
            if (Request.IsHtmx()) {
                if (Request.Query["validate"].Count > 0) {
                    var field = Request.Query["validate"][0];
                    var errors = ModelState[field].Errors;
                    foreach (var error in errors) {
                        var text = System.Net.WebUtility.HtmlEncode(error.ErrorMessage);
                        return Content(text, "text/html");
                    }
                    return Content("", "text/html");                    
                }
                return Partial("_Form", this);
            } else {
                return Page();
            }
        }
    }
}