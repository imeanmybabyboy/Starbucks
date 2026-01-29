namespace ASP_Starbucks.Models.Responses
{
    public class ApiResponse
    {
        public string Status { get; set; } = null!;
        public object? Data { get; set; }
        public string? Message { get; set; }

        public static ApiResponse Ok(object? data = null, string? message = null) => new() { Status = "Ok", Data = data, Message = message };
        public static ApiResponse Error(string message) => new() { Status = "Error", Message = message };
    }
}
