using Microsoft.AspNetCore.Mvc;

namespace internship_entry_task.Api.Controllers.Abstract;

/// <summary>
/// Абстрактный контроллер API версии 1.
/// </summary>
/// <remarks>
/// Этот абстрактный контроллер используется как базовый для всех контроллеров API версии 1,
/// наследуя функциональность базового контроллера авторизации.
/// </remarks>
[ApiController]
[Route("v1/[controller]")]
public class ApiControllerV1 : ControllerBase
{
    
}