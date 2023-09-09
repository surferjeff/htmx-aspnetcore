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
            await Task.Delay(TimeSpan.FromSeconds(1));
            // handle Htmx request
            return Request.IsHtmx()
                ? Validate(this) ?? Partial("_Form", this)
                : Page();
        }

        // If the request query contains ?validate=FieldName, then return
        // the results of validating that field.
        static public IActionResult? Validate(PageModel pageModel) {
            if (pageModel.Request.Query["validate"].Count > 0) {
                var field = pageModel.Request.Query["validate"][0];
                var errors = pageModel.ModelState[field].Errors;
                var id = System.Net.WebUtility.HtmlEncode(field);
                foreach (var error in errors) {
                    var text = System.Net.WebUtility.HtmlEncode(error.ErrorMessage);
                    var scrip = $"<script>document.getElementById('{id}').classList.add('input-validation-error')</script>";
                    return pageModel.Content(text + scrip, "text/html");
                }
                var script = $"<script>document.getElementById('{id}').classList.remove('input-validation-error')</script>";
                return pageModel.Content(script, "text/html");
            }
            return null;
        }
    }
}