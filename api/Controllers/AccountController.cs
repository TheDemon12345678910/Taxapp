using api;
using api.dtoModels;
using api.filters;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace Taxapp.Controllers;

/**
 * This controller class controls the accounts
 */
public class AccountController: ControllerBase
{
    private readonly AccountService _service;
    private JwtService _jwtService;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("/account/login")]
    public ResponseDto Login([FromBody] LoginDto dto)
    {
            var user = _service.Authenticate(dto.email, dto.password);
            //Setting the sessionData here
            var token = _jwtService.IssueToken(SessionData.FromUser(user!));
            return new ResponseDto
            {
                MessageToClient = "Successfully authenticated",
                ResponseData = new { token },
            };
    }
    

    [HttpPost]
    [Route("/account/register")]
    public ResponseDto Register([FromBody] RegisterDto dto)
    {
        Console.WriteLine("Hi Im: \t\t" + dto.username + "\nmy number:\t" + dto.tlfnumber.ToString() + "\nmy email:\t" + dto.email + "\nPassword:\t" + dto.password);
        var user = _service.Register(dto.username, dto.tlfnumber, dto.email, dto.password);
        return new ResponseDto
        {
            MessageToClient = "Successfully registered",
            ResponseData = user
        };
    }
    
    [Route("/account/logout")]
    public ResponseDto Logout()
    {
        //Clears the session
        HttpContext.Session.Clear();
        return new ResponseDto()
        {
            MessageToClient = "Successfully logged out"
        };
    }


    /**
     * Calls the method, to find out, who they are. Using the Session data, the method
     * returns a person
     */
    [RequireAuthentication]
    [HttpGet]
    [Route("/account/whoami")]
    public ResponseDto WhoAmI()
    {
        var data = HttpContext.GetSessionData();
        var user = _service.Get(data);
        return new ResponseDto
        {
            ResponseData = user
        };
    }
}