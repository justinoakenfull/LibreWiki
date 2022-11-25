namespace LibreWiki.Models
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = "";
#nullable enable
        public T? ObjectResult { get; set; }
#nullable disable

    }
}
