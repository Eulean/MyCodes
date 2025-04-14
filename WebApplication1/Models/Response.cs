using System;

namespace WebApplication1.Models;

public class Response
{
    public string Message { get; set; }
    public bool Success { get; set; }
    public object Data { get; set; }

    public Response(string message, bool success, object data )
    {
        Message = message;
        Success = success;
        Data = data;
    }

}
