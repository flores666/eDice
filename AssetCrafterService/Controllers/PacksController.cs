using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetCrafterService.Controllers;

[Authorize]
[ApiController]
[Route("/packs")]
public class PacksController : Controller
{
    
}