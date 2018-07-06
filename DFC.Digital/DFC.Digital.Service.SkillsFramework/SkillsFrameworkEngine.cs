namespace DFC.Digital.Service.SkillsFramework
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Interface;
    using DFC.Digital.Data.Model;
    using Repository.ONET.Interface;


    public class SkillsFrameworkEngine:IBusinessRuleEngine 
    {
        private readonly ISkillsFrameworkRepository _repository;

        // Business Rule Engine implemetation 
        // Repository will be called with the Expression predicate (Rule Engine)
        public SkillsFrameworkEngine( ISkillsFrameworkRepository repository )
        {
            _repository = repository;
        }

        #region Implementation of IBusinessRuleEngine

        public async Task<SkillsFramework> GetSkillsFrameworkFor(string socCode)
        {
           var result= _repository.GetAttributesValuesAsync<DfcGdsTranslation>(s=>s.SocCode==socCode).Result.
                Select(skills => new SkillsFramework {SocCode = skills.SocCode}).
                FirstOrDefault();
            return await Task.FromResult ( result).ConfigureAwait ( false );
        }

        #endregion
    }
}
