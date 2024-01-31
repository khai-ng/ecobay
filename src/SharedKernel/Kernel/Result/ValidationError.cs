namespace SharedKernel.Kernel.Result
{
    public class ValidationError
    {
        public ValidationError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public ValidationError(string identifier, string errorMessage, ValidationSeverity severity)
        {
            Identifier = identifier;
            ErrorMessage = errorMessage;
            Severity = severity;
        }

        public string Identifier { get; set; }
        public string ErrorMessage { get; set; }
        public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
    }
}
