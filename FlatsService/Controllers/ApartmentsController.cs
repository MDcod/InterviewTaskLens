using FlatsService.DbContext.Helpers;
using FlatsService.DbContext.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlatsService.Controllers;

[ApiController]
[Route("[controller]")]
public class ApartmentsController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Apartment> Get()
    {
        using var openSqliteConnection = Default.OpenSqliteConnection();
        return QueryHelpers.GetApartments(openSqliteConnection);
    }
}