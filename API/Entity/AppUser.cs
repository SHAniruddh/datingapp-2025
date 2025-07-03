namespace API.Entity;

public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string  DisplayNamwe { get; set; }
    public  required string Email { get; set; }
}
