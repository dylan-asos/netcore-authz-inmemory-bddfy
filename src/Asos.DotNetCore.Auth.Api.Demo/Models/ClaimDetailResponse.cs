namespace Asos.DotNetCore.Auth.Api.Demo.Models;

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