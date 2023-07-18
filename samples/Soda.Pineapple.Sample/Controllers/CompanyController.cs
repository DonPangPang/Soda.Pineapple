using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Soda.Pineapple.Sample.Data;
using Soda.Pineapple.Sample.Domain;
using Soda.Pineapple.Services.PublicServices;

namespace Soda.Pineapple.Sample.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CompanyController:ControllerBase
{
    private readonly AutoDbContext<SampleDbContext> _autoDbContext;

    public CompanyController(AutoDbContext<SampleDbContext> autoDbContext)
    {
        _autoDbContext = autoDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        var res = await _autoDbContext.Table<Company>().ToListAsync();
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> SetCompany([FromBody] Company company)
    {
        _autoDbContext.Set<Company>().Add(company);
        var res = await _autoDbContext.Db.SaveChangesAsync();

        return Ok(res > 0);
    }
}