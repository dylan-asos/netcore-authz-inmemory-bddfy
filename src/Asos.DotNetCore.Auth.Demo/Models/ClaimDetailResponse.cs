namespace Asos.DotNetCore.Auth.Demo.Models;

public class ClaimDetailResponse
{
    public ClaimDetailResponse(string claimType, string claimValue)
    {
        ClaimType = claimType;
        ClaimValue = claimValue;
    }

    public string ClaimType { get; }

    public string ClaimValue { get; }
}