namespace DFC.Digital.Service.SkillsFramework
{
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Interface;
    using DFC.Digital.Data.Model;
    public class SkillsFramework:IBusinessRuleEngine
    {
        private readonly IRepository<SkillsFramework> _repository;

        // Business Rule Engine implemetation 
        // Repository will be called with the Expression predicate (Rule Engine)
        public SkillsFramework(IRepository<SkillsFramework> repository)
        {
            _repository = repository;
        }
        #region Implementation of IBusinessRuleEngine

        public Task<SkillsFramework> GetSkillsFrameworkFor(string socCode)
        {
          //  _repository.GetMany(s => s.SocCode == socCode);
            return null;
        }

        #endregion
    }
}
