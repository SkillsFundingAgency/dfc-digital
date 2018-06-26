namespace DFC.Digital.Service.SkillsFramework.Interface
{
    using System.Threading.Tasks;
    using Data.Model;

    public interface IBusinessRuleEngine
    {
        Task<SkillsFramework> GetSkillsFrameworkFor(string socCode);
    }
}