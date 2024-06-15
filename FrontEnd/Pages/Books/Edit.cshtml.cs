using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookLibraryManagementSystem.Models;

public class EditModel : PageModel
{
    private readonly HttpClient _httpClient;

    public EditModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [BindProperty]
    public Book Book { get; set; }

    [BindProperty]
    public List<Author> Authors { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Book = await _httpClient.GetFromJsonAsync<Book>($"https://localhost:5001/api/books/{id}");
        if (Book == null)
        {
            return NotFound();
        }

        Authors = Book.Authors.ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Book.Authors = Authors;
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5001/api/books/{Book.Id}", Book);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Books/Index");
        }

        return Page();
    }
}
