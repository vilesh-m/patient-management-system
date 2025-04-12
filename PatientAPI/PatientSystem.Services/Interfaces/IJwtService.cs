namespace PatientSystem.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username, List<string> roles, int expiryMinutes = 5);
    }
}