using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc_test.Data;
using mvc_test.Models;

namespace mvc_test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExpensesDataAccess _expensesDataAccess;

        public HomeController(ILogger<HomeController> logger, ExpensesDataAccess expensesDataAccess)
        {
            _logger = logger;
            _expensesDataAccess = expensesDataAccess;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Expenses()
        {
            var expenses = await _expensesDataAccess.GetAllExpensesAsync();
            var total = expenses.Sum(e=>e.Price);
            ViewBag.Total = total;
            return View(expenses);
        }

        public async Task<IActionResult> CreateEditExpense(int? id)
        {
            if (id != null)
            {
                var expenseToEdit = await _expensesDataAccess.GetExpenseAsync(id);
                if (expenseToEdit != null) return View(expenseToEdit);
            }
            return View();
        }

        public async Task<IActionResult> CreateEditExpenseForm(Expense model)
        {
            if (model.Id == 0)
            {
                _ = await _expensesDataAccess.CreateExpenseAsync(model);
            }
            else
            {
                _ = await _expensesDataAccess.UpdateExpenseAsync(model);
            }
            return RedirectToAction("Expenses");
        }


        public async Task<IActionResult> DeleteExpense(int id)
        {
            var deleted = await _expensesDataAccess.DeleteExpenseAsync(id);
            if (deleted <= 0)
            {
                return View();
            }
            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
