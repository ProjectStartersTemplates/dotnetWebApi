global using Entities.Models;
global using Microsoft.EntityFrameworkCore;


namespace Repository.Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
                 await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

        public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
        {
            var companies = await FindAll(trackChanges)
            .OrderBy(e => e.Name).ToListAsync();
            return PagedList<Company>.ToPagedList(companies, companyParameters.PageNumber, companyParameters.PageSize);
        }

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
                await FindByCondition(c => c.Id.Equals(companyId), trackChanges)
                 .SingleOrDefaultAsync();

        public void CreateCompany(Company company) => Create(company);



        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
                 await FindByCondition(x => ids.Contains(x.Id), trackChanges)
                 .ToListAsync();


        public void DeleteCompany(Company company)
        {
            Delete(company);
        }

    }

}
