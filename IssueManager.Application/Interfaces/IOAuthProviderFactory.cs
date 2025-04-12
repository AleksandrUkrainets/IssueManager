using IssueManager.Domain.Interfaces;

namespace IssueManager.Application.Interfaces
{
    public interface IOAuthProviderFactory
    {
        IOAuthProvider? Create(string provider);
    }
}
