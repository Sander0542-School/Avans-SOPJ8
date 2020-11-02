namespace Bumbo.Data
{
    public class RepositoryWrapper
    {
        private readonly ApplicationDbContext _repositoryContext;

        // Add implemented repositories below


        public RepositoryWrapper(ApplicationDbContext context)
        {
            _repositoryContext = context;
        }
    }
}